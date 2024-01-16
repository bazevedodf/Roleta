import { Component, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AccountService } from '@app/Services/account.service';
import { UserGame } from '@app/model/UserGame';
import { UserUpdate } from '@app/model/identity/UserUpdate';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit, OnDestroy {

  public form!: FormGroup;
  public user = {} as UserUpdate;

  get f(): any{ return this.form.controls; }

  constructor(private renderer: Renderer2,
              private fb: FormBuilder,
              private spinner: NgxSpinnerService,
              private toastr: ToastrService,
              private accountService: AccountService,){

  }

  ngOnInit(): void {
    this.GetUserData(false);
    this.Validacao();
    this.renderer.addClass(document.querySelector('body'), "bg-01");
  }

  public validaCSS(campo: FormControl | AbstractControl): any {
    return { "is-invalid": campo?.errors && campo.touched };
  }

  private Validacao(): void {
    this.form = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(84)]],
      lastName: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(84)]],
      email: ['', [Validators.required, Validators.email]],
      cpf: ['', [Validators.required, Validators.minLength(11)]],
      phoneNumber: ['', [Validators.required, Validators.minLength(11)]],
      tipoChavePix: ['cpf', Validators.required],
      chavePix: [],
    });
  }

  public switchMask(): string{
    switch(this.f.tipoChavePix.value){
      case "CPF": return "000.000.000-00"
      case "TELEFONE": return "(00) 00000-0000"
      default: return "";
    }
  }

  public SalvarDados(): void{
    this.spinner.show();
    if (this.form.valid) {
      debugger;
      this.accountService.putUserGame(this.user).subscribe({
        next: (user: UserUpdate) => {
          this.toastr.success('Gravado com Sucesso!', 'Sucesso');
        },
        error: (error: any) => {
          console.error(error);
          this.spinner.hide();
          this.toastr.error('Error ao gravar dados', 'Erro');
        }
      }).add(() => this.spinner.hide());
    }
  }

  public GetUserData(includeDados: boolean): void{
    this.spinner.show();
    this.accountService.getUserLogado(includeDados).subscribe({
      next:(result: UserGame) => {
        if (result){
          console.log(result);
          this.user = {...result};
        }
      },
      error:(error: any) =>{
        if (error.status == 401){
          this.toastr.error(error.error, "Erro!");
        }
        else
          this.toastr.error("Erro de conexão, tente mais tarde!.","Erro!");
      }
    }).add(()=>{this.spinner.hide()});
  }

  ngOnDestroy(): void {
    this.renderer.removeClass(document.querySelector('body'), "bg-01");
  }
}
