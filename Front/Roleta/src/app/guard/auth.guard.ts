import { Injectable } from '@angular/core';
import { CanActivate, Router  } from '@angular/router';
import { AccountService } from '@app/Services/account.service';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private router: Router,
              private accountService: AccountService,
              private toaster: ToastrService){

  }

  public canActivate(): boolean {
    if(this.accountService.loggedIn()){
      return true;
    }

    else{
      this.accountService.logout();
    }

    this.toaster.info("Efetue o login para acessar os recursos do sistema!")
    this.router.navigateByUrl('/authentication');
    return false;
  }

}
