import { Observable } from 'rxjs';
import { Component, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { PaymentService } from '@app/Services/payment.service';
import { DadosPix } from '@app/model/DadosPix';
import { Pix } from '@app/model/Pix';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { RoletaService } from '@app/Services/roleta.service';

@Component({
  selector: 'app-deposit',
  templateUrl: './deposit.component.html',
  styleUrls: ['./deposit.component.scss']
})
export class DepositComponent  implements OnInit, OnDestroy {

  public form!: FormGroup;
  public dadosPix!: DadosPix;
  public PixDeposit = {} as Pix;
  public modoGerarPix = 'Gerar'

  public planos!: any[];

  get f(): any{ return this.form.controls; }

  constructor(private renderer: Renderer2,
              private paymentService: PaymentService,
              private roletaService: RoletaService,
              private fb: FormBuilder,
              private toastr: ToastrService,
              private spinner: NgxSpinnerService){}

  ngOnInit(): void {
    this.renderer.addClass(document.querySelector('body'), "bg-01");
    this.Validacao();
  }

  public GerarPix(): void{
    this.spinner.show();
    this.dadosPix = {... this.form.value}
    this.paymentService.gerarPix(this.dadosPix).subscribe({
      next:(result: Pix) => {
        this.PixDeposit = result;
        this.modoGerarPix = 'Gerado';
      },
      error:(error: any) =>{
        if (error.status == 401){
          this.toastr.error(error.error, "Erro!");
        }
        else
          this.toastr.error("Erro de conexão, tente mais tarde!.","Erro!");
      }
    }).add(() => {this.spinner.hide()});
  }

  public voltar(): void{
    this.PixDeposit = {} as Pix;
    this.modoGerarPix = 'Gerar'
  }

  public copyCode(): void{
    let btn = <HTMLButtonElement>document.getElementById('btnCopy');
    let contentBtn = btn.innerHTML;
    navigator.clipboard.writeText(this.PixDeposit.qrCodeText);
    btn.innerHTML = '<i class="fa-regular fa-circle-check me-2"></i>Copiado!';
    setTimeout(function(){
      btn.innerHTML = contentBtn;
    }, 2000)
  }

  public validaCSS(campo: FormControl | AbstractControl): any {
    return { "is-invalid": campo?.errors && campo.touched };
  }

  private Validacao(): void {
    this.form = this.fb.group({
      nome: ['', [Validators.required]],
      cpf: ['', [Validators.required, Validators.minLength(11), Validators.maxLength(11)]],
      valor: ['', Validators.required],
    });
  }

  ngOnDestroy(): void {
    this.renderer.removeClass(document.querySelector('body'), "bg-01");
  }

}
