import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, take } from 'rxjs';
import { User } from '@app/Models/Identity/User';
import { AccountService } from '@app/Services/account.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private accountService: AccountService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let currentUser!: User;

    this.accountService.currentUser$.pipe(take(1)).subscribe((user:User) => {
      currentUser = user;

      if(currentUser){
        request = request.clone(
          {
            setHeaders: {
              Authorization: `Bearer ${currentUser.token}`
            }
          }
        );
      }

    });

    return next.handle(request);
  }
}
