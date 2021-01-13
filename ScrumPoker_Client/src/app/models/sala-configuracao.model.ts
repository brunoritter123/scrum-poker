import { Carta } from './carta.model';

export class SalaConfiguracao {
  public id?: number;
  public salaId = '';
  public jogadorFinalizaJogo = false;
  public jogadorResetaJogo = false;
  public jogadorRemoveJogador = false;
  public jogadorRemoveAdministrador = false;
  public cartas: Array<Carta> = [];
}
