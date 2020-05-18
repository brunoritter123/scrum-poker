import { Carta } from './carta.model';

export class Sala {
  constructor(
    public id: string,
    public jogadorFinalizaJogo: boolean = false,
    public jogadorResetaJogo: boolean = false,
    public jogadorRemoveJogador: boolean = false,
    public jogadorRemoveAdministrador: boolean = true,
    public cartas: Array<Carta> =
      [ {id: 0 , idSala: id, ordem: 1   , value: '01', especial: false},
        {id: 0 , idSala: id, ordem: 2   , value: '02', especial: false},
        {id: 0 , idSala: id, ordem: 3   , value: '03', especial: false},
        {id: 0 , idSala: id, ordem: 5   , value: '05', especial: false},
        {id: 0 , idSala: id, ordem: 8   , value: '08', especial: false},
        {id: 0 , idSala: id, ordem: 13  , value: '13', especial: false},
        {id: 0 , idSala: id, ordem: 21  , value: '21', especial: false},
        {id: 0 , idSala: id, ordem: 34  , value: '34', especial: false},
        {id: 0 , idSala: id, ordem: 55  , value: '55', especial: false},
        {id: 0 , idSala: id, ordem: 100 , value: '?' , especial: true }
      ]

  ) { }



}
