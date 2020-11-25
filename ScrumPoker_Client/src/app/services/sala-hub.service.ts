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
  public data: ChartModel[];
  public broadcastedData: ChartModel[];
  public receberConfiguracaoSala = new EventEmitter<SalaConfiguracao>();
  public receberJogadores = new EventEmitter<Array<SalaParticipante>>();
  public receberAdministradores = new EventEmitter<Array<SalaParticipante>>();
  public receberParticipanteRemovido = new EventEmitter<string>();

  private hubConnection: singalR.HubConnection;

  constructor(
    private authService: AuthService
    ) { }


  public startConection(participanteId: string) {
    this.hubConnection = new singalR.HubConnectionBuilder()
    .withUrl(`http://localhost:5000/sala-hub?participante-id=${participanteId}`)
    .build();

    return this.hubConnection.start().then(() => {
      this.receberConfiguracaoSalaHub();
      this.receberJogadoresHub();
      this.receberAdministradoresHub();
      this.receberParticipanteRemovidoHub();
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
      this.receberAdministradores.emit(administradores)});
  }

  public enviarConfiguracaoSala(salaConfig: SalaConfiguracao): Promise<any> {
    return this.hubConnection.invoke('EnviarConfiguracaoSala', salaConfig)
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
}
