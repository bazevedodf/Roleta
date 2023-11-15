import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '@app/Services/account.service';
import { UserLogin } from '@app/Models/Identity/userLogin';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  public model = {} as UserLogin;

  constructor(private accountService: AccountService,
              private router: Router,
              private toaster: ToastrService){

  }

  ngOnInit(): void {}

  public login(): void{
    this.accountService.login(this.model).subscribe({
      next:(response) => {
        this.router.navigateByUrl('/home');
      },
      error:(error:any) => {
        if(error.status == 401)
          this.toaster.error("Usuário ou senha invalidos!", "Erro");
        else if(error.status == 0)
          this.toaster.error("Erro de comunicação com API", "Erro");
        else
          this.toaster.error(error.error, "Erro");
      }
    });
  }

}
