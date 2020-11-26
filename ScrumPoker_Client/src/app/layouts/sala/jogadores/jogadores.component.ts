import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PoDialogService } from '@po-ui/ng-components';
import { Subscription } from 'rxjs';
import { SalaConfiguracao } from 'src/app/models/sala-configuracao.model';
import { SalaParticipante } from 'src/app/models/sala-participante.model';
import { Sala } from 'src/app/models/sala.model';
import { AuthService } from 'src/app/services/auth.service';
import { SalaHubService } from 'src/app/services/sala-hub.service';

@Component({
  selector: 'app-jogadores',
  templateUrl: './jogadores.component.html'
})
export class JogadoresComponent implements OnInit, OnDestroy {
  public sala: Sala;
  public jogadores: Array<SalaParticipante>;
  public meuIdParticipante: string;
  public souJogador: boolean;
  public possoRemoverJogador: boolean;

  private salaConfig: SalaConfiguracao
  private administradores: Array<SalaParticipante>;

  constructor(
    private activatedRoute: ActivatedRoute,
    private authService: AuthService,
    private salaHubService: SalaHubService,
    private poDialogService: PoDialogService
  ) {
    this.meuIdParticipante = authService.idParticipante;
    this.souJogador = authService.isJogador;
    this.sala = this.activatedRoute.snapshot.data['sala'];
    this.jogadores = this.sala.jogadores;
    this.salaConfig = this.sala.configuracao;
    this.administradores = this.sala.administradores;
    this.verificarSePossoRemoverJogador();
   }

  private inscricaoNovoJogador: Subscription;
  private inscricaoNovoAdministrador: Subscription;
  private inscricaoSalaConfiguracaor: Subscription;
  private inscricaoReceberSala: Subscription;
  ngOnInit(): void {
    this.inscricaoReceberSala = this.salaHubService.receberSala.subscribe(x => this.onReceberSala(x));
    this.inscricaoNovoJogador = this.salaHubService.receberJogadores.subscribe((x) => this.onReceberJogadores(x));
    this.inscricaoNovoAdministrador = this.salaHubService.receberAdministradores.subscribe((x) => this.onReceberAdministradores(x));
    this.inscricaoSalaConfiguracaor = this.salaHubService.receberConfiguracaoSala.subscribe((x) => this.onNovaConfiguracaoSala(x));
  }

  ngOnDestroy() {
    this.inscricaoNovoJogador.unsubscribe();
    this.inscricaoNovoAdministrador.unsubscribe();
    this.inscricaoSalaConfiguracaor.unsubscribe();
    this.inscricaoReceberSala.unsubscribe();
  }

  private onReceberSala(sala: Sala): void {
    this.sala = sala;
  }

  private onReceberJogadores(jogadores: Array<SalaParticipante>) {
    this.jogadores = jogadores;
  }

  private onReceberAdministradores(administradores: Array<SalaParticipante>) {
    this.administradores = administradores;
    this.verificarSePossoRemoverJogador();
  }

  private onNovaConfiguracaoSala(salaConfig: SalaConfiguracao) {
    this.salaConfig = salaConfig;
    this.verificarSePossoRemoverJogador();
  }

  private verificarSePossoRemoverJogador(){
    this.possoRemoverJogador = !this.souJogador ||
                               this.salaConfig.jogadorRemoveJogador ||
                               this.administradores.length == 0;
  }


  public remover(jogador: SalaParticipante): void {
    if(jogador.id != this.meuIdParticipante && this.possoRemoverJogador) {
      this.poDialogService.confirm({
        title: 'Atenção',
        message: 'Deseja realmente remover o jogador: ' + jogador.nome + '?',
        confirm: () => this.salaHubService.removerParticipante(jogador.id)
      })
    }
  }
}
