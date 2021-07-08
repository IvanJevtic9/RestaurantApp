import { Component, OnInit,OnDestroy } from '@angular/core';
import { MenuService } from 'src/app/services/menu.service';
import { Dish, Menu } from 'src/app/models/Food.model';
import { ActivatedRoute } from '@angular/router';
import { Subscription, Observable } from 'rxjs';
import { Restaurant } from 'src/app/models/Restaurant.model';
import { CartService } from '../../../services/cart.service';


@Component({
  selector: 'app-client-search',
  templateUrl: './client-search.component.html',
  styleUrls: ['./client-search.component.css'],
})
export class ClientSearchComponent implements OnInit,OnDestroy {

  subscription: Subscription;
  menus: Menu[] = [];
  restaurant: Restaurant = null;
  gallery: string[] = [];
  displayItemFlag = false;
  displayDish: Dish = null;
  count: number = 1;


  constructor(
    private menuService: MenuService,
    private activeRoute: ActivatedRoute,
    private cartService: CartService
  ) {}

  ngOnInit(): void {
   this.subscription = this.activeRoute.params.subscribe(data=>{
    this.menuService.getRestaurantMenues(data.id).subscribe(data => {
      this.gallery = data.gallery;
      this.menus = data.menues;
    });
    const result = this.menuService.getRestaurant(data.id);
    this.setRestaurant(result, data.id);
   });
  }

  ngOnDestroy():void{
    this.subscription.unsubscribe();
  }

  private setRestaurant(result: Restaurant | Observable<Restaurant[]>, id: number){
    if(result instanceof Observable){
      result.subscribe(el => {
        const r = this.menuService.getRestaurant(id);
        this.setRestaurant(r, id);
      })
    } else {
      this.restaurant = result;
    }
  }

  displayItem(dish: Dish){
    this.displayItemFlag = true;
    this.displayDish = dish;
    console.log(this.displayDish.image);

  }

  addDishToCart(){
    this.cartService.addItem({
      item_id: this.displayDish.id,
      image: this.displayDish.image,
      name: this.displayDish.name,
      attribute: [],
      item_count: this.count,
      price: this.displayDish.price
    }, this.restaurant.id);

    this.displayItemFlag = false;
    this.count = 1;
    this.displayDish = null;
  }


}
