import { Component, OnInit, Renderer2, OnDestroy } from '@angular/core';
import { AbstractControl, AbstractControlOptions, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AccountService } from '@app/Services/account.service';
import { RoletaService } from '@app/Services/roleta.service';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { Saque } from '@app/model/Saque';
import { UserGame } from '@app/model/UserGame';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-saque',
  templateUrl: './saque.component.html',
  styleUrls: ['./saque.component.scss']
})
export class SaqueComponent implements OnInit, OnDestroy{

  public user = {} as UserGame;
  public viewForm : boolean = false;
  public form!: FormGroup;
  public valorSaque: number = 0;

  get f(): any{ return this.form.controls; }

  constructor(private renderer: Renderer2,
              private spinner: NgxSpinnerService,
              private fb: FormBuilder,
              private accountService: AccountService,
              private roletaService: RoletaService,
              private toastr: ToastrService){
  }

  ngOnInit(): void {
    this.GetUserData();
    this.getValorSaque();
    this.Validacao();
    this.renderer.addClass(document.querySelector('body'), "bg-01");
  }

  public showForm(): void{
    this.viewForm = true;
  }

  public validaCSS(campo: FormControl | AbstractControl): any {
    return { "is-invalid": campo?.errors && campo.touched };
  }

  private Validacao(): void {
     const formOptions: AbstractControlOptions = {
      validators: ValidatorField.ComparaSaldo("valor", "saldo"),
    };

    this.form = this.fb.group({
      valor: ['', [Validators.required, Validators.min(this.valorSaque)]],
      saldo: [this.user.carteira?.saldoAtual, [Validators.required]]
    }, formOptions);
  }

  public getValorSaque(){
    this.roletaService.GetRoleta().subscribe({
      next:(result: any) => {
        if (result){
          this.valorSaque = result.valorSaque;
        }
      },
      error:(error: any) =>{
        this.toastr.error("Erro de conexão, tente mais tarde!","Erro!");
      }
    });
  }

  public GetUserData(): void{
    this.accountService.getUserLogado(true).subscribe({
      next:(result: UserGame) => {
        if (result){
          this.user = result;
          this.Validacao();
          this.accountService.setUserGame(result);
        }
      },
      error:(error: any) =>{
        if (error.status == 401){
          this.toastr.error(error.error, "Erro!");
        }
        else
          this.toastr.error("Erro de conexão, tente mais tarde!","Erro!");
      }
    });
  }

  public solicitarSaque(): void{
    debugger;
    let valor = this.f.valor.value;
    this.spinner.show();
    this.roletaService.Saque(valor).subscribe({
      next:(result: Saque) => {
        if (result) {
          this.GetUserData();
          this.form.reset();
          this.toastr.success("Saque solicitado com sucesso!","Sucesso");
        }
      },
      error:(error: any) => {
        this.form.reset();
        this.toastr.error(error.error, "Erro");
      }
    }).add(()=>{this.spinner.hide()});
  }

  ngOnDestroy(): void {
    this.renderer.removeClass(document.querySelector('body'), "bg-01");
  }

}
