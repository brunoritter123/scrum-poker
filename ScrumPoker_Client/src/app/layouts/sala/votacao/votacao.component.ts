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
  templateUrl: './votacao.component.html',
  styleUrls: ['./votacao.component.css']
})
export class VotacaoComponent implements OnInit, OnDestroy {
  public souJogador: boolean;
  public possoRestarJogo: boolean;
  public possoFinalizarJogo: boolean;

  public sala: Sala;

  public _salaConfig: SalaConfiguracao;
  public set salaConfig(v : SalaConfiguracao) {
    this._salaConfig = v;
    this.selecionaCartaVotada();
    this.verificarConfiguracoes();
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

  private _administradores: Array<SalaParticipante>;
  private set administradores(v: Array<SalaParticipante>){
    this._administradores = v;
    this.verificarConfiguracoes();
  }
  private get administradores(): Array<SalaParticipante> {
    return this._administradores
  }

  private estouConectado: boolean = true;
  private meuIdParticipante: string;
  private inscricaoSalaconfiguracao: Subscription;
  private inscricaoReceberAdministradores: Subscription;
  private inscricaoReceberJogaodoes: Subscription;
  private inscricaoReceberSala: Subscription;

  constructor(
    private activatedRoute: ActivatedRoute,
    private authService: AuthService,
    private salaHubService: SalaHubService
    ) {
      this.sala = this.activatedRoute.snapshot.data['sala'];
      this.salaConfig = this.sala.configuracao;
      this.administradores = this.sala.administradores;
      this.meuVotoValue = this.sala.jogadores.find(jogador => jogador.id == this.meuIdParticipante)?.votoCartaValor;
      this.meuIdParticipante = authService.idParticipante;
      this.souJogador = authService.isJogador;
     }

  ngOnInit(): void {
    this.inscricaoReceberSala = this.salaHubService.receberSala.subscribe(x => this.onReceberSala(x));
    this.inscricaoSalaconfiguracao = this.salaHubService.receberConfiguracaoSala.subscribe(x => this.onReceberConfiguracaoSala(x));
    this.inscricaoReceberAdministradores = this.salaHubService.receberAdministradores.subscribe(x => this.onReceberAdministradores(x));
    this.inscricaoReceberJogaodoes = this.salaHubService.receberJogadores.subscribe(x => this.onReceberJogadores(x));
  }

  ngOnDestroy(): void {
    this.inscricaoSalaconfiguracao.unsubscribe();
    this.inscricaoReceberAdministradores.unsubscribe();
    this.inscricaoReceberJogaodoes.unsubscribe();
    this.inscricaoReceberSala.unsubscribe();
  }

  private onReceberConfiguracaoSala(salaConfig: SalaConfiguracao): void {
    this.salaConfig = salaConfig;
  }

  private onReceberAdministradores(administradores: Array<SalaParticipante>): void {
    this.administradores = administradores;
  }

  private onReceberJogadores(jogadores: Array<SalaParticipante>): void {
    this.meuVotoValue = jogadores.find(jogador => jogador.id == this.meuIdParticipante)?.votoCartaValor;
  }

  private onReceberSala(sala: Sala): void {
    this.sala = sala;
  }

  public votar(carta: Carta): void {
    if (!this.sala.jogoFinalizado && !!carta && this.estouConectado) {
      this.meuVotoValue = carta.typeButton == 'danger' ? null : carta.value;
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
      if (carta.value == this.meuVotoValue){
        carta.typeButton = 'danger';
      } else {
        carta.typeButton = 'default';
      }
    })
  }

  private verificarConfiguracoes(): void {
    this.possoFinalizarJogo = !this.souJogador || this.administradores.length == 0 || this.salaConfig.jogadorFinalizaJogo;
    this.possoRestarJogo = !this.souJogador || this.administradores.length == 0 || this.salaConfig.jogadorResetaJogo;
  }

}
