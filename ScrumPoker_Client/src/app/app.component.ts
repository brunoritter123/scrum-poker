import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';

import { PoMenuItem, PoModalComponent, PoModalAction, PoToolbarAction, PoToolbarProfile } from '@po-ui/ng-components';
import { SignalRService } from './services/signal-r.service';

import { AuthService } from './services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {
  @ViewChild('modalSobre', { static: true }) modalSobre: PoModalComponent;

  private inscricaoLogin;
  private inscricaoLogout;

  public title = 'Scrum Poker';
  private profActLogin: PoToolbarAction = {
    icon: 'po-icon-user',
    label: 'Login',
    separator: true,
    action: () => this.router.navigate([''])
  };
  private profActSair: PoToolbarAction = {
    icon: 'po-icon-exit',
    label: 'Sair',
    type: 'danger',
    separator: true,
    action: () => this.authService.logout()
  };
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
    public signalRService: SignalRService
    ) {
  }

  ngOnInit(){
    if (this.authService.logado()) {
      this.eventLogin(this.authService.getProfile())
    } else {
      this.eventLogout();
    }

    this.inscricaoLogin = this.authService.eventLogin.subscribe( profile => this.eventLogin(profile));
    this.inscricaoLogout = this.authService.eventLogout.subscribe(() => this.eventLogout());
  }

  ngOnDestroy() {
    this.inscricaoLogin.unsubscribe();
    this.inscricaoLogout.unsubscribe();
  }

  private eventLogin(newProfile: PoToolbarProfile) {
    this.profile = newProfile;
    this.profileActions = [this.profActSair];
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
