import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { Carta } from 'src/app/models/carta.model';
import { SalaConfiguracao } from 'src/app/models/sala-configuracao.model';
import { Sala } from 'src/app/models/sala.model';
import { AuthService } from 'src/app/services/auth.service';
import { SalaHubService } from 'src/app/services/sala-hub.service';

@Component({
  selector: 'app-votacao',
  templateUrl: './votacao.component.html',
  styleUrls: ['./votacao.component.css']
})
export class VotacaoComponent implements OnInit, OnDestroy {
  public sala: Sala;

  public _meuVotoValue: string;
  public set meuVotoValue(v : string) {
    this._meuVotoValue = v;
    this.selecionaCartaVotada();
  }
  public get meuVotoValue() {
    return this._meuVotoValue;
  }


  private estouConectado: boolean = true;
  private inscricaoSalaconfiguracao: Subscription;
  private meuIdParticipante: string;

  constructor(
    private activatedRoute: ActivatedRoute,
    private authService: AuthService,
    private salaHubService: SalaHubService
    ) {
      this.meuIdParticipante = authService.idParticipante;
     }

  ngOnInit(): void {
    this.sala = this.activatedRoute.snapshot.data['sala'];
    this.meuVotoValue = this.sala.jogadores.find(jogador => jogador.id == this.meuIdParticipante)?.votoCartaValor;

    this.inscricaoSalaconfiguracao = this.salaHubService.receberConfiguracaoSala.subscribe(x => this.onReceberConfiguracaoSala(x));
  }

  ngOnDestroy(): void {
    this.inscricaoSalaconfiguracao.unsubscribe();
  }

  onReceberConfiguracaoSala(salaConfig: SalaConfiguracao): void {
    this.sala.configuracao = salaConfig;
  }

  public votar(carta: Carta): void {
    if (!this.sala.jogoFinalizado && !!carta && this.estouConectado) {
      this.meuVotoValue = carta.typeButton == 'danger' ? null : carta.value;
      this.salaHubService.enviarVoto(this.meuVotoValue);
    }
  }

  private selecionaCartaVotada(){
    this.sala.configuracao.cartas.forEach( carta => {
      if (carta.value == this.meuVotoValue){
        carta.typeButton = 'danger';
      } else {
        carta.typeButton = 'default';
      }
    })
  }

}
