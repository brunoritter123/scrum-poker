import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { PoModule, PoDialogService } from '@po-ui/ng-components';
import { CookieService } from 'ngx-cookie-service';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SobreComponent } from './components/sobre/sobre.component';
import { AuthService } from './services/auth.service';
import { CriarSalaComponent } from './components/home/criar-sala/criar-sala.component';
import { ConfigurarSalaComponent } from './components/configurar-sala/configurar-sala.component';
import { SalaService } from './services/sala.service';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/home/login/login.component';
import { SalaResolver } from './guards/sala.resolver';
import { CadastroContaUsuarioComponent } from './components/cadastro-conta-usuario/cadastro-conta-usuario.component';
import { ConfirmarEmailComponent } from './components/confirmar-email/confirmar-email.component';
import { ConfirmarResetarSenhaComponent } from './components/confirmar-resetar-senha/confirmar-resetar-senha.component';
import { EditarPerfilComponent } from './components/editar-perfil/editar-perfil.component';

@NgModule({
  declarations: [
    AppComponent,
    SobreComponent,
    CriarSalaComponent,
    ConfigurarSalaComponent,
    HomeComponent,
    LoginComponent,
    CadastroContaUsuarioComponent,
    ConfirmarEmailComponent,
    ConfirmarResetarSenhaComponent,
    EditarPerfilComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    PoModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule
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
