import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './Pages/home/home.component';
import { LoginComponent } from './Pages/login/login.component';
import { PagamentosComponent } from './Pages/pagamentos/pagamentos.component';
import { AuthGuard } from './Guard/auth.guard';
import { AuthRoleGuard } from './Guard/auth-role.guard';
import { CadastrosComponent } from './Pages/cadastros/cadastros.component';
import { CadastroDetalheComponent } from './Pages/cadastros/cadastro-detalhe/cadastro-detalhe.component';
import { CadastroListaComponent } from './Pages/cadastros/cadastro-lista/cadastro-lista.component';
import { AfiliadosComponent } from './Pages/afiliados/afiliados.component';
import { AfiliadoListaComponent } from './Pages/afiliados/afiliado-lista/afiliado-lista.component';
import { ConfiguracaoComponent } from './Pages/configuracao/configuracao.component';

const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'cadastros', redirectTo: 'cadastros/lista' },
  { path: 'afiliados', redirectTo: 'afiliados/lista' },
  { path: '', runGuardsAndResolvers: 'always', canActivate: [AuthGuard],
    children: [
      { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
      { path: 'home/:afl', component: HomeComponent, canActivate: [AuthGuard] },
      { path: 'depositos', component: PagamentosComponent, canActivate: [AuthGuard, AuthRoleGuard], data:{ role: 'Admin' } },
      { path: 'configuracao', component: ConfiguracaoComponent, canActivate: [AuthGuard, AuthRoleGuard], data:{ role: 'Admin' } },
      { path: 'afiliados', component: AfiliadosComponent, canActivate: [AuthGuard, AuthRoleGuard], data:{ role: 'Admin' },
        children: [
          { path: 'lista', component: AfiliadoListaComponent }
        ]
      },
      { path: 'cadastros', component: CadastrosComponent, canActivate: [AuthGuard, AuthRoleGuard], data:{ role: 'Admin' },
        children: [
          { path: 'detalhe/:email', component: CadastroDetalheComponent },
          { path: 'lista', component: CadastroListaComponent }
        ]
      },
    ],
  },
  { path: '**', redirectTo: 'home', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
