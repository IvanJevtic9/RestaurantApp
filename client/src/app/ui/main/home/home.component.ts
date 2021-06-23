import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { Account } from 'src/app/models/Account.model';
import { Restaurant } from 'src/app/models/Restaurant.model';
import { AuthService } from 'src/app/services/auth.service';
import { MenuService } from 'src/app/services/menu.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit, OnDestroy {
  aSubscription: Subscription;
  account: Account = null;

  results: string[];
  text: string;

  restaurants: Restaurant[] = [];

  constructor(
    private authService: AuthService,
    private menuService: MenuService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.aSubscription = this.authService.account.subscribe(user => {
      this.account = user;
      this.restaurants = this.menuService.getAllRestaurants();
    });
  }

  search(e:any){
    console.log(e);

  }

  ngOnDestroy(){
    this.aSubscription.unsubscribe();
  }

  navigateToRestaurantPage(id: number){
    this.router.navigate(['restaurant',id]);
  }
}
