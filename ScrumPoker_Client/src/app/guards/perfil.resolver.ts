import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Perfil } from '../interfaces/perfil.interface';
import { PerfilService } from '../services/perfil.service';
import { AuthService } from '../services/auth.service';

@Injectable({ providedIn: 'root' })
export class PerfilResolver implements Resolve<Perfil> {

  constructor(
    private perfilService: PerfilService,
    private authService: AuthService){}

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<Perfil> {
    const userName = this.authService.userNameLogin;
    return this.perfilService.buscarPerfil(userName);
  }
}
