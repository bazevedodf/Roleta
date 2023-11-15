import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '@app/Models/Identity/User';
import { AccountService } from '@app/Services/account.service';
import { take } from 'rxjs';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {

  isCollapsed = true;
  user = {} as User;

  constructor(public accountService: AccountService,
              private router: Router) {}

  ngOnInit():void {
    this.user = this.accountService.user;
  }

  logout(): void {
    this.accountService.logout();
    this.router.navigateByUrl('/login');
  }

  showMenu(): boolean {
    return this.router.url !== '/login';
  }
}
