import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, take } from 'rxjs';
import { UserDash } from '@app/Models/Identity/UserDash';
import { AccountService } from '@app/Services/account.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private accountService: AccountService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let currentUser!: UserDash;

    this.accountService.currentUser$.pipe(take(1)).subscribe((user:UserDash) => {
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
