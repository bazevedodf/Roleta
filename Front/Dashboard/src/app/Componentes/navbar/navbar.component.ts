import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserDash } from '@app/Models/Identity/UserDash';
import { AccountService } from '@app/Services/account.service';
import { take } from 'rxjs';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {

  isCollapsed = true;

  constructor(public accountService: AccountService,
              private router: Router) {}

  ngOnInit():void {
    this.accountService.loggedIn();
  }

  public verificaRole(roles : string) : boolean{
    let list = roles.split(',');
    for(var i in list) {
      if(this.accountService.user.role.includes(list[i]))
        return true
    }
    return false;
  }

  logout(): void {
    this.accountService.logout();
    this.router.navigateByUrl('/login');
  }

  showMenu(): boolean {
    return this.router.url !== '/login';
  }
}
