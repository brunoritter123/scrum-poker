import { Route } from '@angular/compiler/src/core';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { PoNotificationService } from '@po-ui/ng-components';
import { promise } from 'protractor';
import { Observable } from 'rxjs';
import { SalaParticipante } from '../models/sala-participante.model';
import { AuthService } from '../services/auth.service';
import { LoadPageService } from '../services/load-page.service';
import { SalaHubService } from '../services/sala-hub.service';
import { SalaService } from '../services/sala.service';
import { SalaParticipanteService } from '../services/sala-participante.service';
import { Sala } from '../models/sala.model';

@Injectable({
  providedIn: 'root'
})
export class SalaGuard implements CanActivate {

  constructor(
    private authService: AuthService,
    private router: Router,
    private salaHubService: SalaHubService,
    private salaService: SalaService,
    private participanteService: SalaParticipanteService,
    private poNotification: PoNotificationService,
    private loadPageService: LoadPageService
    ) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
      return new Promise((resolve, reject) => {
        this.loadPageService.loadPage.emit(true);
        if (!route.params?.salaId) {
          this.poNotification.error('Id da Sala não foi definida');
          this.router.navigate(['']);
          reject();
        }
        const salaId: string = route.params?.salaId;
        let sala: Sala;

        this.salaService.gerarSalaPadrao(salaId)
        .then((salaRecebida) => {
          sala = salaRecebida;
          this.salaService.guardarConfigSalaNoLocalStorage(salaRecebida.configuracao);
          return this.getParticipante(salaId);
        })
        .then((participante) => this.participanteService.incluirOuAlterarParticipante(participante))
        .then(() => this.iniciarConexaoHub(sala))
        .then(() => resolve(true))
        .catch((err) => {
          this.poNotification.error(err);
          this.router.navigate(['']);
          reject();
        })
        .finally(() => this.loadPageService.loadPage.emit(false));
      });
  }

  private iniciarConexaoHub(sala: Sala): Promise<any> {
    return this.salaHubService.startConection(sala, this.authService.idParticipante).catch((err) => {
      console.error(err);
      throw new Error('Erro ao iniciar a conexão com o servidor');
    });
  }

  private getParticipante(salaId: string): Promise<SalaParticipante>{
    return new Promise((resolve, reject) => {
      const participante = new SalaParticipante();
      participante.id = this.authService.idParticipante;
      participante.salaId = salaId;
      participante.nome = this.authService.name;
      participante.jogador = this.authService.isJogador;
      participante.online = true;

      if (!participante.id || !participante.salaId || !participante.nome ||
        participante.jogador === undefined || participante.online === undefined)
      {
          this.authService.setSalaCookie(salaId);
          reject('Dados insuficientes para entrar na sala.');
      }

      resolve(participante);
    });
  }

  private entrarSala(idParticipante: string, salaId: string, isJogador: boolean, nomeParticipante: string): Promise<any> {

    const participante = new SalaParticipante();
    participante.id = idParticipante;
    participante.salaId = salaId;
    participante.nome = nomeParticipante;
    participante.jogador = isJogador;
    participante.online = true;
    return this.salaHubService.enviarParticipante(participante).catch((err) => {
      console.error(err);
      throw new Error('Erro ao tentar entrar na sala: ' + salaId);
    });
  }
}
