import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';

import { PoMenuItem, PoModalComponent, PoModalAction, PoToolbarAction, PoToolbarProfile } from '@po-ui/ng-components';

import { AuthService } from './services/auth.service';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { LoadPageService } from './services/load-page.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {
  @ViewChild('modalSobre', { static: true }) modalSobre: PoModalComponent;

  private inscricaoLogin: Subscription;
  private inscricaoLogout: Subscription;
  private inscricaoPerfil: Subscription;
  private inscricaoLoadSala: Subscription;

  public title = 'Scrum Poker';
  public carregando = false;

  private profActLogin: PoToolbarAction = {
    icon: 'po-icon-user',
    label: 'Login',
    separator: true,
    url: 'login'
  };
  private profActSair: PoToolbarAction = {
    icon: 'po-icon-exit',
    label: 'Sair',
    type: 'danger',
    separator: true,
    action: () => this.authService.logout()
  };
  private profActAlterar: PoToolbarAction = {
    icon: 'po-icon-user',
    label: 'Editar Perfil',
    url: 'editar-perfil'
  }

  public profileActions: Array<PoToolbarAction> = [];
  public profile: PoToolbarProfile = undefined;



  public primaryActionSobre: PoModalAction = {
    action: () => {
      this.modalSobre.close()
    },
    label: 'Fechar'
  };

  constructor(
    private authService: AuthService,
    private router: Router,
    private loadPageService: LoadPageService
    ) {
  }

  ngOnInit(){
    if (this.authService.logado()) {
      this.eventLogin();
      this.profile = this.authService.profileLogado;
    } else {
      this.eventLogout();
    }

    this.inscricaoLogin = this.authService.eventLogin.subscribe(() => this.eventLogin());
    this.inscricaoLogout = this.authService.eventLogout.subscribe(() => this.eventLogout());
    this.inscricaoPerfil = this.authService.eventPerfil.subscribe( profile => { this.profile=profile });
    this.inscricaoLoadSala = this.loadPageService.loadPage.subscribe((load) => this.carregando = load);
  }

  ngOnDestroy() {
    this.inscricaoLogin.unsubscribe();
    this.inscricaoLogout.unsubscribe();
    this.inscricaoPerfil.unsubscribe();
  }

  private eventLogin() {
    this.profileActions = [this.profActAlterar, this.profActSair];
  }

  private eventLogout() {
    this.profile = undefined;
    this.profileActions = [this.profActLogin];
  }

  public menus: Array<PoMenuItem> = [
    { label: 'Início', link: '/', icon: 'po-icon-home', shortLabel: 'Início' },
    { label: 'Sobre', action: this.openModal, icon: 'po-icon-user', shortLabel: 'Sobre' }
  ];


  public openModal(): boolean {
    this.modalSobre.open();
    return true;
  }
}
