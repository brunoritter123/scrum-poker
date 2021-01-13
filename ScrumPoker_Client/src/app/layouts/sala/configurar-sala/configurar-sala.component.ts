import { Component, OnInit, OnDestroy, Output, EventEmitter } from '@angular/core';
import { Sala } from '../../../models/sala.model';
import { PoDialogService, PoDropdownAction } from '@po-ui/ng-components';
import { Router, ActivatedRoute } from '@angular/router';
import { SalaService } from '../../../services/sala.service';
import { Subscription } from 'rxjs';
import { SalaHubService } from 'src/app/services/sala-hub.service';
import { SalaConfiguracao } from 'src/app/models/sala-configuracao.model';

@Component({
  selector: 'app-configurar-sala',
  templateUrl: './configurar-sala.component.html'
})
export class ConfigurarSalaComponent implements OnInit, OnDestroy {
  @Output() closeConfig: EventEmitter<any> = new EventEmitter();

  public novaCarta = '';
  public ordemCarta = '';
  public isAlterouCartas = false;
  public cartas: Array<any> = [];
  public carregando = false;

  public cartasEspecias: Array<string> = ['?', '...', 'cafe'];

  public opcoesCartasEspecias = [
    { value: '?', label: 'Interrogação' },
    { value: '...', label: 'Infinito' },
    { value: 'cafe', label: 'Xícara de café' }
  ];

  public readonly sequencias: Array<PoDropdownAction> = [
    { label: 'Padrão 1'   , action: () => this.alteraCartas('Padrão 1') },
    { label: 'Padrão 2'   , action: () => this.alteraCartas('Padrão 2') },
    { label: 'Fibonacci'  , action: () => this.alteraCartas('Fibonacci') },
    { label: 'Tamanho'    , action: () => this.alteraCartas('Tamanho') },
    { label: 'Salvo'      , action: () => this.alteraCartas('Salvo') },
    { label: 'Limpar'     , action: () => this.alteraCartas('Limpar') }
  ];

  public configSala: SalaConfiguracao = this.activatedRoute.snapshot.data.sala.configuracao;

  private inscricaoConfiguracaoSala: Subscription;

  constructor(
    private salaService: SalaService,
    private thfAlert: PoDialogService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private salaHubService: SalaHubService) {

    this.inscricaoConfiguracaoSala = this.salaHubService.receberConfiguracaoSala.subscribe(
      (salaConfig: SalaConfiguracao) => this.receberSala(salaConfig));
    }

  ngOnInit(): void {
    this.receberSala(this.activatedRoute.snapshot.data.sala.configuracao);
  }

  ngOnDestroy(): void {
    this.inscricaoConfiguracaoSala.unsubscribe();
  }

  private receberSala(sala: SalaConfiguracao): void {
    // Gera uma nova referência do objeto
    this.configSala = { ...sala };
    this.execteAlteraCartas('Salvo');
  }

  public alteraCartas(aplicar: string): void{
    if (this.isAlterouCartas){
      this.thfAlert.confirm({
        title: 'Atenção',
        message: `Deseja limpar as alteração realizadas nas cartas e aplicar a sequência selecionada?`,
        confirm: () => this.execteAlteraCartas(aplicar)
      });
    } else {
      this.execteAlteraCartas(aplicar);
    }
  }

  private execteAlteraCartas(aplicar: string): void {
    switch (aplicar) {
      case 'Fibonacci':
        this.cartas = [ {value: ' 0'}, {value: ' ½'}, {value: '01'},
                        {value: '02'}, {value: '03'}, {value: '05'},
                        {value: '08'}, {value: '13'}, {value: '21'},
                        {value: '34'}, {value: '55'}
        ];
        break;

      case 'Padrão 1':
        this.cartas = [ {value: '01'}, {value: '02'}, {value: '03'},
                        {value: '05'}, {value: '08'}, {value: '13'},
                        {value: '21'}, {value: '34'}, {value: '55'}
        ];
        break;

      case 'Padrão 2':
        this.cartas = [ {value: ' 0'}, {value: ' ½'}, {value: '01'},
                        {value: '02'}, {value: '03'}, {value: '05'},
                        {value: '08'}, {value: '13'}, {value: '20'},
                        {value: '40'}, {value: '100'}
        ];
        break;

      case 'Tamanho':
        this.cartas = [ {value: 'XXS'}, {value: 'XS'}, {value: 'S'},
                        {value: 'M'}, {value: 'L'},  {value: 'XL'},
                        {value: 'XXL'}
        ];
        break;

      case 'Limpar':
        this.cartas = [];
        break;

      case 'Salvo':
        this.cartasEspecias = [];
        this.cartas = [];
        this.configSala.cartas.forEach( (carta) => {
          if (carta.especial){
            this.cartasEspecias.push(carta.value);
          } else {
            this.cartas.push({value: carta.value});
          }
        });
        break;


      default:
        break;
    }
    this.isAlterouCartas = false;
  }

  public save(): void {
    this.carregando = true;

    const cartasNew: Array<any> = [];
    this.cartas.forEach( (carta, id) => {
      cartasNew.push({
        idSala: this.configSala.id,
        value: carta.value,
        ordem: id,
        especial: false
      });
    });

    this.cartasEspecias.forEach( (carta, id) => {
      cartasNew.push({
        idSala: this.configSala.id,
        value: carta,
        ordem: id,
        especial: true
      });
    });

    let lAltCartas: boolean = cartasNew.length !== this.configSala.cartas.length;

    if (!lAltCartas)
    {
      this.configSala.cartas.forEach( (carta, index) => {
        if (carta.value !== cartasNew[index].value) {
          lAltCartas = true;
        }
      });
    }

    if (lAltCartas){

      this.salaService.excluirCarta(this.configSala.salaId)
      .then( () => {
        this.configSala.cartas = cartasNew;
        this.salvarSala();
      });
    } else {

      this.salvarSala();
    }
  }

  private salvarSala(): void {
    this.salaHubService.enviarConfiguracaoSala(this.configSala)
    .finally(() => {
      this.carregando = false;
      this.closeConfig.emit(true);
    });
  }

  public cancel(): void {
    this.closeConfig.emit(true);
    this.carregando = true;
    this.salaService.buscarSala(this.configSala.salaId)
    .then((sala: Sala) => this.receberSala(sala.configuracao))
    .finally(() => {
      this.carregando = false;
    });
  }

  public addCarta(): void {
    const cartaTmp: Array<object> = [];
    const ordemCarta = parseInt(this.ordemCarta, 4);
    const novaCarta = {value: this.novaCarta};

    if (this.novaCarta.length > 0) {
      if (ordemCarta <= 0) {
        this.cartas.unshift(novaCarta);

      } else if (ordemCarta > this.cartas.length || isNaN(ordemCarta)) {
        this.cartas.push(novaCarta);

      } else {
        this.cartas.forEach( carta => {
          if ( (cartaTmp.length + 1) === ordemCarta) {
            cartaTmp.push(novaCarta);
          }
          cartaTmp.push(carta);
        });
        this.cartas = cartaTmp.slice();
      }

      this.novaCarta = '';
      this.isAlterouCartas = true;
    }
  }

  public proximaOrdem(): void {
    this.ordemCarta = (this.cartas.length + 1).toString();
  }
}
