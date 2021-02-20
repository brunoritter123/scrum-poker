import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Pontuacao } from 'src/app/models/pontuacao.model';
import { SalaParticipante } from 'src/app/models/sala-participante.model';
import { Sala } from 'src/app/models/sala.model';
import { SalaHubService } from 'src/app/services/sala-hub.service';

@Component({
  selector: 'app-resultados',
  templateUrl: './resultados.component.html'
})
export class ResultadosComponent{
  public maisVotado = '';
  public concenso = false;
  public gifAleatorio = '0';
  public pontuacao: Array<Pontuacao> = [];

  public sala: Sala;

  constructor(
    private activatedRoute: ActivatedRoute,
    public salaHubService: SalaHubService
  ) {
    const keySala = 'sala';
    this.sala = this.activatedRoute.snapshot.data[keySala];
    this.ordenarVoto(this.sala.jogadores);
  }

  private ordenarVoto(jogadores: Array<SalaParticipante>): void {
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
        let ordem = this.sala.configuracao.cartas.find(carta => carta.value === jogador.votoCartaValor)?.ordem;
        ordem = ordem === null ? -1 : ordem;
        this.pontuacao.push(new Pontuacao(jogador.votoCartaValor, 1, ordem));
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
      this.concenso = this.pontuacao.length === 1 && !!this.maisVotado;
    });
  }

  public revotarJogo(): void {
    this.salaHubService.enviarRevotarSala(this.sala.id);
  }

  public concluirJogo(): void {
    this.salaHubService.enviarConcluirSala(this.sala.id);
  }

}
