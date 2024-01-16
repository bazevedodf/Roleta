import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, take, map, ReplaySubject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from '@environments/environment';
import { UserDash } from '@app/Models/Identity/UserDash';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  public baseUrl = environment.apiURL + 'api/Account/';

  private jwtHelper = new JwtHelperService();
  private currentUserSource = new ReplaySubject<UserDash>(1);
  public currentUser$ = this.currentUserSource.asObservable();
  public user! : UserDash;


  constructor(private http: HttpClient) { }

  public login(model: any): Observable<void>{
    return this.http.post<UserDash>(this.baseUrl + 'AfiliateLogin', model).pipe(
      take(1),
      map( (response: UserDash) => {
        this.user = response;
        if(this.user){
          this.user.role = this.getRoles(this.user.token);
          this.setCurrentUser(this.user);
        }
      })
    );
  }

  public loggedIn(): void {
    if(localStorage.getItem('user')){
      this.user = JSON.parse(localStorage.getItem('user') ?? '{}');
      if(this.user && !this.jwtHelper.isTokenExpired(this.user.token)){
        this.user.role = this.getRoles(this.user.token);
        this.setCurrentUser(this.user);
      }
      else{
        localStorage.removeItem("user");
      }
    }
  }

  private getRoles(token: string): string{
    let tokenData: any = JSON.parse(atob(token.split('.')[1]));
    return tokenData.role;
  }

  public logout(): void{
    localStorage.removeItem('user');
    this.currentUserSource.next(null as any);
    this.currentUserSource.complete();
  }

  public setCurrentUser(user: UserDash): void{
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

}
