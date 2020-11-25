import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './layouts/home/home.component';
import { LoginComponent } from './layouts/login/login.component';
import { ConfigurarSalaComponent } from './layouts/sala/configurar-sala/configurar-sala.component';
import { SalaResolver } from './guards/sala.resolver';
import { CadastroContaUsuarioComponent } from './layouts/cadastro-conta-usuario/cadastro-conta-usuario.component';
import { ConfirmarEmailComponent } from './layouts/confirmar-email/confirmar-email.component';
import { ConfirmarResetarSenhaComponent } from './layouts/confirmar-resetar-senha/confirmar-resetar-senha.component';
import { EditarPerfilComponent } from './layouts/editar-perfil/editar-perfil.component';
import { PerfilResolver } from './guards/perfil.resolver';
import { LoginGuard } from './guards/login.guard';
import { SalaGuard } from './guards/sala.guard';
import { SalaComponent } from './layouts/sala/sala.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'login', component: LoginComponent, canActivate: [LoginGuard]},
  { path: 'cadastrar-conta', component: CadastroContaUsuarioComponent },
  { path: 'confirmar-email', component: ConfirmarEmailComponent },
  { path: 'confirmar-resetar-senha', component: ConfirmarResetarSenhaComponent},
  { path: 'editar-perfil', component: EditarPerfilComponent, resolve: {perfil: PerfilResolver}},
  { path: 'sala/:salaId', component: SalaComponent, canActivate: [SalaGuard], resolve: {sala: SalaResolver} },
  { path: '**', redirectTo: ''},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
