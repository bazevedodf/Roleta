import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './Pages/home/home.component';
import { AuthenticationComponent } from './Pages/authentication/authentication.component';
import { GameComponent } from './Pages/game/game.component';
import { DepositComponent } from './Pages/deposit/deposit.component';
import { RoletaComponent } from './Components/roleta/roleta.component';
import { AuthGuard } from './guard/auth.guard';
import { SaqueComponent } from './Pages/saque/saque.component';
import { TermosDeUsoComponent } from './Pages/termos-de-uso/termos-de-uso.component';
import { PoliticaDePrivacidadeComponent } from './Pages/politica-de-privacidade/politica-de-privacidade.component';

const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'user', component: AuthenticationComponent},
  { path: 'user/:tp', component: AuthenticationComponent},
    { path: 'game', component: GameComponent, canActivate:[AuthGuard] },
  { path: 'deposit', component: DepositComponent, canActivate:[AuthGuard] },
  { path: 'saque', component: SaqueComponent, canActivate:[AuthGuard] },
  { path: 'termos-de-uso', component: TermosDeUsoComponent },
  { path: 'politica-de-privacidade', component: PoliticaDePrivacidadeComponent },
  { path: 'roleta', component: RoletaComponent },
  { path: '**', redirectTo: 'home', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
