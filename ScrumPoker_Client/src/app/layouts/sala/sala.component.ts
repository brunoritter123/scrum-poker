import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PoNotificationService, PoPageSlideComponent } from '@po-ui/ng-components';
import { Subscription } from 'rxjs';
import { Sala } from 'src/app/models/sala.model';
import { AuthService } from 'src/app/services/auth.service';
import { SalaHubService } from 'src/app/services/sala-hub.service';

@Component({
  selector: 'app-sala',
  templateUrl: './sala.component.html'
})
export class SalaComponent implements OnInit, OnDestroy {

  public carregando = false;
  public sala: Sala;

  constructor(
    private activatedRoute: ActivatedRoute,
    private authService: AuthService,
    private salaHubService: SalaHubService,
    private poNotification: PoNotificationService,
    private router: Router
  ) {
   }

  private inscricaoSalaconfiguracao: Subscription;
  private inscricaoParticipanteRemovido: Subscription;
  ngOnInit(): void {
    this.sala = this.activatedRoute.snapshot.data['sala'];

    this.inscricaoSalaconfiguracao = this.salaHubService.receberConfiguracaoSala.subscribe((data: string) =>{
       console.log(data)
    });

    this.inscricaoParticipanteRemovido = this.salaHubService.receberParticipanteRemovido.subscribe((nomeParticipanteQueRemoveu: string) => {
      this.onParticipanteRemovido(nomeParticipanteQueRemoveu);
    })
  }

  ngOnDestroy(): void {
    this.inscricaoSalaconfiguracao.unsubscribe();
    this.inscricaoParticipanteRemovido.unsubscribe();
    this.salaHubService.stopConection();
  }

  private onParticipanteRemovido(nomeParticipanteQueRemoveu: string): void {
    this.poNotification.warning("Você foi removido da sala pelo participante: " + nomeParticipanteQueRemoveu)
    this.router.navigate(['']);
  }
}
