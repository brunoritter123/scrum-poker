import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PoDialogService } from '@po-ui/ng-components';
import { Subscription } from 'rxjs';
import { SalaConfiguracao } from 'src/app/models/sala-configuracao.model';
import { SalaParticipante } from 'src/app/models/sala-participante.model';
import { Sala } from 'src/app/models/sala.model';
import { Voto } from 'src/app/models/voto.model';
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
  public possoRemoverJogador = false;

  private salaConfig: SalaConfiguracao;
  private administradores: Array<SalaParticipante>;

  private inscricaoNovoJogador: Subscription;
  private inscricaoNovoAdministrador: Subscription;
  private inscricaoSalaConfiguracaor: Subscription;
  private inscricaoReceberSala: Subscription;
  private inscricaoReceberVoto: Subscription;

  constructor(
    private activatedRoute: ActivatedRoute,
    private authService: AuthService,
    private salaHubService: SalaHubService,
    private poDialogService: PoDialogService
  ) {
    this.meuIdParticipante = authService.idParticipante;
    this.souJogador = authService.isJogador;
    const keySala = 'sala';
    this.sala = this.activatedRoute.snapshot.data[keySala];
    this.jogadores = this.sala.jogadores;
    this.salaConfig = this.sala.configuracao;
    this.administradores = this.sala.administradores;
    this.verificarSePossoRemoverJogador();

    this.inscricaoReceberSala = this.salaHubService.receberSala.subscribe((x: any) => this.onReceberSala(x));
    this.inscricaoNovoJogador = this.salaHubService.receberJogadores.subscribe((x: any) => this.onReceberJogadores(x));
    this.inscricaoNovoAdministrador = this.salaHubService.receberAdministradores.subscribe((x: any) => this.onReceberAdministradores(x));
    this.inscricaoSalaConfiguracaor = this.salaHubService.receberConfiguracaoSala.subscribe((x: any) => this.onNovaConfiguracaoSala(x));
    this.inscricaoReceberVoto = this.salaHubService.receberVoto.subscribe((x: Voto) => this.onReceberVoto(x));
  }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this.inscricaoNovoJogador.unsubscribe();
    this.inscricaoNovoAdministrador.unsubscribe();
    this.inscricaoSalaConfiguracaor.unsubscribe();
    this.inscricaoReceberSala.unsubscribe();
    this.inscricaoReceberVoto.unsubscribe();
  }

  private onReceberSala(sala: Sala): void {
    this.sala = sala;
  }

  private onReceberJogadores(jogadores: Array<SalaParticipante>): void {
    this.jogadores = jogadores;
  }

  private onReceberVoto(voto: Voto): void {
    const jogadorEncontrado = this.jogadores.find(jogador => jogador.id === voto.jogadorId);
    if (!!jogadorEncontrado){
      jogadorEncontrado.votoCartaValor = voto.valorVoto;
    }
  }

  private onReceberAdministradores(administradores: Array<SalaParticipante>): void {
    this.administradores = administradores;
    this.verificarSePossoRemoverJogador();
  }

  private onNovaConfiguracaoSala(salaConfig: SalaConfiguracao): void {
    this.salaConfig = salaConfig;
    this.verificarSePossoRemoverJogador();
  }

  private verificarSePossoRemoverJogador(): void {
    this.possoRemoverJogador = !this.souJogador ||
                               this.salaConfig.jogadorRemoveJogador ||
                               this.administradores.length === 0;
  }


  public remover(jogador: SalaParticipante): void {
    if (jogador.id !== this.meuIdParticipante && this.possoRemoverJogador) {
      this.poDialogService.confirm({
        title: 'Atenção',
        message: 'Deseja realmente remover o jogador: ' + jogador.nome + '?',
        confirm: () => this.salaHubService.removerParticipante(jogador.id)
      });
    }
  }
}
