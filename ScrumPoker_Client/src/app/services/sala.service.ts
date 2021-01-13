import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Sala } from '../models/sala.model';
import { PoNotificationService } from '@po-ui/ng-components';

@Injectable({
  providedIn: 'root'
})
export class SalaService {

  private readonly url: string = environment.API;

  constructor(
    private http: HttpClient,
    private poNotification: PoNotificationService
  ) { }

  public buscarSala(salaId: string): Promise<Sala> {
    return this.http.get<Sala>(`${this.url}/sala/${salaId}`)
      .toPromise()
      .catch( erro => {
          this.poNotification.error('Houve um erro ao tentar buscar a sala.');
          console.log(erro);
          throw erro;
      });
  }

  public incluirSalaPadrao(salaId: string): Promise<Sala> {
    return this.http.post<Sala>(`${this.url}/sala/${salaId}`, {})
      .toPromise()
      .catch( erro => {
          this.poNotification.error('Houve um erro ao tentar buscar a sala.');
          console.log(erro);
          throw erro;
      });
  }

  public alterarSala(sala: Sala): Promise<any> {
    return this.http.put(`${this.url}/sala/`, sala)
    .toPromise()
    .then( () => this.poNotification.success('Sala salva com sucesso.'))
    .catch( (erro) => {
      this.poNotification.error('Houve um erro ao tentar salvar a sala.');
      console.log(erro);
      throw erro;
    });
  }

  public excluirCarta(salaId: string): Promise<any> {
    return this.http.delete(`${this.url}/sala/${salaId}/cartas`)
    .toPromise();
  }
}
