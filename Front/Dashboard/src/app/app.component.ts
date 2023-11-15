import { Component } from '@angular/core';
import { AccountService } from './Services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Dashboard';

  constructor(private accountService: AccountService){
  }

  ngOnInit(): void{
    this.accountService.loggedIn();
  }
}
