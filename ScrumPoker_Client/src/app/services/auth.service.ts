import { Injectable, EventEmitter } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { PoToolbarProfile } from '@po-ui/ng-components';
import { environment } from 'src/environments/environment';
import { JwtHelperService } from '@auth0/angular-jwt';
import { map } from 'rxjs/operators'
import { AuthRegistrar } from '../interfaces/authRegistrar.interface';
import { AuthLogin } from '../interfaces/authLogin.interface';
import { TokenJwt } from '../interfaces/tokenJwt.interface';
import { AuthResetarSenha } from '../interfaces/authResetarSenha.interface';
import { Perfil } from '../interfaces/perfil.interface';

@Injectable()
export class AuthService {
  public idSala: string = '';
  public isJogador: boolean = true;
  public idParticipante: string = '';
  public name: string = '';
  public eventLogin = new EventEmitter();
  public eventLogout = new EventEmitter();
  public eventPerfil = new EventEmitter<PoToolbarProfile>();
  public userNameLogin: string;
  public profileLogado: PoToolbarProfile;

  private readonly jwtHelper = new JwtHelperService();
  private readonly url: string = environment.API;
  private decodedToken: TokenJwt;
  private perfil: Perfil;

  constructor(
    private http: HttpClient
  ) {
    this.getConfig()
    this.loadToken();
  }

  public getConfig(): void {

    this.idSala = localStorage.getItem('idSala')
    this.name = localStorage.getItem('nome')
    this.isJogador = localStorage.getItem('isJogador') == 'true'

    if (!!this.idSala) {
        this.idParticipante = localStorage.getItem(this.idSala.toUpperCase())
        if (!this.idParticipante) {
          this.idParticipante = this.idSala.toUpperCase() + this.aleatorio(30)
          localStorage.setItem(this.idSala.toUpperCase(), this.idParticipante)
        }
    }
  }

  public saveConfig(idSala: string, nome: string, isJogador: boolean, isIntegraJira: boolean): void {
    const dtExpires = new Date(2100, 1, 1)
    this.idSala = idSala
    this.name = nome
    this.isJogador = isJogador

    localStorage.setItem('idSala', idSala)
    localStorage.setItem('nome', nome)
    localStorage.setItem('isJogador', isJogador.toString())
    localStorage.setItem('isIntegraJira', isIntegraJira.toString())

    if (!!this.idSala) {
      this.idParticipante = localStorage.getItem(this.idSala.toUpperCase())
      if (!this.idParticipante) {
        this.idParticipante = this.idSala.toUpperCase() + this.aleatorio(30)
        localStorage.setItem(this.idSala.toUpperCase(), this.idParticipante)
      }
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
    localStorage.setItem(nameCokie, valorCookie)
  }

  public login(model: AuthLogin){
    return this.http
      .post(`${this.url}/user/login`, model).pipe(
        this.pipeLogin()
    )
  }

  public cadastrarConta(model: AuthRegistrar) {
    return this.http.post(`${this.url}/user/registrar`, model, {
      params: {
        "urlConfirmaEmail": `${location.origin}/confirmar-email`
      }})
  }

  public enviarConfirmacaoEmail(userName: string) {
    return this.http.post(`${this.url}/user/enviar-confirmacao-email/${userName}`, null, {
      params: {
        "urlConfirmaEmail": `${location.origin}/confirmar-email`
      }})
  }

  public solicitacaoResetarSenha(userName: string) {
    return this.http.post(`${this.url}/user/solicitacao-resetar-senha/${userName}`, null, {
      params: {
        "urlResetarSenha": `${location.origin}/confirmar-resetar-senha`
      }})
  }

  public resetarSenha(userName: string, authResetarSenha: AuthResetarSenha) {
    return this.http.post(`${this.url}/user/resetar-senha/`, authResetarSenha)
  }

  public confirmarEmail(token: string, userName: string) {
    return this.http.post(`${this.url}/user/confirmar-email`, {
      "token": token,
      "userName": userName
    }).pipe(this.pipeLogin());
  }

  private pipeLogin() {
    return map( (respose: any) => {
      const user = respose;
      if (user) {
        localStorage.setItem('token', user.token);
        this.loadToken();
      }
    })
  }

  private loadToken() {
    if (this.logado()) {
      const token = localStorage.getItem('token');
      this.decodedToken = this.jwtHelper.decodeToken(token);
      this.userNameLogin = this.decodedToken.nameid;
      this.eventLogin.emit();
      this.getProfile();
    }
  }

  public logado(): boolean {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }

  public logout() {
    localStorage.removeItem('token');
    this.userNameLogin = '';
    this.eventLogout.emit();
  }

  public getProfile(): void {

    this.http.get<Perfil>(`${this.url}/perfil/${this.userNameLogin}`)
      .toPromise()
      .then((perfil) => {

        let profile: PoToolbarProfile = {
          //avatar: string
          title: perfil.nome,
          subtitle: perfil.email
        }

        this.profileLogado = profile
        this.eventPerfil.emit(profile)
      })
  }

}
