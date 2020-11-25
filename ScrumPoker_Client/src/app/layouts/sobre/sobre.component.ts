import { Component } from '@angular/core';

@Component({
  selector: 'app-sobre',
  templateUrl: './sobre.component.html'
})
export class SobreComponent {

  constructor() { }

  public contact = {
    name: 'Bruno de Lima Ritter',
    email: 'brunolritter123@gmail.com'
  };

  sendContact(email) {
    window.open(`mailto:${email}`, '_self');
  }

  apoiar() {
    window.open('https://github.com/brunoritter123/scrum-poker', '_blank');
  }

}
