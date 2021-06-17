import { SalaConfiguracao } from "./sala-configuracao.model";

export class GerarSalaPadraoInput {
  public id = '';
  public salaConfiguracao: SalaConfiguracao | null = new SalaConfiguracao();
}
