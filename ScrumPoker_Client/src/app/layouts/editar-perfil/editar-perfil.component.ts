import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs/internal/Subscription';
import { PerfilService } from 'src/app/services/perfil.service';
import { Router, ActivatedRoute } from '@angular/router';
import { Perfil } from 'src/app/interfaces/perfil.interface';
import { PoNotificationService } from '@po-ui/ng-components';

@Component({
  selector: 'app-editar-perfil',
  templateUrl: './editar-perfil.component.html'
})
export class EditarPerfilComponent implements OnInit {
  public carregando: boolean = false;
  public perfil: Perfil = {id: '', nome: '', email: ''};

  constructor(
    private perfilService: PerfilService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private poNotification: PoNotificationService) { }

  ngOnInit(): void {
    this.activatedRoute.data.subscribe( (resolve) => {
        this.perfil = resolve.perfil;
      },
      (erro) => {
        console.log(erro);
        this.poNotification.error("Houve um erro ao buscar o perfil");
      })
  }

  public upload() {

  }

  public cancel() {
    history.go(-1);
  }

  public save() {
    this.carregando = true;

    this.perfilService.alterarPerfil(this.perfil)
    .then( () => history.go(-1))
    .finally(() => this.carregando = false);

  }

}
