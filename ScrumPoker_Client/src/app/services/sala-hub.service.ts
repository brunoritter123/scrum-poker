import { EventEmitter, Injectable } from '@angular/core';
import * as singalR from '@aspnet/signalr';
import { ChartModel } from '../interfaces/chartmodel.interface';
import { SalaConfiguracao } from '../models/sala-configuracao.model';
import { SalaParticipante } from '../models/sala-participante.model';
import { Sala } from '../models/sala.model';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class SalaHubService {
  private souJogador: boolean;

  public _administradores: Array<SalaParticipante>;
  public set administradores(v : Array<SalaParticipante>) {
    this._administradores = v;
    this.verificarConfiguracoes();
  }
  public get administradores(): Array<SalaParticipante> {
    return this._administradores;
  }

  public _salaConfig: SalaConfiguracao;
  public set salaConfig(v : SalaConfiguracao) {
    this._salaConfig = v;
    this.verificarConfiguracoes();
  }
  public get salaConfig(): SalaConfiguracao {
    return this._salaConfig;
  }

  public possoFinalizarJogo: boolean;
  public possoResetarJogo: boolean;
  public data: ChartModel[];
  public broadcastedData: ChartModel[];
  public receberConfiguracaoSala = new EventEmitter<SalaConfiguracao>();
  public receberJogadores = new EventEmitter<Array<SalaParticipante>>();
  public receberAdministradores = new EventEmitter<Array<SalaParticipante>>();
  public receberParticipanteRemovido = new EventEmitter<string>();
  public receberSala = new EventEmitter<Sala>();
  public jogadorFinalizaJogo = new EventEmitter<boolean>();
  public jogadorResetaJogo = new EventEmitter<boolean>();

  private hubConnection: singalR.HubConnection;

  constructor(
    private authService: AuthService
    ) {
      this.souJogador = authService.isJogador;
    }


  public startConection(participanteId: string) {
    this.hubConnection = new singalR.HubConnectionBuilder()
    .withUrl(`http://localhost:5000/sala-hub?participante-id=${participanteId}`)
    .build();

    return this.hubConnection.start().then(() => {
      this.receberConfiguracaoSalaHub();
      this.receberJogadoresHub();
      this.receberAdministradoresHub();
      this.receberParticipanteRemovidoHub();
      this.receberSalaHub();
    });
  };

  public stopConection() {
    this.hubConnection.stop();
  };

  private receberConfiguracaoSalaHub(): void {
    this.hubConnection.on('ReceberConfiguracaoSala', (salaconfig: SalaConfiguracao) => {
      this.receberConfiguracaoSala.emit(salaconfig)});
  }

  private receberJogadoresHub(): void {
    this.hubConnection.on('ReceberJogadores', (jogadores: Array<SalaParticipante>) => {
      this.receberJogadores.emit(jogadores)});
  }

  private receberAdministradoresHub(): void {
    this.hubConnection.on('ReceberAdministradores', (administradores: Array<SalaParticipante>) => {
      this.administradores = administradores;
      this.receberAdministradores.emit(administradores)});
  }

  public enviarConfiguracaoSala(salaConfig: SalaConfiguracao): Promise<any> {
    this.salaConfig = salaConfig;
    return this.hubConnection.invoke('EnviarConfiguracaoSala', salaConfig)
  }

  public enviarResetarSala(salaId: string): Promise<any> {
    return this.hubConnection.invoke('ResetarSala', salaId)
  }

  public enviarRevotarSala(salaId: string): Promise<any> {
    return this.hubConnection.invoke('RevotarSala', salaId)
  }

  public enviarConcluirSala(salaId: string): Promise<any> {
    return this.hubConnection.invoke('ConcluirSala', salaId)
  }

  public enviarVoto(votoValor: string): Promise<any> {
    return this.hubConnection.invoke('EnviarVoto', votoValor)
  }

  public enviarParticipante(participante: SalaParticipante): Promise<any> {
    return this.hubConnection.invoke('EnviarParticipante', participante)
  }

  public removerParticipante(participanteId: string): Promise<any> {
    return this.hubConnection.invoke('RemoverParticipante', participanteId, this.authService.name)
  }

  private receberParticipanteRemovidoHub(): void {
    this.hubConnection.on('ParticipanteRemovido', (nomeParticipanteQueRemoveu: string) => {
      this.receberParticipanteRemovido.emit(nomeParticipanteQueRemoveu)
    });
  }

  private receberSalaHub(): void {
    this.hubConnection.on('ReceberSala', (sala: Sala) => {
      this.receberSala.emit(sala)
    });
  }

  public enviarFinalizarJogo(salaId: string) {
    return this.hubConnection.invoke('FinalizarJogo', salaId)
  }

  private verificarConfiguracoes(): void {
    if(!this.administradores || !this.salaConfig)
      return;

    this.possoFinalizarJogo = !this.souJogador || this.administradores.length == 0 || this.salaConfig.jogadorFinalizaJogo;
    this.possoResetarJogo = !this.souJogador || this.administradores.length == 0 || this.salaConfig.jogadorResetaJogo;
    this.jogadorFinalizaJogo.emit(this.possoFinalizarJogo);
    this.jogadorResetaJogo.emit(this.possoResetarJogo);
  }
}
