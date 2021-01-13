import { SalaConfiguracao } from './sala-configuracao.model';
import { SalaParticipante } from './sala-participante.model';

export class Sala {
  public id = '';
  public titulo = '';
  public jogoFinalizado = false;
  public configuracao: SalaConfiguracao = new SalaConfiguracao();
  public jogadores: Array<SalaParticipante> = [];
  public administradores: Array<SalaParticipante> = [];
}
