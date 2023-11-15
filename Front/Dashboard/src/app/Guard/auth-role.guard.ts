import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { AccountService } from '@app/Services/account.service';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthRoleGuard implements CanActivate {

  constructor(private accountService: AccountService,
              private toaster: ToastrService,
              private router: Router){}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    const isAutorized = this.accountService.user.role.includes(route.data['role']);

    if (!isAutorized){
      this.toaster.error('Você não possue autorização para acessar essa página!', 'Error');
      this.router.navigate(['/home']);
    }

    return isAutorized;
  }

}
