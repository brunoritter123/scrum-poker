import { Injectable, EventEmitter } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { PoToolbarProfile, PoNotificationService } from '@po-ui/ng-components';

@Injectable()
export class AuthService {
  public idSala: string = '';
  public isJogador: boolean = true;
  public id: string = '';
  public name: string = '';
  public httpOptions: any;
  public emitirAuth = new EventEmitter<PoToolbarProfile>();
  public emitirConectarJira = new EventEmitter<void>();

  constructor(
    private cookieService: CookieService,
    private http: HttpClient,
    private poNotification: PoNotificationService
  ) {
    this.getConfig()
    this.poNotification.setDefaultDuration(5000)
  }

  public getConfig(): void {
    this.idSala = this.cookieService.get('idSala')
    this.name = this.cookieService.get('nome')
    this.isJogador = this.cookieService.get('isJogador') == 'true'

    if (!!this.idSala) {
      if (this.cookieService.check(this.idSala.toUpperCase())) {
        this.id = this.cookieService.get(this.idSala.toUpperCase())

      } else {
        this.id = this.idSala.toUpperCase() + this.aleatorio(30)
        this.cookieService.set(this.idSala.toUpperCase(), this.id);
      }
    }
  }

  public getProfile(imageUrl: string, subtitle: string, name: string): PoToolbarProfile {
    const newProfile: PoToolbarProfile = {
      avatar: imageUrl,
      subtitle: subtitle,
      title: name
    };
    return newProfile;
  }

  public saveConfig(idSala: string, nome: string, isJogador: boolean, isIntegraJira: boolean): void {
    const dtExpires = new Date(2100, 1, 1)
    this.idSala = idSala
    this.name = nome
    this.isJogador = isJogador
    this.cookieService.set('idSala', idSala, dtExpires);
    this.cookieService.set('nome', nome, dtExpires);
    this.cookieService.set('isJogador', isJogador.toString(), dtExpires);
    this.cookieService.set('isIntegraJira', isIntegraJira.toString(), dtExpires);

    if (this.cookieService.check(idSala.toUpperCase())) {
      this.id = this.cookieService.get(idSala.toUpperCase())

    } else {
      this.id = idSala.toUpperCase() + this.aleatorio(25)
      this.cookieService.set(idSala.toUpperCase(), this.id, dtExpires);
    }
  }

  private aleatorio(txtTam: number): string {
    const letras = '1234567890abcdefghijlkmnopqtuvxywz';
    let txtAleatorio: string = '';

    for (let i = 0; i < txtTam; i++) {
      const pos = Math.floor(Math.random() * letras.length);
      txtAleatorio += letras.substring(pos, pos + 1);
    }

    return txtAleatorio;
  }

  public setCookie(nameCokie: string, valorCookie: string) {
    this.cookieService.set(nameCokie, valorCookie, new Date(2100, 1, 1))
  }

  public conectarConta(): void{

  }

  public sairConta(): void{

  }
}
