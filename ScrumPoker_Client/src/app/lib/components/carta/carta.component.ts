import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-carta',
  templateUrl: './carta.component.html',
  styleUrls: ['./carta.component.css']
})
export class CartaComponent implements OnInit {
  @Output('p-click') click: EventEmitter<void> = new EventEmitter();
  @Input("p-label") label: string;
  @Input("p-selecionada") set selecionada (value: string) {
    this.tipo = value == 'true' ? 'danger' : 'default';
  }

  tipo: string = 'default';

  constructor() { }

  ngOnInit(): void {
  }

  cartaClick(){
    this.click.emit();
  }

}
