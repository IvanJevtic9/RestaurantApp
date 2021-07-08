import { Injectable } from "@angular/core";
import { Restaurant } from "../models/Restaurant.model";
import { Dish, Menu } from "../models/Food.model";
import { HttpClient } from '@angular/common/http';
import { Config } from "./config.service";
import { map, tap } from 'rxjs/operators';
import { Observable } from 'rxjs';

interface RestaurantmenuResponse{
  data:{
    id: number,
    menuCategories:{
      menuItems:{
        id:number;
        name: string;
        description: string;
        attributes: string;
        price: number;
        imageUrl: string
      }[];
      id:number;
      name: string;
      imageUrl: string;
    }[],
    galleryImages: string[]
  }
}
@Injectable({
  providedIn: 'root'
})
export class MenuService{
  private restaurants: Restaurant[] = [];

  constructor(private http: HttpClient, private config: Config){}

  getAllRestaurants(){
    return this.http.get<Restaurant[]>(
      this.config.GET_MENU_BY_RESTAURANT_ID
    ).pipe(
      tap(result => {
        this.restaurants = result;
      })
    )
  }

  getMyDishes(id: number){
    // TODO: Should retrive dishes from server according to account id
    // return MenuService.dishes;
  }

  getRestaurant(id: number): Restaurant | Observable<Restaurant[]>{
    if(this.restaurants.length == 0){
      return this.getAllRestaurants();
    }
    for(let i = 0; i < this.restaurants.length; ++i){
      if(this.restaurants[i].id == id)
        return this.restaurants[i];
    }
    return null;
  }

  getRestaurantById(id: number){
    for(let i = 0; i < this.restaurants.length; ++i){
      if(this.restaurants[i].id == id)
        return this.restaurants[i];
    }
    return null;
  }

  getRestaurantMenues(id: number){
    return this.http.get<RestaurantmenuResponse>(
      this.config.GET_MENU_BY_RESTAURANT_ID + '/' + id
    ).pipe(map(data => {
      const m = data.data.menuCategories;
      const menues: Menu[] = [];
      m.forEach(category => {
        const menu: Menu = {
          id: 0,
          dishes: [],
          name: category.name,
          image: category.imageUrl
        };
        menues.push(menu);
        category.menuItems.forEach(dish => {
          menu.dishes.push({
            attributes: JSON.parse(dish.attributes),
            id: dish.id,
            ingredients_list: dish.description,
            name: dish.name,
            price: dish.price,
            image: dish.imageUrl
          })
        })
      })
      return {
        menues: menues,
        gallery: data.data.galleryImages
      };
    }));
  }
}
