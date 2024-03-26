import { CUSTOM_ELEMENTS_SCHEMA, NgModule, LOCALE_ID } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgxSpinnerModule } from 'ngx-spinner';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { ToastrModule } from 'ngx-toastr';
import { ModalModule } from 'ngx-bootstrap/modal';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { ptBrLocale } from 'ngx-bootstrap/locale';
import { NgxCurrencyModule } from 'ngx-currency';
import { NgxMaskDirective, NgxMaskPipe, provideNgxMask } from 'ngx-mask';

import { JwtInterceptor } from './interceptor/jwt.interceptor';

import { DateTimeFormatPipe } from './Helpers/DateTimeFormat.pipe';
import { DateFormatPipe } from './Helpers/DateFormat.pipe';
import { PagamentosComponent } from './Pages/pagamentos/pagamentos.component';
import { NavbarComponent } from './Componentes/navbar/navbar.component';
import { LoginComponent } from './Pages/login/login.component';
import { HomeComponent } from './Pages/home/home.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { CadastrosComponent } from './Pages/cadastros/cadastros.component';
import { CadastroDetalheComponent } from './Pages/cadastros/cadastro-detalhe/cadastro-detalhe.component';
import { CadastroListaComponent } from './Pages/cadastros/cadastro-lista/cadastro-lista.component';
import { AfiliadosComponent } from './Pages/afiliados/afiliados.component';
import { AfiliadoListaComponent } from './Pages/afiliados/afiliado-lista/afiliado-lista.component';
import { ConfiguracaoComponent } from './Pages/configuracao/configuracao.component';

defineLocale('pt-br', ptBrLocale);

@NgModule({
  declarations: [
    AppComponent,
    PagamentosComponent,
    NavbarComponent,
    LoginComponent,
    HomeComponent,
    DateTimeFormatPipe,
    DateFormatPipe,
    CadastrosComponent,
    CadastroDetalheComponent,
    CadastroListaComponent,
    AfiliadosComponent,
    AfiliadoListaComponent,
    ConfiguracaoComponent
  ],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    NgxMaskDirective,
    NgxMaskPipe,
    NgxSpinnerModule,
    NgxCurrencyModule,
    PaginationModule.forRoot(),
    CollapseModule.forRoot(),
    BsDropdownModule.forRoot(),
    BsDatepickerModule.forRoot(),
    ModalModule.forRoot(),
    TooltipModule.forRoot(),
    ToastrModule.forRoot({
      timeOut: 8000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
      progressBar: true
    }),
  ],
  providers: [
                {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
                provideNgxMask()],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule { }
