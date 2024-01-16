import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Carteira } from '@app/Models/Carteira';
import { UserUpdate } from '@app/Models/UserUpdate';
import { DashBoardService } from '@app/Services/dash-board.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-cadastro-detalhe',
  templateUrl: './cadastro-detalhe.component.html',
  styleUrls: ['./cadastro-detalhe.component.scss']
})
export class CadastroDetalheComponent implements OnInit{

  public form!: FormGroup;
  public userSelected = {} as UserUpdate;
  public carteira = {} as Carteira;

  public get f(): any {
    return this.form.controls;
  }

  constructor(private spinner: NgxSpinnerService,
              private toastr: ToastrService,
              private dashboradService: DashBoardService,
              private fb: FormBuilder,
              private router: Router,
              private activatedRouter: ActivatedRoute){

  }

  ngOnInit(): void {
    this.carregarUser();
    this.Validacao();
  }

  public validaCSS(campo: FormControl | AbstractControl): any {
    return { "is-invalid": campo.errors && campo.touched };
  }

  public Validacao(): void {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      firstName: ['', [Validators.minLength(3), Validators.maxLength(80)]],
      lastName: ['', [Validators.minLength(3), Validators.maxLength(80)]],
      phoneNumber: ['', [Validators.minLength(11)]],
      cpf: ['', Validators.maxLength(11)],
      chavePix: ['' ],
      tipoChavePix: ['CPF'],
      isAfiliate :  [false],
      afiliateCode : ['',],
      comissao: ['25'],
      isBlocked :  [false],
      demoAcount :  [false],
      saldoDemo :  [0],
      saldoAtual :  [0],
    });
  }

  public carregarUser(): void {
    const emailParam = this.activatedRouter.snapshot.paramMap.get('email');
    if (emailParam !== null){
      this.spinner.show();
      this.dashboradService.getUserDash(emailParam).subscribe({
        next: (user: UserUpdate) => {
          if(user.comissao === 0)
            user.comissao = 25;
          this.carteira = {...user.carteira};
          this.userSelected = {...user};

        },
        error:(error:any) => {
          this.toastr.error('Erro ao carregar usuário');
        }
      }).add(() => this.spinner.hide());
    }
  }

  public salvarUser(): void {
    this.spinner.show();
    if (this.form.valid) {
      this.userSelected = {
        id: this.userSelected.id,
        carteira: this.carteira,
        dataCadastro: this.userSelected.dataCadastro,
        ...this.form.value
      };

      this.dashboradService.putUser(this.userSelected).subscribe({
        next: (user: UserUpdate) => {
          this.toastr.success('Usuário salvo com Sucesso!', 'Sucesso');
          this.carregarUser();
        },
        error: (error: any) => {
          console.error(error);
          this.spinner.hide();
          this.toastr.error('Error ao salvar Usuário', 'Erro');
        }
      }).add(() => this.spinner.hide());
    }
  }

  public btnCancelar(): void{
    this.router.navigate([`cadastros/lista`]);
  }

}
