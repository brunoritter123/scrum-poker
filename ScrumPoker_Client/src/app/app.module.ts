import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { PoModule, PoPageModule, PoDialogService } from '@po-ui/ng-components';
import { CookieService } from 'ngx-cookie-service';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SobreComponent } from './layouts/sobre/sobre.component';
import { AuthService } from './services/auth.service';
import { CriarSalaComponent } from './layouts/home/criar-sala/criar-sala.component';
import { ConfigurarSalaComponent } from './layouts/sala/configurar-sala/configurar-sala.component';
import { SalaService } from './services/sala.service';
import { HomeComponent } from './layouts/home/home.component';
import { LoginComponent } from './layouts/login/login.component';
import { SalaResolver } from './guards/sala.resolver';
import { CadastroContaUsuarioComponent } from './layouts/cadastro-conta-usuario/cadastro-conta-usuario.component';
import { ConfirmarEmailComponent } from './layouts/confirmar-email/confirmar-email.component';
import { ConfirmarResetarSenhaComponent } from './layouts/confirmar-resetar-senha/confirmar-resetar-senha.component';
import { EditarPerfilComponent } from './layouts/editar-perfil/editar-perfil.component';
import { SalaComponent } from './layouts/sala/sala.component';
import { JogadoresComponent } from './layouts/sala/jogadores/jogadores.component';
import { AdministradoresComponent } from './layouts/sala/administradores/administradores.component';
import { VotacaoComponent } from './layouts/sala/votacao/votacao.component';
import { ResultadosComponent } from './layouts/sala/resultados/resultados.component';
import { SalaHubService } from './services/sala-hub.service';
import { LoadPageService } from './services/load-page.service';
import { CartaComponent } from './lib/components/carta/carta.component';

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
    EditarPerfilComponent,
    SalaComponent,
    JogadoresComponent,
    AdministradoresComponent,
    VotacaoComponent,
    ResultadosComponent,
    CartaComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    PoPageModule,
    PoModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule
  ],
  providers: [
    AuthService,
    PoDialogService,
    CookieService,
    SalaHubService,
    LoadPageService,
    SalaService,
    SalaResolver],
  bootstrap: [AppComponent]
})
export class AppModule { }
