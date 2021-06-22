import { Injectable } from "@angular/core";
import { Restaurant } from "../models/Restaurant.model";
import { Dish, Menu } from "../models/Food.model";

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

  static menues: Menu[] = [
    {
      restaurant_id: 0,
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
      restaurant_id:0,
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

  static dishes: Dish[] =[
    {
      id: 0,
      name: "Kaprićoza",
      image: "pizza.webp",
      ingredients_list: "Pelat, sir, šunka, pečurke. Maksimalan broj dodataka: 4",
      attributes: [
        {
          multiple_select: false,
          should_add_on_price: false,
          name: "Veličine",
          values: [
            {name: "Junior – 24 Cm", price: 560},
            {name: "Standard – 30 Cm", price: 810},
            {name: "Junior Integralna - 24 Cm", price: 990},
            {name: "Standard Integralna - 30 Cm", price: 890}
          ]
        },
        {
          multiple_select: true,
          name: "Dodaci Za Pice I Izbor Umaka",
          should_add_on_price: true,
          values: [
            {name: "Sir", price:80},
            {name: "Mocarela", price:190},
            {name: "Kulen", price:100}
          ]
        }
      ],
      price: 560
    },
    {
      id: 1,
      name: "Pizza Kiflizza",
      image: "pizza2.webp",
      ingredients_list: "38 cm. Rub: kiflice punjene Moja kravica krem sirom, susam. Pelat, sir, šunka, pečurke, paprika, kulen. Maksimalan broj dodataka: 1",
      attributes: [
        {
          multiple_select: true,
          name: "Prilozi",
          should_add_on_price: true,
          values: [
            {name: "Sir", price:80},
            {name: "Mocarela", price:190},
            {name: "Kulen", price:100}
          ]
        }
      ],
      price: 1320
    },
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
    {
      id: 6,
      name:"Ustipci 250 grama",
      ingredients_list:"Somun. Rostilj meso mesano sa kackavaljem, slaninom i tucanom paprikom",
      price:320,
      attributes:[]
     }
  ];

  constructor(){}

  getAllRestaurants(){
    // TODO make it from backend
    return this.restaurants;
  }

  getMyDishes(id: number){
    // TODO: Should retrive dishes from server according to account id
    return MenuService.dishes;
  }

  getRestaurant(id: number){
    // TODO get restaurant from server
    for(const restaurant of this.restaurants){
      if(restaurant.id == id){
        return restaurant;
      }
    }
    return null;
  }

  getRestaurantMenues(id: number){
    let menu: Menu[] = [];
    for(const m of MenuService.menues){
      if(m.restaurant_id == id){
        menu.push(m);
      }
    }
    return menu;
  }
}
