import { Injectable, OnDestroy } from '@angular/core';
import { AuthService } from './auth.service';
import { Account } from 'src/app/models/Account.model';
import { Subscription } from 'rxjs';
import { Menu, attribute, Dish } from '../models/Food.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Config } from './config.service';
import { map, tap } from 'rxjs/operators';

interface RestaurantResponse{
  data:{
    galleryImages: any[],
    menuCategories:{
      id: number,
      imageUrl: string,
      name: string,
      menuItems: any[]
    }[]
    id: number
  }
}
@Injectable({
  providedIn: 'root'
})
export class AdminService {

  private account: Account;
  private authSubscription: Subscription;

  //TEST baza
  static menues: Menu[] = [
    {
      id: 0,
      name: "Salbo specijaliteti",
      dishes: [
        {
          id: 2,
          name: "Salbo dorucak",
          ingredients_list: "3 jaja, šunka, slanina, pečurke, dimljeni kačkavalj, pršuta, peršun",
          price: 300,
          attributes: [
            {
              multiple_select: false,
              should_add_on_price: true,
              name: "Izbor",
              values: [
                {name: "Omlet", price: 0},
                {name: "Jaja na oko", price: 0}
              ]
            }
          ]
        },
        {
          id: 3,
          name: "Salbo pljeskavica",
          ingredients_list: "300 grama. Juneće meso, slanina, kulen, dimljeni kačkavalj",
          price: 370,
          attributes: []
        },
      ]
    },
    {
      id:0,
      name: "Rostilj porcija",
      dishes:[
        {
          id: 3,
          name:"Rolovani cevapi",
          ingredients_list:"Somun. Rostilj mesu u slanini",
          price:320,
          attributes:[
            {
              multiple_select:false,
              should_add_on_price:true,
              name:"Izbor",
              values:[
                {name:"Urnebes", price: 20},
                {name:"Kupus salata", price: 30}
              ]
            }
          ]
         },
         {
          id: 6,
          name:"Ustipci 250 grama",
          ingredients_list:"Somun. Rostilj meso mesano sa kackavaljem, slaninom i tucanom paprikom",
          price:320,
          attributes:[]
         }
       ]
    },
  ];

  constructor(private auth: AuthService, private http: HttpClient, private config: Config) {
    this.authSubscription = auth.account.subscribe(account => {
      this.account = account;
    })
  }

  OnDestroy(){
    this.authSubscription.unsubscribe();
  }

  // Meni APIs
  createNewMenu(data: {menu: Menu, file: File}){
    const formData = new FormData();
    formData.append("name", data.menu.name);
    formData.append("manuBanner", data.file);

    return this.http.post<{data: {id: number}}>(
      this.config.MENU_API,
      formData,
      {headers: this.createHeader()}
    )
  }

  deleteMenu(id: number){
    return this.http.delete(
      this.config.MENU_API + '/' + id,
      {headers: this.createHeader()}
    ).subscribe(data => {
      console.log(data);
    })
  }

  updateMenu(id: number,name: string, file: File) {
    const formData = new FormData();
    if(name !== null){
      formData.append("name", name);
    }
    if(file !== null){
      formData.append("manuBanner", file);
    }
    return this.http.put<{data:{name:string, imageUrl: string}}>(
      this.config.MENU_API + '/' + id,
      formData,
      {headers: this.createHeader()}
    );
  }

  getMyMenues(){
    // Get with token
    return this.http.get<RestaurantResponse>(
      this.config.GET_MENU_BY_RESTAURANT_ID + this.account.restaurant.restaurantId
    ).pipe(
      map((value) => {
        const menues: Menu[] = [];
        value.data.menuCategories.forEach(element => {
          const menu: Menu = {
            id: element.id,
            image: element.imageUrl,
            name: element.name,
            dishes: []
          };

          console.log(element.imageUrl);


          element.menuItems.forEach(element => {
            menu.dishes.push({
              id: element.id,
              name: element.name,
              attributes: JSON.parse(element.attributes),
              ingredients_list: element.description,
              price: element.price,
              image: element.imageUrl
            })
          });
          console.log(menu);

          menues.push(menu);
        });

        return menues;
      })
    );
  }

  // Dish APIs
  createNewDish(menu_id: number, dish: Dish, file: File){
    const formData = new FormData();
    formData.append("menuId", ''+menu_id);
    formData.append("name", dish.name);
    formData.append("description", dish.ingredients_list);
    formData.append("attributes", JSON.stringify(dish.attributes));
    formData.append("price", ''+dish.price);
    formData.append("itemImage", file);

    return this.http.post<{data: {id: number; imageUrl: string}}>(
      this.config.DISH_API,
      formData,
      {headers: this.createHeader()}
    )
  }

  deleteDish(dish_id: number){
    return this.http.delete(
      this.config.DISH_API + '/' + dish_id,
      {headers: this.createHeader()}
    ).subscribe(data => { console.log(data);
    });
  }

  private createHeader(){
    return new HttpHeaders({
      'Accept': 'application/json',
      'Authorization': 'Bearer ' + this.account.token
    });
  }
}
