import { Injectable, OnDestroy } from '@angular/core';
import { AuthService } from './auth.service';
import { Account } from 'src/app/models/Account.model';
import { Subscription } from 'rxjs';
import { Menu } from '../models/Food.model';

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

  constructor(private auth: AuthService) {
    this.authSubscription = auth.account.subscribe(account => {
      this.account = account;
    })
  }

  OnDestroy(){
    this.authSubscription.unsubscribe();
  }

  // Meni APIs

  getMyMenues(){
    // Get with token
    let menu: Menu[] = [];
    for(const m of AdminService.menues){
      if(m.id == 0){
        menu.push(m);
      }
    }
    return menu;
  }
}
