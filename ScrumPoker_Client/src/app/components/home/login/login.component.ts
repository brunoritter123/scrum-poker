import { Component, OnInit, ViewChild } from '@angular/core';
import { PoModalComponent, PoModalAction, PoNotificationService } from '@po-ui/ng-components';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { AuthLogin } from 'src/app/interfaces/authLogin.interface';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent implements OnInit {
  @ViewChild('modalEsqueceuSenha', { static: true }) modalEsqueceuSenha: PoModalComponent;

  public carregando: boolean = false;
  public userNameEsqueceuSenha: string = '';

  public closeModal: PoModalAction = {
    action: () => {
      this.modalEsqueceuSenha.close();
    },
    label: 'Cancelar',
    danger: true
  };

  public confirmModal: PoModalAction = {
    action: () => {
      this.enviarEmailEsqueceuSenha();
    },
    label: 'Enviar'
  };

  public authLogin : AuthLogin = {
    userName: '',
    password: ''
  }

  constructor(
    public authService: AuthService,
    private router: Router,
    private poNotification: PoNotificationService
  ) { }

  ngOnInit(): void {
  }

  public login(): void{
    this.carregando = true;

    this.authService.login(this.authLogin).subscribe(
      () => {
        this.poNotification.success('Logado com sucesso!');
      }, erro => {
        if (erro.status == 401) {

          if (erro.error.code == 'EmailNotConfirmed') {
            this.poNotification.warning(`E-mail não está confirmado`);
            this.authService.enviarConfirmacaoEmail(this.authLogin.userName).subscribe(
              () => this.router.navigate([`/confirmar-email`])
            )
          }

          if (erro.error.code == 'Unauthorized') {
            this.poNotification.warning(`Usuário ou senha estão incorretos!`);
          }

        } else {
          this.poNotification.error(`Falha para logar!`);
          console.log(erro);
        }
      },
      () => this.carregando = false
    )
  }

  public cadastrar(): void{
    this.router.navigate([`/cadastrar-conta`]);
  }

  public esqueceuSenha(): void{
    this.modalEsqueceuSenha.open()
  }

  public enviarEmailEsqueceuSenha(): void{
    if (!this.userNameEsqueceuSenha) {
      this.poNotification.warning("Usuário não foi informado.")
      return
    }

    this.carregando = true;
    this.authService.solicitacaoResetarSenha(this.userNameEsqueceuSenha).toPromise()
      .then(() => {
        this.poNotification.success("E-mail enviado com sucesso!")
        this.modalEsqueceuSenha.close();
      })
      .catch((erro) => {
        console.log(erro);
        this.poNotification.error("Houve algum erro ao tentar resetar a senha.")
      })
      .finally(() => this.carregando = false)
  }
}
