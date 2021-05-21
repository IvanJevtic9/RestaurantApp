import { Component, OnInit, OnDestroy } from '@angular/core';
import { Account } from 'src/app/models/Account.model';
import { AuthService } from '../../../services/auth.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit, OnDestroy {

  account: Account;
  accountSubscription: Subscription;
  acc_initials: string;

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.accountSubscription = this.authService.account.subscribe(account => {
      console.log(account);

      this.account = account;
      this.acc_initials =
        account.user.firstName.charAt(0)+account.user.lastName.charAt(0);
    });
  }

  ngOnDestroy(){
    this.accountSubscription.unsubscribe();
  }

}
