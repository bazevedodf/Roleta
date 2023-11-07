import { Component, OnInit, ViewChild } from '@angular/core';
import { RoletaService } from '@app/Services/roleta.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { NgxWheelComponent, TextAlignment, TextOrientation } from '../../Components/ngx-wheel/ngx-wheel.component';
import { AccountService } from '@app/Services/account.service';
import { UserGame } from '@app/model/UserGame';
import { GiroRoleta } from '@app/model/GiroRoleta';
import { interval, Observable } from 'rxjs';
import { PaymentService } from '@app/Services/payment.service';
import { Pix } from '@app/model/Pix';

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.scss']
})
export class GameComponent implements OnInit {
  @ViewChild(NgxWheelComponent, { static: false }) wheel: any;

  user!: UserGame;
  freeBet = true;
  valorAposta: number = 0;
  valorPremio: number = 0;
  GiroRoleta = {} as GiroRoleta;
  btnPressionado = false;

  reelDialog: string = "TESTE SUA SORTE";
  idToLandOn: number = 0;
  textOrientation: TextOrientation = TextOrientation.HORIZONTAL
  textAlignment: TextAlignment = TextAlignment.INNER
  soudFile: string = 'assets/sounds/tick.mp3';

  showDialog = false;
  tituloDialo: string = "Bem Vindo!";
  mensagemDialog: string = "Para multiplicar e lucrar, escolha uma opção de depósito ou teste a sua sorte na versão demo.";

  tituloResult: string = "";
  showResult = false;

  tituloAlert: string = "";
  mensagemAlert: string = "";
  showAlert = false;

  novoObservable! : Observable<number>;

  items: any = [
    { "id": 1, "text": "0x", "fillStyle": "#003399", "textFillStyle": "white", "textFontSize": "22" },
    { "id": 2, "text": "0.5x", "fillStyle": "#006633", "textFillStyle": "white", "textFontSize": "22" },
    { "id": 3, "text": "1.2x", "fillStyle": "#ffcc00", "textFillStyle": "white", "textFontSize": "22" },
    { "id": 4, "text": "3x", "fillStyle": "#ff6600", "textFillStyle": "white", "textFontSize": "22" },
    { "id": 5, "text": "0x", "fillStyle": "#003399", "textFillStyle": "white", "textFontSize": "22" },
    { "id": 6, "text": "0.5x", "fillStyle": "#006633", "textFillStyle": "white", "textFontSize": "22"},
    { "id": 7, "text": "1.2x", "fillStyle": "#ffcc00", "textFillStyle": "white", "textFontSize": "22"},
    { "id": 8, "text": "5x", "fillStyle": "#ff6600", "textFillStyle": "white", "textFontSize": "22"},
    { "id": 9, "text": "100x", "fillStyle": "#fff", "textFillStyle": "black", "textFontSize": "22"},
    { "id": 10, "text": "0x", "fillStyle": "#003399", "textFillStyle": "white", "textFontSize": "22"},
    { "id": 11, "text": "0.5x", "fillStyle": "#006633", "textFillStyle": "white", "textFontSize": "22"},
    { "id": 12, "text": "1.2x", "fillStyle": "#ffcc00", "textFillStyle": "white", "textFontSize": "22"},
    { "id": 13, "text": "7x", "fillStyle": "#ff6600", "textFillStyle": "white", "textFontSize": "22"},
    { "id": 14, "text": "0x", "fillStyle": "#003399", "textFillStyle": "white", "textFontSize": "22"},
    { "id": 15, "text": "0.5x", "fillStyle": "#006633", "textFillStyle": "white", "textFontSize": "22"},
    { "id": 16, "text": "1.2x", "fillStyle": "#ffcc00", "textFillStyle": "white", "textFontSize": "22"},
    { "id": 17, "text": "9x", "fillStyle": "#ff6600", "textFillStyle": "white", "textFontSize": "22"},
    { "id": 18, "text": "30x", "fillStyle": "#fff", "textFillStyle": "black", "textFontSize": "22"}

/*  0 vermelho
    0.5 preto
    1 = ffcc00 amarelo
    5 = ff6600 lanranja
    30 = 003399 azul
    100 = 339966 verde */
  ];

  constructor(private spinner: NgxSpinnerService,
              private paymentService: PaymentService,
              private accountService: AccountService,
              private roletaService: RoletaService,
              private toastr: ToastrService){
  }

  ngOnInit(): void{
    this.GetUserData();
    this.observablePagamentos(10000);
  }

  public async GetUserData(): Promise<void>{
    await this.accountService.getUserLogado(true).subscribe({
      next:(result: UserGame) => {
        if (result){
          if (!this.user){
            if (result.saldoDeposito === 0)
              this.showDialog = true;
            else{
              this.freeBet = false;
              this.reelDialog = "SELECIONE O VALOR E GIRE";
            }
          }
          this.user = result;
          if (this.user.pagamentos.length > 0)
            this.observablePagamentos(5000);
          this.accountService.setUserGame(result);
        }
      },
      error:(error: any) =>{
        if (error.status == 401){
          this.toastr.error(error.error, "Erro!");
        }
        else
          this.toastr.error("Erro de conexão, tente mais tarde!.","Erro!");
      }
    });
  }

  public observablePagamentos(secunds: number): void{
    if (!this.novoObservable){
      this.novoObservable = interval(secunds);
      this.novoObservable.subscribe(vezes => {
        console.log(vezes);
        this.verificarPagamentos();
      })
    }
  }

   public verificarPagamentos(): void{
    this.user.pagamentos.forEach(pag => {
      //PENDING - APPROVED - EXPIRED - RETURNED - ERROR
      if(pag.status === "PENDING"){
        console.log(pag.transactionId + ": Pendente");
        this.GetUserData();
        //this.consultaPix(pag.transactionId);
      }
    });
  }
/*
  public consultaPix(transactionId: string): void{
    this.paymentService.consultarPix(transactionId).subscribe({
      next:(result: Pix) => {
        if(result.status !== "PENDING")
          this.GetUserData();
      },
      error:(error: any) =>{
        if (error.status == 401){
          this.toastr.error(error.error, "Erro!");
        }
        else
          this.toastr.error("Erro de conexão, tente mais tarde!.","Erro!");
      }
    });
  } */

  public before(): void{
    //console.log('before');
  }

  public after(): void{
    this.valorPremio = this.valorAposta * this.GiroRoleta.multiplicador;
    if (!this.freeBet){
      this.user.saldoDeposito -= this.valorAposta;
      this.user.saldoSaque += this.valorPremio;
      this.tituloResult = "Você multiplicou por "+ this.GiroRoleta.multiplicador + "x";
      this.showResult = true;
    }
    else{
      if(this.user.saldoDeposito > 0){
        this.freeBet = false
        this.setAlertBox("","Você possue saldo, é hora de multiplicar");
      }
      else
        this.setDialogoFreeBet("Você multiplicou por "+ this.GiroRoleta.multiplicador + "x", "Que tal lucrar pra valer dessa vez?!");
    }
    this.btnPressionado = false;
  }

  public proxima(): void{
    this.valorPremio = 0;
    this.showResult = false;
  }

  public setDialogoFreeBet(titulo : string, texto : string) : void{
    this.tituloDialo = titulo;
    this.mensagemDialog = texto;
    this.showDialog = true;
  }

  public setAlertBox(titulo : string, texto : string) : void{
    this.tituloAlert = titulo;
    this.mensagemAlert = texto;
    this.showAlert = true;
  }

  public spin(): void{
    if(this.btnPressionado)
      return;
    else
      this.btnPressionado = true

    if (!this.freeBet){
      if (this.user.saldoDeposito < 1)
      {
        this.setAlertBox("Saldo Insuficiente!", "Você não possue saldo suficiente para fazer uma nova aposta, tente depositar ou converter seu lucro em saldo. Menu -> Saque -> Converter Lucros");
        this.btnPressionado = false;
        return;
      }

      if (this.user.saldoDeposito < this.valorAposta)
      {
        this.setAlertBox("Saldo Insuficiente!", "Você está tentando apostar um valor superior ao seu saldo de jogo.");
        this.btnPressionado = false;
        return;
      }
    }

    if(this.valorAposta > 0){
      this.roletaService.SpinBet(this.valorAposta, this.freeBet).subscribe({
        next:(result: GiroRoleta) => {
          this.GiroRoleta = result;
          this.idToLandOn = result.posicao;
          this.wheel.reset();
          this.wheel.spin(result.posicao);
        },
        error:(error: any) =>{
          if (error.status == 401){
            this.toastr.error(error.error, "Erro!");
          }
          else
            this.toastr.error("Erro de conexão, tente mais tarde!.","Erro!");
        }
      });
    }
    else{
      this.setAlertBox("Valor de Aposta Inválido", "Você precisa selecionar uma opção de aposta abaixo!");
      this.btnPressionado = false;
      return;
    }
  }

}
