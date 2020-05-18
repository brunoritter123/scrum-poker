import { Component, OnInit, OnDestroy } from '@angular/core';
import { Sala } from '../models/sala.model';
import { PoDialogService } from '@po-ui/ng-components';
import { Router, ActivatedRoute } from '@angular/router';
import { SalaService } from '../services/sala.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-configurar-sala',
  templateUrl: './configurar-sala.component.html'
})
export class ConfigurarSalaComponent implements OnInit, OnDestroy {
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

  public readonly sequencias: Array<object> = [
    { label: 'Padrão 1'   , action: () => this.alteraCartas('Padrão 1') },
    { label: 'Padrão 2'   , action: () => this.alteraCartas('Padrão 2') },
    { label: 'Fibonacci'  , action: () => this.alteraCartas('Fibonacci') },
    { label: 'Tamanho'    , action: () => this.alteraCartas('Tamanho') },
    { label: 'Salvo'      , action: () => this.alteraCartas('Salvo') },
    { label: 'Limpar'     , action: () => this.alteraCartas('Limpar') }
  ];

  public configSala: Sala;

  private inscrisaoResolve: Subscription;

  constructor(
    private salaService: SalaService,
    private thfAlert: PoDialogService,
    private router: Router,
    private activatedRoute: ActivatedRoute) {}

  ngOnInit() {
    this.inscrisaoResolve = this.activatedRoute.data.subscribe( (resolve) => {
      this.configSala = resolve.sala;
      this.alteraCartas('Salvo');
    });
  }

  ngOnDestroy() {
    this.inscrisaoResolve.unsubscribe();
  }

  public alteraCartas(aplicar: string){
    if(this.isAlterouCartas){
      this.thfAlert.confirm({
        title: 'Atenção',
        message: `Deseja limpar as alteração realizadas nas cartas e aplicar a sequência selecionada?`,
        confirm: () => this.execteAlteraCartas(aplicar)
      });
    } else {
      this.execteAlteraCartas(aplicar);
    }
  }

  private execteAlteraCartas(aplicar: string) {
    switch (aplicar) {
      case 'Fibonacci':
        this.cartas = [ {value: ' 0'}, {value: ' ½'}, {value: '01'},
                        {value: '02'}, {value: '03'}, {value: '05'},
                        {value: '08'}, {value: '13'}, {value: '21'},
                        {value: '34'}, {value: '55'}
        ]
        break;

      case 'Padrão 1':
        this.cartas = [ {value: '01'}, {value: '02'}, {value: '03'},
                        {value: '05'}, {value: '08'}, {value: '13'},
                        {value: '21'}, {value: '34'}, {value: '55'}
        ]
        break;

      case 'Padrão 2':
        this.cartas = [ {value: ' 0'}, {value: ' ½'}, {value: '01'},
                        {value: '02'}, {value: '03'}, {value: '05'},
                        {value: '08'}, {value: '13'}, {value: '20'},
                        {value: '40'}, {value: '100'}
        ]
        break;

      case 'Tamanho':
        this.cartas = [ {value: 'XXS'}, {value: 'XS'}, {value: 'S'},
                        {value: 'M'}, {value: 'L'},  {value: 'XL'},
                        {value: 'XXL'}
        ]
        break;

      case 'Limpar':
        this.cartas = [];
        break;

      case 'Salvo':
        this.cartasEspecias = [];
        this.cartas = [];
        this.configSala.cartas.forEach( (carta) => {
          if (carta.especial){
            this.cartasEspecias.push(carta.value)
          } else {
            this.cartas.push({value: carta.value});
          }
        })


      default:
        break;
    }
    this.isAlterouCartas = false;
  }

  public save() {
    this.carregando = true;

    let cartasNew: Array<any> = [];
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

    let lAltCartas: boolean = cartasNew.length != this.configSala.cartas.length;

    if (!lAltCartas)
    {
      this.configSala.cartas.forEach( (carta, index) => {
        if (carta.value != cartasNew[index].value) {
          lAltCartas = true;
        }
      })
    }

    if (lAltCartas){

      this.salaService.excluirCarta(this.configSala.id)
      .then( () => {
        this.configSala.cartas = cartasNew;
        this.salvarSala()
      })
    } else {

      this.salvarSala()
    }
  }

  private salvarSala(): void {
    this.salaService.alterarSala(this.configSala)
    .then( () => this.router.navigate([`/`]))
    .finally(() => this.carregando = false)

  }

  public cancel() {
    this.router.navigate([`/`])
  }

  public addCarta() {
    let cartaTmp : Array<object> = [];
    const ordemCarta = parseInt(this.ordemCarta)
    const novaCarta = {value: this.novaCarta}

    if (this.novaCarta.length > 0) {
      if (ordemCarta <= 0) {
        this.cartas.unshift(novaCarta);

      } else if (ordemCarta > this.cartas.length || isNaN(ordemCarta)) {
        this.cartas.push(novaCarta);

      } else {
        this.cartas.forEach( carta => {
          if( (cartaTmp.length + 1) == ordemCarta) {
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

  public proximaOrdem(){
    this.ordemCarta = (this.cartas.length + 1).toString();
  }
}
