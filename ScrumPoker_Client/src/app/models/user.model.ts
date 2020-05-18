import { Carta } from './carta.model';

export class User {
  public idUser: string;
  public socket: string;
  public status: string;
  public nome: string;
  public isJogador: boolean;
  public idCarta: number;
  public voto: Carta;
}
