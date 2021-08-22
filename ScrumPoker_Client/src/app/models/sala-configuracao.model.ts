import { Carta } from './carta.model';

export class SalaConfiguracao {
  public id?: number;
  public salaId = '';
  public jogadorFinalizaJogo = false;
  public jogadorResetaJogo = false;
  public jogadorRemoveJogador = false;
  public jogadorRemoveAdministrador = true;
  public cartas: Array<Carta> = [
    new Carta(undefined, 1 , '01', false, false),
    new Carta(undefined, 2 , '02', false, false),
    new Carta(undefined, 3 , '03', false, false),
    new Carta(undefined, 4 , '05', false, false),
    new Carta(undefined, 5 , '08', false, false),
    new Carta(undefined, 6 , '13', false, false),
    new Carta(undefined, 7 , '21', false, false),
    new Carta(undefined, 8 , '34', false, false),
    new Carta(undefined, 9 , '55', false, false),
    new Carta(undefined, 10, '?' , true , false)
  ];
}
