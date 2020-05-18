import { Component, OnInit, ViewChild } from '@angular/core';
import { PoModalComponent, PoModalAction } from '@po-ui/ng-components';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent implements OnInit {
  @ViewChild('modalEsqueceuSenha', { static: true }) modalEsqueceuSenha: PoModalComponent;

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

  public userLogin: string = '';
  public userPass: string = '';

  constructor(
    private router: Router
  ) { }

  ngOnInit(): void {
  }

  public keyUpPass(): void{

  }

  public logar(): void{

  }

  public cadastrar(): void{
    this.router.navigate(
      [`/cadastrar-conta`]
    );
  }

  public esqueceuSenha(): void{
    this.modalEsqueceuSenha.open()
  }

  public enviarEmailEsqueceuSenha(): void{

  }
}
