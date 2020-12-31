import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { Carta } from 'src/app/models/carta.model';
import { SalaConfiguracao } from 'src/app/models/sala-configuracao.model';
import { SalaParticipante } from 'src/app/models/sala-participante.model';
import { Sala } from 'src/app/models/sala.model';
import { AuthService } from 'src/app/services/auth.service';
import { SalaHubService } from 'src/app/services/sala-hub.service';

@Component({
  selector: 'app-votacao',
  templateUrl: './votacao.component.html'
})
export class VotacaoComponent implements OnInit, OnDestroy {
  public souJogador: boolean;
  public possoResetarJogo: boolean;
  public possoFinalizarJogo: boolean;

  public sala: Sala;

  public _salaConfig: SalaConfiguracao;
  public set salaConfig(v : SalaConfiguracao) {
    this._salaConfig = v;
    this.selecionaCartaVotada();
  }
  public get salaConfig(): SalaConfiguracao {
    return this._salaConfig;
  }

  public _meuVotoValue: string;
  public set meuVotoValue(v : string) {
    this._meuVotoValue = v;
    this.selecionaCartaVotada();
  }
  public get meuVotoValue(): string {
    return this._meuVotoValue;
  }

  private estouConectado: boolean = true;
  private meuIdParticipante: string;
  private inscricaoSalaconfiguracao: Subscription;
  private inscricaoReceberJogaodoes: Subscription;
  private inscricaoJogadorFinalizaJogo: Subscription;
  private inscricaoJogadorResetaJogo: Subscription;

  constructor(
    private activatedRoute: ActivatedRoute,
    private authService: AuthService,
    private salaHubService: SalaHubService
    ) {
      this.sala = this.activatedRoute.snapshot.data['sala'];
      this.salaConfig = this.sala.configuracao;
      this.meuVotoValue = this.sala.jogadores.find(jogador => jogador.id == this.meuIdParticipante)?.votoCartaValor;
      this.meuIdParticipante = authService.idParticipante;
      this.souJogador = authService.isJogador;
      this.possoResetarJogo = salaHubService.possoResetarJogo;
      this.possoFinalizarJogo = salaHubService.possoFinalizarJogo;
     }

  ngOnInit(): void {
    this.inscricaoSalaconfiguracao = this.salaHubService.receberConfiguracaoSala.subscribe(x => this.onReceberConfiguracaoSala(x));
    this.inscricaoReceberJogaodoes = this.salaHubService.receberJogadores.subscribe(x => this.onReceberJogadores(x));
    this.inscricaoJogadorFinalizaJogo = this.salaHubService.jogadorFinalizaJogo.subscribe(x => this.possoFinalizarJogo = x);
    this.inscricaoJogadorResetaJogo = this.salaHubService.jogadorResetaJogo.subscribe(x => this.possoResetarJogo = x);
  }

  ngOnDestroy(): void {
    this.inscricaoSalaconfiguracao.unsubscribe();
    this.inscricaoReceberJogaodoes.unsubscribe();
    this.inscricaoJogadorFinalizaJogo.unsubscribe();
    this.inscricaoJogadorResetaJogo.unsubscribe();
  }

  private onReceberConfiguracaoSala(salaConfig: SalaConfiguracao): void {
    this.salaConfig = salaConfig;
  }

  private onReceberJogadores(jogadores: Array<SalaParticipante>): void {
    this.meuVotoValue = jogadores.find(jogador => jogador.id == this.meuIdParticipante)?.votoCartaValor;
  }

  public votar(carta: Carta): void {
    if (!this.sala.jogoFinalizado && !!carta && this.estouConectado) {
      this.meuVotoValue = carta.selecionada ? null : carta.value;
      this.salaHubService.enviarVoto(this.meuVotoValue);
    }
  }

  public resetarJogo(): void {
    this.salaHubService.enviarResetarSala(this.sala.id);
  }

  public finalizarJogo(): void {
    this.salaHubService.enviarFinalizarJogo(this.sala.id);
  }

  private selecionaCartaVotada(): void {
    this.salaConfig.cartas.forEach( carta => {
      carta.selecionada = carta.value == this.meuVotoValue;
    })
  }
}
