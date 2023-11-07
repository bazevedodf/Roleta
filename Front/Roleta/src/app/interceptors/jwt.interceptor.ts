import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, take } from 'rxjs';
import { User } from '@app/model/identity/user';
import { AccountService } from '@app/Services/account.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor( private accountService: AccountService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {

    if(localStorage.getItem('user')){
      let currentUser : User;
      currentUser = JSON.parse(localStorage.getItem('user') ?? '{}');
      if (currentUser !== null){
        request = request.clone({
          setHeaders: {
            Authorization: `Bearer ${currentUser.token}`
          }
        });
      }
    }

    return next.handle(request);
  }
}
