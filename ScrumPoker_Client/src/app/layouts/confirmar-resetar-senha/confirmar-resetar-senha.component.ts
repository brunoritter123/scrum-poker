import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from 'src/app/services/auth.service';
import { Router, ActivatedRoute } from '@angular/router';
import { PoNotificationService } from '@po-ui/ng-components';
import { AuthResetarSenha } from 'src/app/interfaces/authResetarSenha.interface';

@Component({
  selector: 'app-confirmar-resetar-senha',
  templateUrl: './confirmar-resetar-senha.component.html'
})
export class ConfirmarResetarSenhaComponent implements OnInit {
  public registrarForm: FormGroup;
  public carregando = false;
  private token = '';
  private userName = '';

  constructor(
    private activatedRoute: ActivatedRoute,
    public fb: FormBuilder,
    public authService: AuthService,
    private router: Router,
    private poNotification: PoNotificationService
  ) {
    this.registrarForm = this.fb.group({
      // tslint:disable-next-line: deprecation
      senhas: this.fb.group({
        senha: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(10)]],
        confirmaSenha: ['', Validators.required ]
      }, {validator: this.compararSenhas})
    });
  }

  ngOnInit(): void {
    const keyToken = 'token';
    const keyUserName = 'userName';
    this.token = this.activatedRoute.snapshot.queryParams[keyToken];
    this.userName = this.activatedRoute.snapshot.queryParams[keyUserName];

    if (!this.token || !this.userName){
      this.router.navigate(['']);
    }
  }

  private compararSenhas(fb: FormGroup): void {
    const senha = fb.get('senha');
    const confirmaSenha = fb.get('confirmaSenha');

    if (confirmaSenha?.errors == null || 'mismatch' in confirmaSenha.errors) {
      if (senha?.value !== confirmaSenha?.value) {
        confirmaSenha?.setErrors({mismatch: 'Confirmação da senha não confere com a senha.'});
      } else {
        confirmaSenha?.setErrors(null);
      }
    }
  }

  public resetarSenha(): void{
    this.carregando = true;

    const authRegistrar: AuthResetarSenha = {
      userName: this.userName,
      password: this.senha?.value,
      token: this.token
    };

    this.authService.resetarSenha(this.userName, authRegistrar).toPromise()
      .then(() => {
        this.poNotification.success('Senha resetada com sucesso');
        this.router.navigate(['']);
      })
      .catch((erro) => {
        console.log(erro);
        this.poNotification.error('Houve um erro ao tentar resetar a senha');
      })
      .finally(() => this.carregando = false);
  }

  get senha(): AbstractControl | null {
    return this.registrarForm.get('senhas.senha');
  }

  get confirmaSenha(): AbstractControl | null {
    return this.registrarForm.get('senhas.confirmaSenha');
  }

  public getErroForm(item: AbstractControl | null): string {
    let erros = '';

    if (!!item && item.errors != null && 'mismatch' in item.errors) {
      erros += item.errors.mismatch;
    }

    return erros;
  }
}
