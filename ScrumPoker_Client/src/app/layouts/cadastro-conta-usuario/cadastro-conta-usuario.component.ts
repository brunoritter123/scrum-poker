import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { AuthRegistrar } from '../../interfaces/authRegistrar.interface';
import { PoNotificationService } from '@po-ui/ng-components';

@Component({
  selector: 'app-cadastro-conta-usuario',
  templateUrl: './cadastro-conta-usuario.component.html'
})
export class CadastroContaUsuarioComponent {
  public registrarForm: FormGroup;
  public carregando = false;

  constructor(
    public fb: FormBuilder,
    public authService: AuthService,
    private router: Router,
    private poNotification: PoNotificationService
  ) {
    this.registrarForm = this.fb.group({
      nome: ['', Validators.required],
      login: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      senhas: this.fb.group({
        senha: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(10)]],
        confirmaSenha: ['', Validators.required ]
      }, {validator: this.compararSenhas})
    });
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

  public cadastrarConta(): void{
    this.carregando = true;

    const authRegistrar: AuthRegistrar = {
      nome: this.nome?.value,
      userName: this.login?.value,
      email: this.email?.value,
      password: this.senha?.value
    };

    this.authService.cadastrarConta(authRegistrar).toPromise()
      .then(() => {
        this.poNotification.success('Cadastro realizado');
        this.router.navigate(['confirmar-email']);
      })
      .catch((erro) => {
        console.log(erro);

        if (typeof erro.error === 'object' && Array.isArray(erro.error)) {
          erro.error.forEach((element: any)  => {
            switch (element.code) {
              case 'DuplicateUserName':
                this.poNotification.error('Nome de usuário duplicado!');
                break;
              default:
                this.poNotification.error(`Erro no cadastro! Code: ${element.code}`);
                break;
            }
          });
        } else {
          this.poNotification.error('Erro ao tentar registrar o usuário');
        }
      })
      .finally(() => this.carregando = false);
  }

  get nome(): AbstractControl | null {
    return this.registrarForm.get('nome');
  }

  get login(): AbstractControl | null {
    return this.registrarForm.get('login');
  }

  get email(): AbstractControl | null {
    return this.registrarForm.get('email');
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
