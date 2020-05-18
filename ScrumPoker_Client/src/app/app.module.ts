import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { PoModule, PoDialogService } from '@po-ui/ng-components';
import { CookieService } from 'ngx-cookie-service';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SobreComponent } from './sobre/sobre.component';
import { AuthService } from './services/auth.service';
import { CriarSalaComponent } from './home/criar-sala/criar-sala.component';
import { ConfigurarSalaComponent } from './configurar-sala/configurar-sala.component';
import { SalaService } from './services/sala.service';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './home/login/login.component';
import { SalaResolver } from './guards/sala.resolver';
import { CadastroContaUsuarioComponent } from './cadastro-conta-usuario/cadastro-conta-usuario.component';

@NgModule({
  declarations: [
    AppComponent,
    SobreComponent,
    CriarSalaComponent,
    ConfigurarSalaComponent,
    HomeComponent,
    LoginComponent,
    CadastroContaUsuarioComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    PoModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [
    AuthService,
    PoDialogService,
    CookieService,
    SalaService,
    SalaResolver],
  bootstrap: [AppComponent]
})
export class AppModule { }
