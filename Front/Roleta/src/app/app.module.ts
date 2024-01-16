import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import {FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { HomeComponent } from './Pages/home/home.component';
import { AuthenticationComponent } from './Pages/authentication/authentication.component';

import { CollapseModule } from 'ngx-bootstrap/collapse';
import { AccordionModule } from 'ngx-bootstrap/accordion';
import { NgxSpinnerModule } from 'ngx-spinner';
import { NgxCurrencyModule } from "ngx-currency";
import { ToastrModule } from 'ngx-toastr';

import { AccountService } from './Services/account.service';
import { JwtInterceptor } from './interceptors/jwt.interceptor';
import { GameComponent } from './Pages/game/game.component';
import { DepositComponent } from './Pages/deposit/deposit.component';
import { NavbarComponent } from './Components/navbar/navbar.component';
import { RoletaComponent } from './Components/roleta/roleta.component';
import { NgxWheelModule } from './Components/ngx-wheel/ngx-wheel.module';
import { DialogComponent } from './Components/dialog/dialog.component';
import { SaqueComponent } from './Pages/saque/saque.component';
import { DateTimeFormatPipe } from './helpers/DateTimeFormat.pipe';
import { TermosDeUsoComponent } from './Pages/termos-de-uso/termos-de-uso.component';
import { PoliticaDePrivacidadeComponent } from './Pages/politica-de-privacidade/politica-de-privacidade.component';
import { provideNgxMask, NgxMaskDirective } from 'ngx-mask';
import { PerfilComponent } from './Pages/perfil/perfil.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    AuthenticationComponent,
    GameComponent,
    DepositComponent,
    NavbarComponent,
    RoletaComponent,
    DialogComponent,
    SaqueComponent,
    DateTimeFormatPipe,
    TermosDeUsoComponent,
    PoliticaDePrivacidadeComponent,
    PerfilComponent,
  ],
  imports: [
    FormsModule,
    BrowserModule,
    BrowserAnimationsModule,
    NgxWheelModule,
    ReactiveFormsModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
    NgxCurrencyModule,
    NgxMaskDirective,
    AccordionModule.forRoot(),
    CollapseModule.forRoot(),
    ToastrModule.forRoot({
      timeOut: 8000,
      positionClass: 'toast-bottom-center',
      preventDuplicates: true,
      progressBar: true
    }),
  ],
  providers: [
    AccountService,
    provideNgxMask(),
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule { }
