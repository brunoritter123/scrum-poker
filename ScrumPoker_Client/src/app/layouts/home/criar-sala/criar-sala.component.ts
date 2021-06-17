import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { PoNotificationService, PoDialogService, PoSwitchLabelPosition } from '@po-ui/ng-components';


@Component({
  selector: 'app-criar-sala',
  templateUrl: './criar-sala.component.html',
  providers: [PoNotificationService, PoDialogService]
})
export class CriarSalaComponent implements OnInit {

  public idSala = '';
  public jogador = true;
  public integraJira = false;
  public nome = '';
  public switchLabelPosition: PoSwitchLabelPosition = PoSwitchLabelPosition.Left;

  constructor(
    private poNotification: PoNotificationService,
    private router: Router,
    public authService: AuthService,
    private poAlert: PoDialogService
  ) { }

  ngOnInit(): void {
    this.idSala = this.authService.idSala;
    this.nome = this.authService.name;
    this.jogador = this.authService.isJogador;
  }

  public confirmLogin(): void {
    this.poAlert.confirm({
      title: 'Atenção',
      message: 'Não foi possível ler o cookie de identificação.',
      confirm: () => this.authService.saveConfig(this.idSala.toUpperCase(), this.nome, this.jogador, this.integraJira)
    });
  }

  public entrar(): void {
    this.authService.saveConfig(this.idSala.toUpperCase(), this.nome, this.jogador, this.integraJira);

    if (this.authService.idParticipante === undefined) {
      this.confirmLogin();

    } else if (this.nome.trim().length < 3 || this.nome.trim().length > 12) {
      this.poNotification.error('O nome deve ter no mínimo 3 caracteres.');

    } else if (this.idSala.trim().length < 3 || this.idSala.trim().length > 12) {
      this.poNotification.error('O ID da Sala deve ter no mínimo 3 caracteres.');

    } else {
      this.router.navigate(
        [`/sala/${this.idSala.toUpperCase()}`]
      );
    }
  }
}
