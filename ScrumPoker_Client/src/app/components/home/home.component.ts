import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit, OnDestroy {

  public logado: boolean;
  private inscricaoLogin;
  private inscricaoLogout;

  constructor(
    private authService: AuthService
    ) { }

  ngOnInit(): void {
    this.logado = this.authService.logado();
    this.inscricaoLogin = this.authService.eventLogin.subscribe(() => this.logado = true);
    this.inscricaoLogout = this.authService.eventLogout.subscribe(() => this.logado = false);
  }

  ngOnDestroy(): void {
    this.inscricaoLogin.unsubscribe();
    this.inscricaoLogout.unsubscribe();
  }
}
