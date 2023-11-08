import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  isCollapsed = true;

  constructor(private router: Router) {}

  ngOnInit(): void {}

  logout(): void {
    this.router.navigateByUrl('/login');
  }

  showMenu(): boolean {
    return this.router.url !== '/login';
  }
}
