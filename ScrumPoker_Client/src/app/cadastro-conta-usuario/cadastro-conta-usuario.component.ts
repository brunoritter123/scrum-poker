import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-cadastro-conta-usuario',
  templateUrl: './cadastro-conta-usuario.component.html'
})
export class CadastroContaUsuarioComponent implements OnInit {
  public login: string = '';
  public email: string = '';
  public senha: string = '';

  constructor() { }

  ngOnInit(): void {
  }

  public cadastrarConta(): void{

  }
}
