import { EventEmitter, Injectable } from '@angular/core';
import * as singalR from '@microsoft/signalr';
import { SalaConfiguracao } from '../models/sala-configuracao.model';
import { SalaParticipante } from '../models/sala-participante.model';
import { Sala } from '../models/sala.model';
import { AuthService } from './auth.service';
import { environment } from '../../environments/environment';
import { SalaService } from './sala.service';
import { Voto } from '../models/voto.model';
import { ParticipanteRemovido} from '../models/participante-removido';

@Injectable({
  providedIn: 'root'
})
export class SalaHubService {
  private jogadores: Array<SalaParticipante> = [];
  private pAdministradores: Array<SalaParticipante> = [];
  public set administradores(v: Array<SalaParticipante>) {
    this.pAdministradores = v;
    this.verificarConfiguracoes();
  }
  public get administradores(): Array<SalaParticipante> {
    return this.pAdministradores;
  }

  private pSalaConfig: SalaConfiguracao | undefined;
  public set salaConfig(v: SalaConfiguracao | undefined) {
    this.pSalaConfig = v;
    this.verificarConfiguracoes();
  }
  public get salaConfig(): SalaConfiguracao | undefined{
    return this.pSalaConfig;
  }

  public possoFinalizarJogo = false;
  public possoResetarJogo = false;
  public receberConfiguracaoSala = new EventEmitter<SalaConfiguracao>();
  public receberJogadores = new EventEmitter<Array<SalaParticipante>>();
  public receberAdministradores = new EventEmitter<Array<SalaParticipante>>();
  public receberParticipanteRemovido = new EventEmitter<string>();
  public receberSala = new EventEmitter<Sala>();
  public receberVoto = new EventEmitter<Voto>();
  public jogadorFinalizaJogo = new EventEmitter<boolean>();
  public jogadorResetaJogo = new EventEmitter<boolean>();

  public onReconectado = new EventEmitter<any>();
  public onReconectando = new EventEmitter<any>();
  public onPercaDeConexao = new EventEmitter<any>();

  private hubConnection!: singalR.HubConnection;

  constructor(
    private authService: AuthService,
    private salaService: SalaService
    ) {
    }


  public startConection(sala: Sala, participanteId: string): Promise<void> {
    const url = environment.SOCKET;
    this.salaConfig = sala.configuracao;
    this.administradores = sala.administradores;
    this.jogadores = sala.jogadores;

    this.hubConnection = new singalR.HubConnectionBuilder()
    .withUrl(`${url}/sala-hub?participante-id=${participanteId}`)
    .withAutomaticReconnect()
    .build();

    this.hubConnection.onclose(x => this.onCloseConection(x));
    this.hubConnection.onreconnecting(x => this.onReconnectingConection(x));
    this.hubConnection.onreconnected(x => this.onReconnectedConection(x));

    return this.hubConnection.start().then(() => {
      this.receberConfiguracaoSalaHub();
      this.receberJogadoresHub();
      this.receberAdministradoresHub();
      this.receberParticipanteRemovidoHub();
      this.receberSalaHub();
      this.receberVotoHub();
      this.receberNovoJogadorHub();
      this.receberNovoAdministradorHub();
      this.recberJogadorDesconectado();
      this.recberAdministradorDesconectado();
    });
  }

  private onCloseConection(error: Error | undefined): void {
    this.onPercaDeConexao.emit();
  }

  private onReconnectedConection(error: string | undefined): void {
    this.salaService.buscarSala(this.salaConfig!.salaId)
    .then((sala) => {
      this.receberSala.emit(sala);
      this.receberConfiguracaoSala.emit(sala.configuracao);
      this.receberJogadores.emit(sala.jogadores);
      this.receberAdministradores.emit(sala.administradores);
      this.receberAdministradores.emit(sala.administradores);
      this.onReconectado.emit();
    })
  }

  private onReconnectingConection(error: Error | undefined): void {
    this.onReconectando.emit();
  }

  public stopConection(): void {
    this.hubConnection.stop();
  }

  private receberConfiguracaoSalaHub(): void {
    this.hubConnection.on('ReceberConfiguracaoSala', (salaconfig: SalaConfiguracao) => {
      this.salaConfig = salaconfig;
      this.receberConfiguracaoSala.emit(salaconfig);
    });
  }

  private recberJogadorDesconectado(): void {
    this.hubConnection.on('RecberJogadorDesconectado', (jogadorId: string) => {
      const jogador = this.jogadores.find(x => x.id === jogadorId);
      if(!!jogador) jogador.online = false;

      this.receberJogadores.emit(this.jogadores);
    });
  }

  private recberAdministradorDesconectado(): void {
    this.hubConnection.on('RecberAdministradorDesconectado', (administradorId: string) => {
      const administrador = this.administradores.find(x => x.id === administradorId);
      if(!!administrador) administrador.online = false;
      this.receberAdministradores.emit(this.administradores);
    });
  }

  private receberNovoJogadorHub(): void {
    this.hubConnection.on('ReceberNovoJogador', (jogador: SalaParticipante) => {
      this.jogadores = this.jogadores.filter(x => x.id !== jogador.id)
      this.administradores = this.administradores.filter(x => x.id !== jogador.id)

      this.jogadores.push(jogador);
      this.receberJogadores.emit(this.jogadores);
      this.receberAdministradores.emit(this.administradores);
    });
  }

  private receberNovoAdministradorHub(): void {
    this.hubConnection.on('ReceberNovoAdministrador', (administrador: SalaParticipante) => {
      this.jogadores = this.jogadores.filter(x => x.id !== administrador.id)
      this.administradores = this.administradores.filter(x => x.id !== administrador.id)

      this.administradores.push(administrador)
      this.receberJogadores.emit(this.jogadores);
      this.receberAdministradores.emit(this.administradores);
    });
  }

  private receberJogadoresHub(): void {
    this.hubConnection.on('ReceberJogadores', (jogadores: Array<SalaParticipante>) => {
      this.jogadores = jogadores;
      this.receberJogadores.emit(this.jogadores);
    });
  }

  private receberAdministradoresHub(): void {
    this.hubConnection.on('ReceberAdministradores', (administradores: Array<SalaParticipante>) => {
      this.administradores = administradores;
      this.receberAdministradores.emit(this.administradores);
    });
  }

  public enviarConfiguracaoSala(salaConfig: SalaConfiguracao): Promise<any> {
    this.salaConfig = salaConfig;
    return this.hubConnection.invoke('EnviarConfiguracaoSala', salaConfig);
  }

  public enviarResetarSala(salaId: string): Promise<any> {
    return this.hubConnection.invoke('ResetarSala', salaId);
  }

  public enviarRevotarSala(salaId: string): Promise<any> {
    return this.hubConnection.invoke('RevotarSala', salaId);
  }

  public enviarConcluirSala(salaId: string): Promise<any> {
    return this.hubConnection.invoke('ConcluirSala', salaId);
  }

  public enviarVoto(votoValor: string): Promise<any> {
    return this.hubConnection.invoke('EnviarVoto', votoValor);
  }

  public enviarParticipante(participante: SalaParticipante): Promise<any> {
    return this.hubConnection.invoke('EnviarParticipante', participante);
  }

  public removerParticipante(participanteId: string): Promise<any> {
    return this.hubConnection.invoke('RemoverParticipante', participanteId, this.authService.name);
  }

  private receberParticipanteRemovidoHub(): void {
    this.hubConnection.on('ParticipanteRemovido', (nomeParticipanteQueRemoveu: string, participanteRemovidoId: string) => {

      if (this.authService.idParticipante === participanteRemovidoId) {
        this.receberParticipanteRemovido.emit(nomeParticipanteQueRemoveu);
        return

      } else {

        this.jogadores = this.jogadores.filter(x => x.id !== participanteRemovidoId);
        this.receberJogadores.emit(this.jogadores);

        this.administradores = this.administradores.filter(x => x.id !== participanteRemovidoId);
        this.receberAdministradores.emit(this.administradores);
        return
      }
    });
  }

  private receberSalaHub(): void {
    this.hubConnection.on('ReceberSala', (sala: Sala) => {
      this.salaConfig = sala.configuracao;
      this.receberSala.emit(sala);
    });
  }

  private receberVotoHub(): void {
    this.hubConnection.on('ReceberVoto', (voto: Voto) => {
      this.receberVoto.emit(voto);
    });
  }

  public enviarFinalizarJogo(salaId: string): Promise<any> {
    return this.hubConnection.invoke('FinalizarJogo', salaId);
  }

  private verificarConfiguracoes(): void {
    if (!this.administradores || !this.salaConfig){
      return;
    }

    this.possoFinalizarJogo = !this.authService.isJogador || this.administradores.length === 0 || this.salaConfig.jogadorFinalizaJogo;
    this.possoResetarJogo = !this.authService.isJogador || this.administradores.length === 0 || this.salaConfig.jogadorResetaJogo;
    this.jogadorFinalizaJogo.emit(this.possoFinalizarJogo);
    this.jogadorResetaJogo.emit(this.possoResetarJogo);
  }
}
