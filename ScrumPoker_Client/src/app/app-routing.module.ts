import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { ConfigurarSalaComponent } from './components/configurar-sala/configurar-sala.component';
import { SalaResolver } from './guards/sala.resolver';
import { CadastroContaUsuarioComponent } from './components/cadastro-conta-usuario/cadastro-conta-usuario.component';
import { ConfirmarEmailComponent } from './components/confirmar-email/confirmar-email.component';
import { ConfirmarResetarSenhaComponent } from './components/confirmar-resetar-senha/confirmar-resetar-senha.component';


const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'cadastrar-conta', component: CadastroContaUsuarioComponent },
  { path: 'confirmar-email', component: ConfirmarEmailComponent },
  { path: 'confirmar-resetar-senha', component: ConfirmarResetarSenhaComponent},
  { path: 'configuracao/:salaId', component: ConfigurarSalaComponent,
  resolve: {sala: SalaResolver} },
  { path: '**', redirectTo: ''},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
