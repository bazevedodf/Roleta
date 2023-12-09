import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from '@app/Services/account.service';
import { UserGame } from '@app/model/UserGame';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {

  public isCollapsed = true;
  public userLogado = false;
  public userGame = {} as UserGame;

  constructor(private accountService: AccountService,
              private router: Router){

  }

  ngOnInit(): void {
    this.userLogado = this.accountService.loggedIn();
    this.accountService.currentUserGame$.subscribe({
      next:(result: UserGame) => {
        this.userGame = result;
      },
      error:(error: any) =>{
        console.log(error);
      }
    });
  }

  public getSaldo(): number{
    if(this.userGame.demoAcount)
      return this.userGame?.carteira?.saldoDemo;
    else
      return this.userGame?.carteira?.saldoAtual;;
  }

  public logout(): void{
    this.accountService.logout();
    this.router.navigateByUrl('/home');
  }

}
