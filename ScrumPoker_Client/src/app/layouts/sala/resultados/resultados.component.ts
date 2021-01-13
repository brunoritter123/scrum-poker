import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { Pontuacao } from 'src/app/models/pontuacao.model';
import { SalaConfiguracao } from 'src/app/models/sala-configuracao.model';
import { SalaParticipante } from 'src/app/models/sala-participante.model';
import { Sala } from 'src/app/models/sala.model';
import { AuthService } from 'src/app/services/auth.service';
import { SalaHubService } from 'src/app/services/sala-hub.service';

@Component({
  selector: 'app-resultados',
  templateUrl: './resultados.component.html'
})
export class ResultadosComponent implements OnInit, OnDestroy {
  public maisVotado = '';
  public pontuacao: Array<Pontuacao> = [];
  public possoRevotarJogo: boolean;
  public possoConcluirJogo: boolean;

  public sala: Sala;
  public salaConfig: SalaConfiguracao;
  public administradores: Array<SalaParticipante>;

  private meuIdParticipante: string;
  private inscricaoSalaconfiguracao: Subscription;
  private inscricaoReceberAdministradores: Subscription;
  private inscricaoReceberJogaodoes: Subscription;
  private inscricaoJogadorFinalizaJogo: Subscription;
  private inscricaoJogadorResetaJogo: Subscription;

  constructor(
    private activatedRoute: ActivatedRoute,
    private authService: AuthService,
    private salaHubService: SalaHubService
  ) {
    const keySala = 'sala';
    this.sala = this.activatedRoute.snapshot.data[keySala];
    this.salaConfig = this.sala.configuracao;
    this.administradores = this.sala.administradores;
    this.meuIdParticipante = authService.idParticipante;
    this.onReceberJogadores(this.sala.jogadores);
    this.possoRevotarJogo = salaHubService.possoResetarJogo;
    this.possoConcluirJogo = salaHubService.possoFinalizarJogo;

    this.inscricaoSalaconfiguracao = this.salaHubService.receberConfiguracaoSala.subscribe(
      (x: any) => this.onReceberConfiguracaoSala(x)
      );
    this.inscricaoReceberAdministradores = this.salaHubService.receberAdministradores.subscribe(
      (x: Array<SalaParticipante>) => this.onReceberAdministradores(x)
      );
    this.inscricaoReceberJogaodoes = this.salaHubService.receberJogadores.subscribe((x: any) => this.onReceberJogadores(x));
    this.inscricaoJogadorFinalizaJogo = this.salaHubService.jogadorFinalizaJogo.subscribe((x: any) => this.possoConcluirJogo = x);
    this.inscricaoJogadorResetaJogo = this.salaHubService.jogadorResetaJogo.subscribe((x: any) => this.possoRevotarJogo = x);
  }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this.inscricaoSalaconfiguracao.unsubscribe();
    this.inscricaoReceberAdministradores.unsubscribe();
    this.inscricaoReceberJogaodoes.unsubscribe();
    this.inscricaoJogadorFinalizaJogo.unsubscribe();
    this.inscricaoJogadorResetaJogo.unsubscribe();
  }

  private onReceberConfiguracaoSala(salaConfig: SalaConfiguracao): void {
    this.salaConfig = salaConfig;
  }

  private onReceberAdministradores(administradores: Array<SalaParticipante>): void {
    this.administradores = administradores;
  }

  private onReceberJogadores(jogadores: Array<SalaParticipante>): void {
    this.pontuacao = [];

    jogadores.forEach(jogador => {
      let existeArray = false;

      this.pontuacao.forEach(ponto => {
        if (ponto.cartaValor === jogador.votoCartaValor) {
          existeArray = true;
          ponto.quantidadePontos += 1;
        }
      });

      if (!existeArray) {
        this.pontuacao.push(new Pontuacao(jogador.votoCartaValor, 1));
      }

      this.pontuacao.sort((a: Pontuacao, b: Pontuacao) => {
        let ret = 0;

        if (a.cartaValor === undefined) {
          ret = 1;
        } else {
          ret = b.quantidadePontos - a.quantidadePontos;
          if (ret === 0) {
            ret = b.ordem - a.ordem;
          }
        }

        return ret;
      });

      this.maisVotado = this.pontuacao[0].cartaValor;
    });
  }

  public revotarJogo(): void {
    this.salaHubService.enviarRevotarSala(this.sala.id);
  }

  public concluirJogo(): void {
    this.salaHubService.enviarConcluirSala(this.sala.id);
  }

}
