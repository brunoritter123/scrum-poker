import { Carta } from './carta.model';

export class Sala {
  public id: string;
  public jogadorFinalizaJogo: boolean;
  public jogadorResetaJogo: boolean;
  public jogadorRemoveJogador: boolean;
  public jogadorRemoveAdministrador: boolean;
  public cartas: Array<Carta>;

  constructor(salaId: string) {
    this.id = salaId,
    this.jogadorFinalizaJogo = false,
    this.jogadorResetaJogo = false,
    this.jogadorRemoveJogador = false,
    this.jogadorRemoveAdministrador = true,
    this.cartas = [
        {id: 0 , idSala: salaId, ordem: 1   , value: '01', especial: false},
        {id: 0 , idSala: salaId, ordem: 2   , value: '02', especial: false},
        {id: 0 , idSala: salaId, ordem: 3   , value: '03', especial: false},
        {id: 0 , idSala: salaId, ordem: 5   , value: '05', especial: false},
        {id: 0 , idSala: salaId, ordem: 8   , value: '08', especial: false},
        {id: 0 , idSala: salaId, ordem: 13  , value: '13', especial: false},
        {id: 0 , idSala: salaId, ordem: 21  , value: '21', especial: false},
        {id: 0 , idSala: salaId, ordem: 34  , value: '34', especial: false},
        {id: 0 , idSala: salaId, ordem: 55  , value: '55', especial: false},
        {id: 0 , idSala: salaId, ordem: 100 , value: '?' , especial: true }
      ]
   }



}
