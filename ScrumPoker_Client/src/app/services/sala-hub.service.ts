import { EventEmitter, Injectable } from '@angular/core';
import * as singalR from '@microsoft/signalr';
import { SalaConfiguracao } from '../models/sala-configuracao.model';
import { SalaParticipante } from '../models/sala-participante.model';
import { Sala } from '../models/sala.model';
import { AuthService } from './auth.service';
import { environment } from '../../environments/environment';
import { SalaService } from './sala.service';

@Injectable({
  providedIn: 'root'
})
export class SalaHubService {
  private souJogador: boolean;

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
      this.souJogador = authService.isJogador;
    }


  public startConection(participanteId: string): Promise<void> {
    const url = environment.SOCKET;

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
      this.receberConfiguracaoSala.emit(salaconfig);
    });
  }

  private receberJogadoresHub(): void {
    this.hubConnection.on('ReceberJogadores', (jogadores: Array<SalaParticipante>) => {
      this.receberJogadores.emit(jogadores);
    });
  }

  private receberAdministradoresHub(): void {
    this.hubConnection.on('ReceberAdministradores', (administradores: Array<SalaParticipante>) => {
      this.administradores = administradores;
      this.receberAdministradores.emit(administradores);
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
    this.hubConnection.on('ParticipanteRemovido', (nomeParticipanteQueRemoveu: string) => {
      this.receberParticipanteRemovido.emit(nomeParticipanteQueRemoveu);
    });
  }

  private receberSalaHub(): void {
    this.hubConnection.on('ReceberSala', (sala: Sala) => {
      this.receberSala.emit(sala);
    });
  }

  public enviarFinalizarJogo(salaId: string): Promise<any> {
    return this.hubConnection.invoke('FinalizarJogo', salaId);
  }

  private verificarConfiguracoes(): void {
    if (!this.administradores || !this.salaConfig){
      return;
    }

    this.possoFinalizarJogo = !this.souJogador || this.administradores.length === 0 || this.salaConfig.jogadorFinalizaJogo;
    this.possoResetarJogo = !this.souJogador || this.administradores.length === 0 || this.salaConfig.jogadorResetaJogo;
    this.jogadorFinalizaJogo.emit(this.possoFinalizarJogo);
    this.jogadorResetaJogo.emit(this.possoResetarJogo);
  }
}
