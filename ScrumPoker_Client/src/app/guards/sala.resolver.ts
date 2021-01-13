import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Sala } from '../models/sala.model';
import { SalaService } from '../services/sala.service';

@Injectable({ providedIn: 'root' })
export class SalaResolver implements Resolve<Sala> {

  constructor(private salaService: SalaService){}

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<Sala> {
    const salaId: string = route.params.salaId;
    return this.salaService.buscarSala(salaId);
  }
}
