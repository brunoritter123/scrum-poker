import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';

import { PoMenuItem, PoModalComponent, PoModalAction, PoToolbarAction, PoToolbarProfile } from '@po-ui/ng-components';
import { SignalRService } from './services/signal-r.service';

import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {
  @ViewChild('modalLogin', { static: true }) modalLogin: PoModalComponent;
  @ViewChild('modalSobre', { static: true }) modalSobre: PoModalComponent;

  private newAuth;
  private conectarJira;

  public title = 'Scrum Poker';
  private profActLogin: PoToolbarAction = { icon: 'po-icon-user', label: 'Login', action: () => this.modalLogin.open() };
  private profActSair: PoToolbarAction = {
    icon: 'po-icon-exit',
    label: 'Sair',
    type: 'danger',
    separator: true,
    action: () => this.authService.sairConta()
  };
  public profileActions: Array<PoToolbarAction> = [this.profActLogin];
  public profile: PoToolbarProfile = {
    avatar: '',
    subtitle: '',
    title: ''
  };

  public primaryActionLogin: PoModalAction = {
    action: () => {
      this.authService.conectarConta()
    },
    label: 'Conectar'
  };

  public primaryActionSobre: PoModalAction = {
    action: () => {
      this.modalSobre.close()
    },
    label: 'Fechar'
  };

  public secondaryActionLogin: PoModalAction = {
    action: () => {
      this.modalLogin.close()
    },
    label: 'Cancelar'
  }


  constructor(
    private authService: AuthService,
    public signalRService: SignalRService
    ) {
  }

  ngOnInit(){
    this.newAuth = this.authService.emitirAuth.subscribe((newProfile) => {
      this.profile = newProfile;
      if (this.profile.title > '') {
        this.profileActions = [this.profActSair];
        this.modalLogin.close();
      } else {
        this.profileActions = [this.profActLogin];
      }
    });

    this.conectarJira = this.authService.emitirConectarJira.subscribe(() => {
      this.openLogin()
    });
  }

  ngOnDestroy() {
    this.newAuth.unsubscribe();
    this.conectarJira.unsubscribe();
  }

  public menus: Array<PoMenuItem> = [
    { label: 'Início', link: '/', icon: 'po-icon-home', shortLabel: 'Início' },
    { label: 'Sobre', action: this.openModal, icon: 'po-icon-user', shortLabel: 'Sobre' }
  ];


  public openModal(): boolean {
    this.modalSobre.open();
    return true;
  }

  public openLogin(): boolean {
    this.modalLogin.open();
    return true;
  }
}
