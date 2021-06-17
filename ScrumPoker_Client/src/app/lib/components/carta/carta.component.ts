import { Component, Input, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-carta',
  templateUrl: './carta.component.html',
  styleUrls: ['./carta.component.css']
})
export class CartaComponent {
  @Output() pClick: EventEmitter<void> = new EventEmitter();
  @Input() pLabel = '';
  @Input() pSelecionada: boolean = false;
  // @Input('pSelecionada') set selecionada(value: boolean) {
  //   this.tipo = value ? 'danger' : 'default';
  // }

  // tipo = 'default';
  public get tipo(): string {
    return this.pSelecionada ? 'danger' : 'default';
  }

  constructor() { }

  cartaClick(): void {
    this.pClick.emit();
  }

}
