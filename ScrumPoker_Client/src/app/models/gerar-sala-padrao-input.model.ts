import { SalaConfiguracao } from "./sala-configuracao.model";

export class GerarSalaPadraoInput {
  public id = '';
  public configuracao: SalaConfiguracao = new SalaConfiguracao();
}
