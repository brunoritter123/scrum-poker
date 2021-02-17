import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PoModalComponent, PoNotificationService, PoPageSlideComponent } from '@po-ui/ng-components';
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
  public textoLoading = 'Carregando';
  public reconectando = false;
  public sala: Sala;

  private inscricaoParticipanteRemovido: Subscription;
  private inscricaoReceberSala: Subscription;
  private inscricaoonPercaDeConexao: Subscription;
  private inscricaoonReconectando: Subscription;
  private inscricaoonReconectado: Subscription;

  constructor(
    private activatedRoute: ActivatedRoute,
    private authService: AuthService,
    private salaHubService: SalaHubService,
    private poNotification: PoNotificationService,
    private router: Router
  ) {
    const keySala = 'sala';
    this.sala = this.activatedRoute.snapshot.data[keySala];
    salaHubService.salaConfig = this.sala.configuracao;

    this.inscricaoReceberSala = this.salaHubService.receberSala.subscribe((x: any) => this.onReceberSala(x));
    this.inscricaoParticipanteRemovido = this.salaHubService.receberParticipanteRemovido.subscribe((x: string) => {
      this.onParticipanteRemovido(x);
    });
    this.inscricaoonPercaDeConexao = this.salaHubService.onPercaDeConexao.subscribe((x: any) => this.onPercaDeConexao());
    this.inscricaoonReconectando = this.salaHubService.onReconectando.subscribe((x: any) => this.onReconectando());
    this.inscricaoonReconectado = this.salaHubService.onReconectado.subscribe((x: any) => this.onReconectado());
   }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this.inscricaoParticipanteRemovido.unsubscribe();
    this.inscricaoReceberSala.unsubscribe();
    this.inscricaoonReconectando.unsubscribe();
    this.inscricaoonReconectado.unsubscribe();
    this.inscricaoonPercaDeConexao.unsubscribe();
    this.salaHubService.stopConection();
  }

  private onParticipanteRemovido(nomeParticipanteQueRemoveu: string): void {
    if (!!nomeParticipanteQueRemoveu) {
      this.poNotification.warning('Você foi removido da sala pelo participante: ' + nomeParticipanteQueRemoveu);
    } else {
      this.poNotification.warning('Essa sessão foi encerrada');
    }
    this.router.navigate(['']);
  }

  private onPercaDeConexao(): void {
    this.poNotification.error('Perca de conexão.');
    this.router.navigate(['']);
  }

  private onReconectando(): void {
    this.reconectando = true;
    this.textoLoading = 'Recnectando...';
  }

  private onReconectado(): void {
    this.reconectando = false;
  }

  private onReceberSala(sala: Sala): void {
    this.sala = sala;
    this.activatedRoute.snapshot.data['sala'] = this.sala;
  }
}
