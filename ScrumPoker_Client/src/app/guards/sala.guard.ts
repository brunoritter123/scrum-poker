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
        let salaId: string = route.params['salaId'];

        this.salaService.incluirSalaPadrao(salaId)
        .then(() => {return this.getParticipante(salaId)})
        .then((participante) => {return this.participanteService.incluirOuAlterarParticipante(participante)})
        .then(() => {return this.iniciarConexaoHub()})
        .then(() => resolve(true))
        .catch((err) => {
          this.poNotification.error(err)
          this.router.navigate(['']);
          reject();
        })
        .finally(() => this.loadPageService.loadPage.emit(false))
      })

    // let idParticipante = this.authService.idParticipante;
    // let isJogador = this.authService.isJogador;
    // let nomeParticipante = this.authService.name;
    // let salaId: string = route.params['salaId']

    // if (!idParticipante || !nomeParticipante || isJogador === undefined) {
    //   return new Promise((resolve, reject) => {
    //     this.router.navigate([''])
    //     reject()
    //   })
    // }

    // return new Promise((resolve, reject) => {
    //   this.iniciarConexaoHub()
    //   .then(() => {
    //     this.criarSalaPadrao(salaId)
    //     .then(() => {
    //       this.entrarSala(idParticipante, salaId, isJogador, nomeParticipante)
    //       .then(() => {
    //         this.loadPageService.loadPage.emit(false);
    //         resolve(true);
    //       })
    //     })
    //   })
    // });
  }

  private iniciarConexaoHub(): Promise<any> {
    return this.salaHubService.startConection(this.authService.idParticipante).catch((err) => {
      // this.poNotification.error('Erro ao iniciar a conexão com o servidor')
      // this.loadPageService.loadPage.emit(false);
      // this.router.navigate(['']);
      console.error(err)
      throw new Error('Erro ao iniciar a conexão com o servidor');
    });
  }

  private getParticipante(salaId: string): Promise<SalaParticipante>{
    return new Promise((resolve, reject) => {
      let participante = new SalaParticipante();
      participante.id = this.authService.idParticipante;
      participante.salaId = salaId;
      participante.nome = this.authService.name;
      participante.jogador = this.authService.isJogador;
      participante.online = true;

      if(!participante.id || !participante.salaId || !participante.nome ||
        participante.jogador === undefined || participante.online === undefined)
      {
          reject('Dados insuficientes para entrar na sala.')
      }

      resolve(participante)
    })
  }

  private entrarSala(idParticipante: string, salaId: string, isJogador: boolean, nomeParticipante: string): Promise<any> {

    let participante = new SalaParticipante();
    participante.id = idParticipante;
    participante.salaId = salaId;
    participante.nome = nomeParticipante;
    participante.jogador = isJogador;
    participante.online = true;
    return this.salaHubService.enviarParticipante(participante).catch((err) => {
      // this.poNotification.error("Erro ao tentar entrar na sala: " + salaId)
      // this.router.navigate(['']);
      // this.loadPageService.loadPage.emit(false);
      console.error(err)
      throw new Error('Erro ao tentar entrar na sala: ' + salaId)
    });
  }



  private criarSalaPadrao(salaId: string): Promise<any> {
    return this.salaService.incluirSalaPadrao(salaId).catch((err) => {
      // this.poNotification.error('Erro ao tentar incluir uma sala padrão')
      // this.loadPageService.loadPage.emit(false);
      // this.router.navigate(['']);
      console.error(err)
      throw new Error('Erro ao tentar incluir uma sala padrão')
    })
  }
}
