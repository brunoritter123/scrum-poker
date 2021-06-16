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

  sendContact(email: string): void {
    window.open(`mailto:${email}`, '_self');
  }

  reportar(): void {
    window.open('https://github.com/brunoritter123/scrum-poker/issues', '_blank');
  }

}
