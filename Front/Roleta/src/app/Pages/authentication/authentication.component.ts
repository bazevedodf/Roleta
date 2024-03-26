import { Component, OnInit } from '@angular/core';
import { AbstractControl, AbstractControlOptions, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from '@app/Services/account.service';
import { UserLogin } from '@app/model/identity/userLogin';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from "ngx-spinner";
import { ValidatorField } from '@app/helpers/ValidatorField';
import { User } from '@app/model/identity/user';

@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  styleUrls: ['./authentication.component.scss'],
})
export class AuthenticationComponent implements OnInit {

  public form!: FormGroup;
  public flip: string = 'login';
  public model = {} as UserLogin;
  public user = {} as User;
  public afiliateCode!: string;

  get f(): any{ return this.form.controls; }

  constructor(private accountService: AccountService,
              private activateRouter: ActivatedRoute,
              private router: Router,
              private fb: FormBuilder,
              private spinner: NgxSpinnerService,
              private toastr: ToastrService) { }

  ngOnInit(): void {
    if(this.accountService.loggedIn()){
      this.router.navigateByUrl('/game');
    }
    this.modoLogin();
    this.Validacao();
    this.verificaAfiliado();
  }

  private modoLogin(){
    const modo = this.activateRouter.snapshot.paramMap.get('tp');
    if(modo !== 'register')
      this.flip = 'login'
    else
    this.flip = 'register'
  }

  private verificaAfiliado(): void{
    const aflCode = this.activateRouter.snapshot.queryParams['afl'];
    if (aflCode !== undefined) {
      localStorage.setItem("AfiliateCode", aflCode);
    }
  }

  private Validacao(): void {

    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.ComparaCampos("password", "confirmPassword"),
    };

    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      //password: ['', [Validators.required, Validators.minLength(6), Validators.pattern('(?=.*[a-z])(?=.*[0-9]).{6,}')]],
      confirmPassword: ['', Validators.required],
    }, formOptions);
  }

  register(): void{
    this.spinner.show();
    this.user = { ... this.form.value};
    if(localStorage.getItem('AfiliateCode')){
      const code = localStorage.getItem('AfiliateCode');
      this.user.afiliateCode = !code ? '' : code;
    }

    this.accountService.register(this.user).subscribe({
      next : () => {
        this.router.navigateByUrl('/game');
        this.toastr.success("Cadastro realizado com sucesso!","Sucesso");
      },
      error: (error: any) => {
        if (error.status == 400) this.toastr.error(error.message, "Erro");

        if(error.status == 403) this.toastr.error("Acesso negado, codigo: 403", "Erro");
        else{
          this.toastr.error(error.message, "Erro");
        }
        this.form.reset();
      }
    }).add(() => {this.spinner.hide()});
  }

  public validaCSS(campo: FormControl | AbstractControl): any {
    return { "is-invalid": campo?.errors && campo.touched };
  }

  public login(): void{
    if (this.model.login === undefined || this.model.password === undefined){
      this.toastr.error('Login ou senha inválidos');
      return;
    }
    this.spinner.show();
    this.accountService.login(this.model).subscribe({
      next :() => {
        this.router.navigateByUrl('/game')
      },
      error :(error: any) => {
        if (error.status == 401){
          this.model.login = "";
          this.model.password = "";
          this.toastr.error('Login ou senha inválidos');
        }
        else{
          this.toastr.error(error.message);
        }
      }
    }).add(() => {this.spinner.hide();});
  }

  toggleFlip() {
    this.flip = (this.flip == 'register') ? 'login' : 'register';
  }
}
