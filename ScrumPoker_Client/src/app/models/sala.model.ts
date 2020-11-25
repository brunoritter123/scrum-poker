import { SalaConfiguracao } from './sala-configuracao.model';
import { SalaParticipante } from './sala-participante.model';

export class Sala {
  public id: string;
  public jogoFinalizado: boolean;
  public configuracao: SalaConfiguracao;
  public jogadores: Array<SalaParticipante>;
  public administradores: Array<SalaParticipante>;
}
