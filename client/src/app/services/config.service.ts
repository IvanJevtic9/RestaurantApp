import { Injectable } from '@angular/core';

@Injectable({providedIn: 'root'})
export class Config {
  private location = 'https://localhost:44367';
  // API
  REGISTER_API = this.location + '/api/account/register';
  LOGIN_API = this.location + '/api/account/login';
  MENU_API = this.location + '/api/restaurant/menu';
  GET_MENU_BY_RESTAURANT_ID = this.location + '/api/restaurant';
  DISH_API = this.location + '/api/restaurant/menu-item';
  ORDER_API = this.location + '/api/restaurant/payment-order';
}
