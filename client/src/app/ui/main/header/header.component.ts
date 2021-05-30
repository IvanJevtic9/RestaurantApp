import { Component, OnInit, OnDestroy } from '@angular/core';
import { Account } from 'src/app/models/Account.model';
import { AuthService } from '../../../services/auth.service';
import { Subscription } from 'rxjs';

//Menu
import {MenuItem} from 'primeng/api';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit, OnDestroy {

  account: Account;
  accountSubscription: Subscription;
  acc_initials: string;

  //Menu
  items: MenuItem[];

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.accountSubscription = this.authService.account.subscribe(account => {
      this.account = account;
      if(this.account !== null){
        this.acc_initials = account.user.firstName.charAt(0)+account.user.lastName.charAt(0);
      }
    });

    this.items = [
      {label: 'Uredite profil', icon: 'pi pi-fw pi-user-edit'},
      {label: 'Pregled porudzbina', icon: 'pi pi-fw pi-shopping-cart'},
      {
        label: 'Odjavite se',
        icon:'pi pi-fw pi-sign-out',
        command: () => {
          this.logout();
        }
      }
    ];
  }

  ngOnDestroy(){
    this.accountSubscription.unsubscribe();
  }

  logout(){
    this.authService.logout();
  }

}
