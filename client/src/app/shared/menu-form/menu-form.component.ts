import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Dish, Menu } from 'src/app/models/Food.model';

@Component({
  selector: 'app-menu-form',
  templateUrl: './menu-form.component.html',
  styleUrls: ['./menu-form.component.css']
})
export class MenuFormComponent implements OnInit {
  @Input('myDishes') dishes: Dish[];
  @Output('menu') onMenu: EventEmitter<Menu> = new EventEmitter();

  allDishes: Dish[] = [];

  menu: Menu = {
    id: -1,
    name: "",
    dishes: []
  };
  selectedDish: Dish;

  constructor() { }

  ngOnInit(): void {
    if(this.dishes !== undefined){
      for(const d of this.dishes){
        this.allDishes.push(d);
      }
    }
  }

  onDishSelect(){
    for(let i = 0; i < this.allDishes.length; ++i){
      if(this.selectedDish.id == this.allDishes[i].id){
        this.menu.dishes.push(this.allDishes[i]);
        this.allDishes.splice(i, 1);
        break;
      }
    }

    this.selectedDish = null;
  }

  removeDish(index: number){
    this.allDishes.push(this.menu.dishes[index]);
    this.menu.dishes.splice(index, 1);
  }

  onConfirm(){
    console.log("confirm");
    this.onMenu.emit(this.menu);
  }
  onCancle(){
    this.onMenu.emit(null);
  }
}
