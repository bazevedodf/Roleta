import { Component, OnInit } from '@angular/core';
import { AccountService } from './Services/account.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Dashboard';

  constructor(private accountService: AccountService, private activatedRouter: ActivatedRoute){
  }

  ngOnInit(): void{
    this.accountService.loggedIn();
  }
}
