import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Perfil } from '../interfaces/perfil.interface';
import { PoNotificationService } from '@po-ui/ng-components';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class PerfilService {

  private readonly url: string = environment.API;

  constructor(
    private http: HttpClient,
    private poNotification: PoNotificationService,
    private authService: AuthService
  ) { }

  public buscarPerfil(userName: string): Promise<Perfil> {
      return this.http.get<Perfil>(`${this.url}/perfil/${userName}`)
      .toPromise();
  }

  public alterarPerfil(perfil: Perfil): Promise<any> {
    return this.http.put(`${this.url}/perfil/`, perfil)
    .toPromise()
    .then( () => {
      this.poNotification.success('Perfil salvo com sucesso.');
      this.authService.getProfile();
    })
    .catch( (erro) => {
      this.poNotification.error('Houve um erro ao tentar salvar a perfil.');
      console.log(erro);
      throw erro;
    });
  }

}
