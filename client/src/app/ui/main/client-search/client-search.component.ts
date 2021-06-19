import { Component, OnInit,OnDestroy } from '@angular/core';
import { MenuService } from 'src/app/services/menu.service';
import { Menu } from 'src/app/models/Food.model';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { Restaurant } from 'src/app/models/Restaurant.model';


@Component({
  selector: 'app-client-search',
  templateUrl: './client-search.component.html',
  styleUrls: ['./client-search.component.css'],
})
export class ClientSearchComponent implements OnInit,OnDestroy {

  subscription: Subscription;
  menus: Menu[] = [];
  restaurant: Restaurant;


  constructor(
    private menuService: MenuService,
    private activeRoute: ActivatedRoute
  ) {}



  ngOnInit(): void {
   this.subscription = this.activeRoute.params.subscribe(data=>{
    this.menus = this.menuService.getRestaurantMenues(data.id);
    this.restaurant = this.menuService.getRestaurant(data.id);
   });
  }


  ngOnDestroy():void{
    this.subscription.unsubscribe();
  }

}
