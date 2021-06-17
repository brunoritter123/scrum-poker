import { Component, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PoDialogService } from '@po-ui/ng-components';
import { Subscription } from 'rxjs';
import { SalaConfiguracao } from 'src/app/models/sala-configuracao.model';
import { Sala } from 'src/app/models/sala.model';
import { AuthService } from 'src/app/services/auth.service';
import { SalaHubService } from 'src/app/services/sala-hub.service';

import { SalaParticipante } from '../../../models/sala-participante.model';

@Component({
  selector: 'app-administradores',
  templateUrl: './administradores.component.html'
})
export class AdministradoresComponent implements OnDestroy {

  public administradores: Array<SalaParticipante>;
  public meuIdParticipante: string;
  public souJogador: boolean;
  public possoRemoverAdministrador = false;

  private salaConfig: SalaConfiguracao;

  private inscricaoNovoAdministrador: Subscription;
  private inscricaoSalaConfiguracaor: Subscription;

  constructor(
    private activatedRoute: ActivatedRoute,
    private salaHubService: SalaHubService,
    private poDialogService: PoDialogService,
    authService: AuthService
  ) {
    this.meuIdParticipante = authService.idParticipante;
    this.souJogador = authService.isJogador;
    const salaData = 'sala';
    const sala: Sala = this.activatedRoute.snapshot.data[salaData];
    this.administradores = sala.administradores;
    this.salaConfig = sala.configuracao;
    this.verificarSePossoRemoverAdministrador();

    this.inscricaoNovoAdministrador = this.salaHubService.receberAdministradores.subscribe((x: any) => this.onNovoAdministradores(x));
    this.inscricaoSalaConfiguracaor = this.salaHubService.receberConfiguracaoSala.subscribe((x: any) => this.onNovaConfiguracaoSala(x));
   }

  ngOnDestroy(): void {
    this.inscricaoNovoAdministrador.unsubscribe();
    this.inscricaoSalaConfiguracaor.unsubscribe();
  }

  private onNovoAdministradores(administradores: Array<SalaParticipante>): void {
    this.administradores = administradores;
    this.verificarSePossoRemoverAdministrador();
  }

  private onNovaConfiguracaoSala(salaConfig: SalaConfiguracao): void {
    this.salaConfig = salaConfig;
    this.verificarSePossoRemoverAdministrador();
  }

  private verificarSePossoRemoverAdministrador(): void {
    this.possoRemoverAdministrador = !this.souJogador || this.salaConfig.jogadorRemoveAdministrador || this.administradores.length === 0;
  }


  public remover(administrador: SalaParticipante): void {
    if (administrador.id !== this.meuIdParticipante && this.possoRemoverAdministrador) {
      this.poDialogService.confirm({
        title: 'Atenção',
        message: 'Deseja realmente remover o administrador: ' + administrador.nome + '?',
        confirm: () => this.salaHubService.removerParticipante(administrador.id)
      });
    }
  }

}
