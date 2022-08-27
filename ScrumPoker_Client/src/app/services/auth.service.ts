import { Injectable, EventEmitter } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { PoToolbarProfile } from '@po-ui/ng-components';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
import { AuthRegistrar } from '../interfaces/authRegistrar.interface';
import { AuthLogin } from '../interfaces/authLogin.interface';
import { TokenJwt } from '../interfaces/tokenJwt.interface';
import { AuthResetarSenha } from '../interfaces/authResetarSenha.interface';
import { Perfil } from '../interfaces/perfil.interface';
import { Observable, OperatorFunction, lastValueFrom} from 'rxjs';
import jwtDecode, { JwtDecodeOptions, JwtPayload } from 'jwt-decode';
import { ConfirmarEmail } from '../models/confirmar-email.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  public idSala = '';
  public isJogador = true;
  public idParticipante = '';
  public name = '';
  public eventLogin = new EventEmitter();
  public eventLogout = new EventEmitter();
  public eventPerfil = new EventEmitter<PoToolbarProfile>();
  public userNameLogin = '';
  public profileLogado: PoToolbarProfile | undefined;

  private readonly jwtDescode = jwtDecode;
  private readonly url: string = environment.API;
  private decodedToken: TokenJwt | undefined;
  private perfil: Perfil | undefined;

  constructor(
    private http: HttpClient
  ) {
    this.getConfig();
    this.loadToken();
  }

  public getConfig(): void {
    const idSala = localStorage.getItem('idSala');
    const nome = localStorage.getItem('nome');

    this.idSala = !!idSala ? idSala : '';
    this.name = !!nome ? nome : '';
    this.isJogador = localStorage.getItem('isJogador') ? localStorage.getItem('isJogador') === 'true' : true;

    if (!!this.idSala) {
      const idParticipante = localStorage.getItem(this.idSala.toUpperCase());
      this.idParticipante = !!idParticipante ? idParticipante : '';
      if (!this.idParticipante) {
        this.idParticipante = this.idSala.toUpperCase() + this.aleatorio(30);
        localStorage.setItem(this.idSala.toUpperCase(), this.idParticipante);
      }
    }
  }

  public saveConfig(idSala: string, nome: string, isJogador: boolean, isIntegraJira: boolean): void {
    const dtExpires = new Date(2100, 1, 1);
    this.idSala = idSala;
    this.name = nome;
    this.isJogador = isJogador;

    localStorage.setItem('idSala', idSala);
    localStorage.setItem('nome', nome);
    localStorage.setItem('isJogador', isJogador.toString());
    localStorage.setItem('isIntegraJira', isIntegraJira.toString());

    if (!!this.idSala) {
      const idParticipante = localStorage.getItem(this.idSala.toUpperCase());
      this.idParticipante = !!idParticipante ? idParticipante : '';
      if (!this.idParticipante) {
        this.idParticipante = this.idSala.toUpperCase() + this.aleatorio(30);
        localStorage.setItem(this.idSala.toUpperCase(), this.idParticipante);
      }
    }
  }

  private aleatorio(txtTam: number): string {
    const letras = '1234567890abcdefghijlkmnopqtuvxywz';
    let txtAleatorio = '';

    for (let i = 0; i < txtTam; i++) {
      const pos = Math.floor(Math.random() * letras.length);
      txtAleatorio += letras.substring(pos, pos + 1);
    }

    return txtAleatorio;
  }

  public setCookie(nameCokie: string, valorCookie: string): void {
    localStorage.setItem(nameCokie, valorCookie);
  }

  public setSalaCookie(idSala: string): void {
    this.idSala = idSala;
    localStorage.setItem('idSala', idSala);
  }

  public login(model: AuthLogin): Observable<void> {
    return this.http
      .post(`${this.url}/user/login`, model).pipe(
        this.pipeLogin()
    );
  }

  public cadastrarConta(model: AuthRegistrar): Observable<any> {
    return this.http.post(`${this.url}/user/registrar`, model, {
      params: {
        urlConfirmaEmail: `${location.origin}/confirmar-email`
      }});
  }

  public enviarConfirmacaoEmail(userName: string): Observable<any> {
    return this.http.post(`${this.url}/user/enviar-confirmacao-email/${userName}`, null, {
      params: {
        urlConfirmaEmail: `${location.origin}/confirmar-email`
      }});
  }

  public solicitacaoResetarSenha(userName: string): Observable<any> {
    return this.http.post(`${this.url}/user/solicitacao-resetar-senha/${userName}`, null, {
      params: {
        urlCallback: `${location.origin}/confirmar-resetar-senha`
      }});
  }

  public resetarSenha(userName: string, authResetarSenha: AuthResetarSenha): Observable<any> {
    return this.http.post(`${this.url}/user/resetar-senha/`, authResetarSenha);
  }

  public confirmarEmail(token: string, userName: string): Observable<any> {
    const confirmarEmail = new ConfirmarEmail();
    confirmarEmail.token = token;
    confirmarEmail.userName = userName;

    return this.http
    .post(`${this.url}/user/confirmar-email`, confirmarEmail)
    .pipe(this.pipeLogin());
  }

  private pipeLogin(): OperatorFunction<any, void> {
    return map( (respose: any) => {
      const user = respose;
      if (user) {
        localStorage.setItem('token', user.token);
        this.loadToken();
      }
    });
  }

  private loadToken(): void {
    if (this.logado()) {
      const token = localStorage.getItem('token');
      this.decodedToken = !!token ? this.jwtDescode(token) : undefined;
      this.userNameLogin = !!this.decodedToken ? this.decodedToken.nameid : '';
      this.eventLogin.emit();
      this.getProfile();
    }
  }

  public logado(): boolean {
    const token = localStorage.getItem('token');
    const tokenDecode: JwtPayload | undefined = !!token ? this.jwtDescode(token) : undefined;
    return !!tokenDecode && !!tokenDecode.exp && new Date(tokenDecode.exp * 1000) > new Date();
  }

  public logout(): void {
    localStorage.removeItem('token');
    this.userNameLogin = '';
    this.eventLogout.emit();
  }

  public getProfile(): void {

    const request = this.http.get<Perfil>(`${this.url}/perfil/${this.userNameLogin}`)
    lastValueFrom(request)
      .then((perfil) => {

        const profile: PoToolbarProfile = {
          title: perfil.nome,
          subtitle: perfil.email
        };

        this.profileLogado = profile;
        this.eventPerfil.emit(profile);
      });
  }

}
