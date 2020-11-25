import { EventEmitter, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoadPageService {
  public loadPage = new EventEmitter<boolean>();

  constructor() { }
}
