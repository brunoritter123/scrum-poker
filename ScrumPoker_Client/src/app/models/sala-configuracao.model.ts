import { Carta } from './carta.model';

export class SalaConfiguracao {
  public id: number;
  public salaId: string;
  public jogadorFinalizaJogo: boolean;
  public jogadorResetaJogo: boolean;
  public jogadorRemoveJogador: boolean;
  public jogadorRemoveAdministrador: boolean;
  public cartas: Array<Carta>;
}
