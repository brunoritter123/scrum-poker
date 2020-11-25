import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { PoNotificationService } from '@po-ui/ng-components';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'confirmar-email',
  templateUrl: 'confirmar-email.component.html'
})

export class ConfirmarEmailComponent implements OnInit {
  public carregando: boolean = false;
  public userName: string = '';

  constructor(
    private activatedRoute: ActivatedRoute ,
    private poNotification: PoNotificationService,
    private router: Router,
    private authService: AuthService
  ) { }

  ngOnInit() {
    let token: string = this.activatedRoute.snapshot.queryParams['token'];
    let userName: string = this.activatedRoute.snapshot.queryParams['userName'];

    if (!!token && !!userName) {
      this.confirmarEmail(token, userName);
    }
  }

  confirmarEmail(token: string, userName: string) {
    this.carregando = true;
    this.authService.confirmarEmail(token, userName).toPromise()
      .then(() => {
        this.poNotification.success('E-mail confirmado com sucesso!')
        this.router.navigate(['']);
      })
      .catch((erro) => {
        console.log(erro);
        this.poNotification.error("Houve um erro ao tentar confirmar seu e-mail.");
      })
      .finally(() => this.carregando = false)
  }

  public reenviarEmail() {
    this.carregando = true;
    this.authService.enviarConfirmacaoEmail(this.userName).toPromise()
      .then(() => {
        this.poNotification.success("E-mail enviado com sucesso!")
        this.router.navigate(['']);
      })
      .catch((erro) => {
        if (erro.status == 404) {
          this.poNotification.warning("Usuário não foi encontrado.");
        } else {
          console.log(erro);
          this.poNotification.error("Houve um erro ao tentar enviar o e-mail");
        }
      })
      .finally(() => this.carregando = false)
  }
}