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
  @ViewChild('modalSobre', { static: true }) modalSobre?: PoModalComponent;

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
  };

  public profileActions: Array<PoToolbarAction> = [];
  public profile?: PoToolbarProfile = undefined;
  public menus: Array<PoMenuItem> = [
    { label: 'Início', link: '/', icon: 'po-icon-home', shortLabel: 'Início' },
    { label: 'Contato', action: this.openModal.bind(this), icon: 'po-icon-chat', shortLabel: 'Contato' }
  ];

  public primaryActionSobre: PoModalAction = {
    action: () => {
      if (!!this.modalSobre){
        this.modalSobre.close();
      }
    },
    label: 'Fechar'
  };

  constructor(
    private authService: AuthService,
    private loadPageService: LoadPageService
    ) {
      this.inscricaoLogin = this.authService.eventLogin.subscribe(() => this.eventLogin());
      this.inscricaoLogout = this.authService.eventLogout.subscribe(() => this.eventLogout());
      this.inscricaoPerfil = this.authService.eventPerfil.subscribe( (profile: PoToolbarProfile) => this.profile = profile);
      this.inscricaoLoadSala = this.loadPageService.loadPage.subscribe((load: boolean) => this.carregando = load);
  }

  ngOnInit(): void{
    if (this.authService.logado()) {
      this.eventLogin();
      this.profile = this.authService.profileLogado;
    } else {
      this.eventLogout();
    }
  }

  ngOnDestroy(): void {
    this.inscricaoLogin.unsubscribe();
    this.inscricaoLogout.unsubscribe();
    this.inscricaoPerfil.unsubscribe();
  }

  private eventLogin(): void {
    this.profileActions = [this.profActAlterar, this.profActSair];
  }

  private eventLogout(): void {
    this.profile = undefined;
    this.profileActions = [this.profActLogin];
  }

  public openModal(): boolean {
    if (!!this.modalSobre) {
      this.modalSobre.open();
      return true;
    }
    return false;
  }
}
