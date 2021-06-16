import { Injectable } from "@angular/core";
import { Restaurant } from "../models/Restaurant.model";
import { Menu } from "../models/Food.model";

@Injectable({
  providedIn: 'root'
})
export class MenuService{

  restaurants: Restaurant[] = [
    {
      id: 0,
      address:"Ustanička 224a",
      phone:"23125412",
      city:"Beograd",
      image: null,
      description: "Pice, sendvici, kuvana jela , obroci, rostilj, paste, tortilje i poslstice. Narucite i uzivajte!",
      name: "Salbo fast food - Konjarnik",
      postalCode: "11050"
    },
    {
      id: 1,
      address:"Ustanička 224a",
      phone:"23125412",
      city:"Beograd",
      image: null,
      description: "Pice, sendvici, kuvana jela , obroci, rostilj, paste, tortilje i poslstice. Narucite i uzivajte!",
      name: "Salbo fast food - Konjarnik",
      postalCode: "11050"
    }
  ]

  static menu: Menu[] = [
    {
      restaurant_id: 0,
      name: "Salbo specijaliteti",
      dishes: [
        {
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
          name: "Salbo pljeskavica",
          ingredients_list: "300 grama. Juneće meso, slanina, kulen, dimljeni kačkavalj",
          price: 370,
          attributes: []
        },
      ]
    }
  ];

  constructor(){}

  getAllRestaurants(){
    // TODO make it from backend
    return this.restaurants;
  }

  getRestaurantMenues(id: number){
    let menu: Menu[] = [];
    for(const m of menu){
      if(m.restaurant_id === id){
        menu.push(m);
      }
    }
    return menu;
  }
}
