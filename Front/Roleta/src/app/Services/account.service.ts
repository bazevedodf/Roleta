import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, ReplaySubject, map, take } from 'rxjs';
import { JwtHelperService } from "@auth0/angular-jwt";

import { environment } from '@environments/environment';
import { User } from '@app/model/identity/user';
import { UserGame } from '@app/model/UserGame';
import { UserUpdate } from '@app/model/identity/UserUpdate';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private baseUrl = environment.apiURL+ 'api/Account/'
  private currentUserSource = new ReplaySubject<User>(1);
  public currentUser$ = this.currentUserSource.asObservable();

  private currentUserGameSource = new ReplaySubject<UserGame>(1);
  public currentUserGame$ = this.currentUserGameSource.asObservable();

  private jwtHelper = new JwtHelperService();
  public decodedToken: any;

  constructor(private http: HttpClient) { }

  public login(model: any): Observable<void>{
    return this.http.post<User>(this.baseUrl + 'Login', model).pipe(
        take(1),
        map((response:User) => {
          const user = response;
          if(user){
            this.setCurrentUser(user);
          }
        })
    );
  }

  public logout(): void{
    localStorage.removeItem('user');
    localStorage.removeItem("GameData");
    this.currentUserSource.next(null as any);
    this.currentUserSource.complete();
    this.currentUserGameSource.next(null as any);
    this.currentUserGameSource.complete();
    this.decodedToken = null;
  }

  public register(model: any): Observable<any>{
    return this.http.post<User>(this.baseUrl + 'Register', model).pipe(
      take(1),
      map((response:User) => {
        const user = response;
        if(user){
          this.setCurrentUser(user);
        }
      })
    );
  }

  public setCurrentUser(user: User) : void{
    localStorage.setItem("user", JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  public setUserGame(user: UserGame) : void{
    localStorage.setItem("GameData", JSON.stringify(user));
    this.currentUserGameSource.next(user);
  }

  public getUserGame(): any {
    let usergame: User;
    if(localStorage.getItem('GameData')){
      usergame = JSON.parse(localStorage.getItem('GameData') ?? '{}');
      return usergame;
    }
    return null;
  }

  public putUserGame(userGame: UserUpdate): Observable<UserUpdate> {
    return this.http
      .put<UserUpdate>(`${this.baseUrl}PutUserGame`, userGame)
      .pipe(take(1));
  }

  public getUserLogado(incDados: boolean): Observable<any> {
    const params = {
      includeDados : incDados
    }
    return this.http.get<UserGame>(this.baseUrl + 'GetUserGame', {params}).pipe(take(1));
  }

  public loggedIn(): boolean {
    if(localStorage.getItem('user')){
      let user: User = JSON.parse(localStorage.getItem('user') ?? '{}');
      return !this.jwtHelper.isTokenExpired(user.token);
    }
    else
      return false;
  }



}
