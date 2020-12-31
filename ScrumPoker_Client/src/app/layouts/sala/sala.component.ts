import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PoNotificationService, PoPageSlideComponent } from '@po-ui/ng-components';
import { Subscription } from 'rxjs';
import { SalaConfiguracao } from 'src/app/models/sala-configuracao.model';
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
    this.sala = this.activatedRoute.snapshot.data['sala'];
    salaHubService.salaConfig = this.sala.configuracao;
   }

  private inscricaoParticipanteRemovido: Subscription;
  private inscricaoReceberSala: Subscription;
  ngOnInit(): void {
    this.inscricaoReceberSala = this.salaHubService.receberSala.subscribe(x => this.onReceberSala(x));
    this.inscricaoParticipanteRemovido = this.salaHubService.receberParticipanteRemovido.subscribe((x: string) => {
      this.onParticipanteRemovido(x);
    })
  }

  ngOnDestroy(): void {
    this.inscricaoParticipanteRemovido.unsubscribe();
    this.inscricaoReceberSala.unsubscribe();
    this.salaHubService.stopConection();
  }

  private onParticipanteRemovido(nomeParticipanteQueRemoveu: string): void {
    this.poNotification.warning("Você foi removido da sala pelo participante: " + nomeParticipanteQueRemoveu)
    this.router.navigate(['']);
  }

  private onReceberSala(sala: Sala): void {
    this.sala = sala;
  }
}
