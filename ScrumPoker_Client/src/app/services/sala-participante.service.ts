import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { SalaParticipante } from '../models/sala-participante.model';
import { PoNotificationService } from '@po-ui/ng-components';
import { lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SalaParticipanteService {

  private readonly url: string = environment.API;

  constructor(
    private http: HttpClient,
    private poNotification: PoNotificationService
  ) { }

  public incluirOuAlterarParticipante(participante: SalaParticipante): Promise<SalaParticipante> {
    const request = this.http.post<SalaParticipante>(`${this.url}/Participante/`, participante)
    return lastValueFrom(request)
      .catch( erro => {
          this.poNotification.error('Houve um erro ao tentar incluir o participante.');
          console.log(erro);
          throw erro;
      });
  }
}
