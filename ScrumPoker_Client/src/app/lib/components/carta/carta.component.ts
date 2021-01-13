import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-carta',
  templateUrl: './carta.component.html',
  styleUrls: ['./carta.component.css']
})
export class CartaComponent implements OnInit {
  @Output() pClick: EventEmitter<void> = new EventEmitter();
  @Input() pLabel = '';
  @Input('pSelecionada') set selecionada(value: boolean) {
    this.tipo = value ? 'danger' : 'default';
  }

  tipo = 'default';

  constructor() { }

  ngOnInit(): void {
  }

  cartaClick(): void {
    this.pClick.emit();
  }

}
