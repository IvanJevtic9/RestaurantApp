import { Component, OnInit } from '@angular/core';
import { Cart } from '../../../models/Order.model';
import { CartService } from '../../../services/cart.service';
import { Subscription, Observable } from 'rxjs';
import { MenuService } from '../../../services/menu.service';
import { Restaurant } from 'src/app/models/Restaurant.model';
import { AuthService } from '../../../services/auth.service';
import { Account } from '../../../models/Account.model';

@Component({
  selector: 'app-cart-preview',
  templateUrl: './cart-preview.component.html',
  styleUrls: ['./cart-preview.component.css']
})
export class CartPreviewComponent implements OnInit {

  cart: Cart = null;
  vendorUsernames: Restaurant[] = [];

  oldNumCount: number[][] = [];
  cartSubscription: Subscription;
  authSubscription: Subscription;
  account: Account = null;

  constructor(
    private cartService: CartService,
    private menuService: MenuService,
    private auth: AuthService
  ) {}

  ngOnInit(): void {
    this.authSubscription = this.auth.account.subscribe(account => {
      this.account = account;
    });

    this.cartSubscription = this.cartService.cart.subscribe( cart => {
      this.cart = cart;
      cart.orders.forEach(order => {
        const temp = this.menuService.getRestaurant(order.restaurantId);
        if(temp instanceof Observable){
          temp.subscribe(d => {
            this.vendorUsernames.push(this.menuService.getRestaurantById(order.restaurantId));
          })
        } else {
          this.vendorUsernames.push(temp);
        }
      });
    });
  }

  ngOnDestroy(){
    this.cartSubscription.unsubscribe();
    this.authSubscription.unsubscribe();
  }

  // update(att: Artical, index, column){
  //   const order = this.cart.orders[this.vendorUsernames[index]];

  //   console.log(this.oldNumCount[index][column]);
  //   let dif = att.number_of_pieces - this.oldNumCount[index][column];
  //   console.log(dif);

  //   if(dif > att.product.storage){
  //     dif = att.product.storage;
  //     att.number_of_pieces = this.oldNumCount[index][column] + dif;
  //   }

  //   order.total_items += dif;
  //   this.cart.total_items += dif;

  //   order.total_items += dif*att.product.price;
  //   this.cart.total_items += dif*att.product.price;

  //   this.oldNumCount[index][column] = att.number_of_pieces;
  //   att.product.storage += dif;

  //   this.orderService.cart.next(this.cart);
  // }

  removeArticle(restaurant_id: number, item_id: number){
    this.cartService.removeFromCart(restaurant_id, item_id);
  }
  makeNewOrder(){
    this.cartService.makeNewOrder();
  }
}
