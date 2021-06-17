import { Component, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { Carta } from 'src/app/models/carta.model';
import { SalaConfiguracao } from 'src/app/models/sala-configuracao.model';
import { SalaParticipante } from 'src/app/models/sala-participante.model';
import { Sala } from 'src/app/models/sala.model';
import { Voto } from 'src/app/models/voto.model';
import { AuthService } from 'src/app/services/auth.service';
import { SalaHubService } from 'src/app/services/sala-hub.service';

@Component({
  selector: 'app-votacao',
  templateUrl: './votacao.component.html'
})
export class VotacaoComponent implements OnDestroy {
  public souJogador: boolean;
  public possoResetarJogo: boolean;
  public possoFinalizarJogo: boolean;

  public sala: Sala;

  private pSalaConfig?: SalaConfiguracao;
  public set salaConfig(v: SalaConfiguracao | undefined) {
    this.pSalaConfig = v;
    this.selecionaCartaVotada();
  }
  public get salaConfig(): SalaConfiguracao | undefined {
    return this.pSalaConfig;
  }

  private pMeuVotoValue = '';
  public set meuVotoValue(v: string) {
    this.pMeuVotoValue = v;
    this.selecionaCartaVotada();
  }
  public get meuVotoValue(): string {
    return this.pMeuVotoValue;
  }

  private estouConectado = true;
  private meuIdParticipante: string;
  private inscricaoSalaconfiguracao: Subscription;
  private inscricaoReceberJogaodoes: Subscription;
  private inscricaoJogadorFinalizaJogo: Subscription;
  private inscricaoJogadorResetaJogo: Subscription;
  private inscricaoReceberVoto: Subscription;

  constructor(
    private activatedRoute: ActivatedRoute,
    private authService: AuthService,
    private salaHubService: SalaHubService
    ) {
      const keySala = 'sala';
      this.sala = this.activatedRoute.snapshot.data[keySala];
      this.meuIdParticipante = authService.idParticipante;
      this.salaConfig = this.sala.configuracao;
      const meuVotoValue = this.sala.jogadores.find(jogador => jogador.id === this.meuIdParticipante)?.votoCartaValor;
      this.meuVotoValue = !!meuVotoValue ? meuVotoValue : '';
      this.meuIdParticipante = authService.idParticipante;
      this.souJogador = authService.isJogador;
      this.possoResetarJogo = salaHubService.possoResetarJogo;
      this.possoFinalizarJogo = salaHubService.possoFinalizarJogo;

      this.inscricaoSalaconfiguracao = this.salaHubService.receberConfiguracaoSala.subscribe((x: any) => this.onReceberConfiguracaoSala(x));
      this.inscricaoReceberJogaodoes = this.salaHubService.receberJogadores.subscribe((x: any) => this.onReceberJogadores(x));
      this.inscricaoJogadorFinalizaJogo = this.salaHubService.jogadorFinalizaJogo.subscribe((x: any) => this.possoFinalizarJogo = x);
      this.inscricaoJogadorResetaJogo = this.salaHubService.jogadorResetaJogo.subscribe((x: any) => this.possoResetarJogo = x);
      this.inscricaoReceberVoto = this.salaHubService.receberVoto.subscribe((x: Voto) => this.onReceberVoto(x));
    }

  ngOnDestroy(): void {
    this.inscricaoSalaconfiguracao.unsubscribe();
    this.inscricaoReceberJogaodoes.unsubscribe();
    this.inscricaoJogadorFinalizaJogo.unsubscribe();
    this.inscricaoJogadorResetaJogo.unsubscribe();
    this.inscricaoReceberVoto.unsubscribe();
  }

  private onReceberConfiguracaoSala(salaConfig: SalaConfiguracao): void {
    this.salaConfig = salaConfig;
  }

  private onReceberJogadores(jogadores: Array<SalaParticipante>): void {
    const meuVotoValue = jogadores.find(jogador => jogador.id === this.meuIdParticipante)?.votoCartaValor;
    this.meuVotoValue = !!meuVotoValue ? meuVotoValue : '';
  }

  private onReceberVoto(voto: Voto): void {
    this.meuVotoValue = voto.jogadorId === this.meuIdParticipante ? voto.valorVoto : '';
  }

  public votar(carta: Carta): void {
    if (!this.sala.jogoFinalizado && !!carta && this.estouConectado) {
      this.meuVotoValue = carta.selecionada ? '' : carta.value;
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
    this.salaConfig?.cartas.forEach( carta => {
      carta.selecionada = carta.value === this.meuVotoValue;
    });
  }
}
