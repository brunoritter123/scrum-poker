import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { PoNotificationService } from '@po-ui/ng-components';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-confirmar-email',
  templateUrl: 'confirmar-email.component.html'
})

export class ConfirmarEmailComponent implements OnInit {
  public carregando = false;
  public userName = '';

  constructor(
    private activatedRoute: ActivatedRoute ,
    private poNotification: PoNotificationService,
    private router: Router,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    const keyToken = 'token';
    const keyUserName = 'userName';
    const token = this.activatedRoute.snapshot.queryParams[keyToken];
    const userName = this.activatedRoute.snapshot.queryParams[keyUserName];

    if (!!token && !!userName) {
      this.confirmarEmail(token, userName);
    }
  }

  confirmarEmail(token: string, userName: string): void {
    this.carregando = true;
    this.authService.confirmarEmail(token, userName).toPromise()
      .then(() => {
        this.poNotification.success('E-mail confirmado com sucesso!');
        this.router.navigate(['']);
      })
      .catch((erro) => {
        console.log(erro);
        this.poNotification.error('Houve um erro ao tentar confirmar seu e-mail.');
      })
      .finally(() => this.carregando = false);
  }

  public reenviarEmail(): void {
    this.carregando = true;
    this.authService.enviarConfirmacaoEmail(this.userName).toPromise()
      .then(() => {
        this.poNotification.success('E-mail enviado com sucesso!');
        this.router.navigate(['']);
      })
      .catch((erro) => {
        if (erro.status === 404) {
          this.poNotification.warning('Usuário não foi encontrado.');
        } else {
          console.log(erro);
          this.poNotification.error('Houve um erro ao tentar enviar o e-mail');
        }
      })
      .finally(() => this.carregando = false);
  }
}
