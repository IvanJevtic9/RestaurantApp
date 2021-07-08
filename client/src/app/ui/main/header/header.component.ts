import { Component, OnInit, OnDestroy } from '@angular/core';
import { Account } from 'src/app/models/Account.model';
import { AuthService } from '../../../services/auth.service';
import { Subscription } from 'rxjs';

//Menu
import {MenuItem} from 'primeng/api';
import { Router } from '@angular/router';
import { CartService } from '../../../services/cart.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit, OnDestroy {

  account: Account;
  accountSubscription: Subscription;
  cartSubscription: Subscription;

  acc_initials: string;
  price = 0;
  //Menu
  items: MenuItem[];

  constructor(
    private authService: AuthService,
    private router: Router,
    private cartService: CartService
  ) {}

  ngOnInit(): void {
    this.accountSubscription = this.authService.account.subscribe(account => {

      this.account = account;
      if(this.account !== null && this.account !== undefined){
        if(account.isUserType){
          this.acc_initials = account.user.firstName.charAt(0)+account.user.lastName.charAt(0);
        } else {
          this.acc_initials = account.restaurant.name.charAt(0);
        }
      }
    });

    this.cartSubscription = this.cartService.cart.subscribe(cart => {
      this.price = cart.total_price;
    });

    this.items = [
      {
        label: 'Uredite profil',
        icon: 'pi pi-fw pi-user-edit',
        command: () => {
          if(this.account.isUserType){
            this.router.navigate(['/user/details']);
          } else {
            this.router.navigate(['/admin/user/details']);
          }
        }
      },
      {
        label: 'Pregled porudzbina',
        icon: 'pi pi-fw pi-shopping-cart',
        command: () => {
          if(this.account.isUserType){
            this.router.navigate(['/my-orders']);
          } else {
            // this.router.navigate(['/admin/user/details']);
          }
        }
      },
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
    this.cartSubscription.unsubscribe();
  }

  previewCart(){
    this.router.navigate(['/cart']);
  }

  logout(){
    this.authService.logout();
  }

}
