import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ConfigurarSalaComponent } from './configurar-sala/configurar-sala.component';
import { SalaResolver } from './guards/sala.resolver';
import { CadastroContaUsuarioComponent } from './cadastro-conta-usuario/cadastro-conta-usuario.component';


const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'cadastrar-conta', component: CadastroContaUsuarioComponent },
  { path: 'configuracao/:salaId', component: ConfigurarSalaComponent,
  resolve: {sala: SalaResolver} },
  { path: '**', redirectTo: ''},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
