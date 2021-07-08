import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';
import { BehaviorSubject, Subscription } from 'rxjs';
import { Cart, OrderItem, RestaurantOrder } from '../models/Order.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Config } from './config.service';
import { AuthService } from './auth.service';
import { Account } from '../models/Account.model';
import { Restaurant } from '../models/Restaurant.model';

export interface MyOrdersResponse{
  data: {
    deliveryTime: Date;
    id: number;
    paymentItems: any;
    restaurant: Restaurant;
    state: string;
    timeCreated: string;
    totalPrice: number;
  }[]
}

@Injectable({
  providedIn: 'root'
})
export class CartService{
  private static CART = "cart";

  private account: Account;
  private authSubscription: Subscription;

  cart: BehaviorSubject<Cart> = new BehaviorSubject(null);
  m_cart: Cart = {
    orders: [],
    total_price: 0,
  };

  constructor(
    private messageService: MessageService,
    private http: HttpClient,
    private config: Config,
    private authService: AuthService
  ){
    this.authSubscription = authService.account.subscribe(account => {
      this.account = account;
    })

    const temp = JSON.parse(localStorage.getItem(CartService.CART));
    if(temp !== null){
      this.m_cart = temp;
    }
    this.cart.next(this.m_cart);
  }

  OnDestroy(){
    this.authSubscription.unsubscribe();
  }

  addItem(orderItem: OrderItem, restaurant_id: number){
    let order = null;
    for(let i = 0; i < this.m_cart.orders.length; ++i){
      if(this.m_cart.orders[i].restaurantId == restaurant_id){
        order = this.m_cart.orders[i];
        break;
      }
    }

    if(order === null){
      this.createNewOrderEntry(orderItem, restaurant_id);
    } else {
      const total = orderItem.price * orderItem.item_count;
      this.m_cart.total_price += total;
      this.updateExsistingEntry(order, orderItem, total);
    }

    this.updateCartStorage();
  }


  private createNewOrderEntry(orderItem: OrderItem, restaurant_id: number){
    const total = orderItem.price * orderItem.item_count;
    this.m_cart.orders.push({
      total_price: total,
      restaurantId: restaurant_id,
      paymentItems: [orderItem]
    })
    this.m_cart.total_price += total;
  }

  private updateExsistingEntry(order: RestaurantOrder, orderItem: OrderItem, total: number) {
    order.total_price += total;
    let found = false;
    for(let i = 0; i < order.paymentItems.length; ++i){
      if(order.paymentItems[i].item_id == orderItem.item_id){
        if(JSON.stringify(order.paymentItems[i].attribute) === JSON.stringify(orderItem.attribute)){
          order.paymentItems[i].item_count += orderItem.item_count;
          found = true;
        }
        break;
      }
    }
    if(!found){
      order.paymentItems.push(orderItem);
    }
  }

  private updateCartStorage() {
    localStorage.setItem(CartService.CART, JSON.stringify(this.m_cart));
    this.cart.next(this.m_cart);
  }


  makeNewOrder(){
    const send_data: {
      paymentItems: string;
      totalPrice: number;
      restaurantId: number;
    }[] = [];

    this.m_cart.orders.forEach(data => {
      console.log(data.paymentItems);
      send_data.push({
        totalPrice: data.total_price,
        restaurantId: data.restaurantId,
        paymentItems: JSON.stringify(data.paymentItems)
      })
    })


    this.http.post(
      'https://localhost:44367/api/restaurant/payment-order',
      {paymentOrders: send_data},
      {headers: this.createHeader()}
    ).subscribe(result => {
      console.log(result);
      this.m_cart = {
        orders: [],
        total_price: 0,
      };
      this.updateCartStorage();
      this.messageService.add({
        summary:'Uspešno ste poslali narudžbinu',
        detail: 'Vaša narudžbina će uskoro biti obrađena.',
        severity: 'success'
      });
    })
  }

  getMyOrders(){
    return this.http.get<MyOrdersResponse>(
      'https://localhost:44367/api/restaurant/payment-order',
      {headers: this.createHeader()}
    );
  }

  removeFromCart(restaurant_id: number, item_id: number){

  }

  private createHeader(){
    return new HttpHeaders({
      'Accept': 'application/json',
      'Authorization': 'Bearer ' + this.account.token
    });
  }
}


