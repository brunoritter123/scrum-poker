export class Carta {

  constructor(
    public id: number = 0,
    public idSala: string = '',
    public ordem: number,
    public value: string = '',
    public especial: boolean = false,
    public selecionada: boolean = false,
  ) {}

}
