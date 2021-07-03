import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Dish, Menu } from 'src/app/models/Food.model';

@Component({
  selector: 'app-menu-form',
  templateUrl: './menu-form.component.html',
  styleUrls: ['./menu-form.component.css']
})
export class MenuFormComponent implements OnInit {
  @Output('menu') onMenu: EventEmitter<Menu> = new EventEmitter();
  display = false;

  menu: Menu = {
    id: -1,
    name: "",
    dishes: []
  };

  mDish: Dish;

  private menuFile: File = undefined;

  constructor() { }

  ngOnInit(): void {
    this.mDish = this.creatyEmptyDish();
  }

  creatyEmptyDish(){
    return {
      id: 0,
      name: "",
      attributes: [],
      ingredients_list: "",
      price: 0
    }
  }

  onFileSelect($event){

  }

  removeDish(index: number){
  }

  onConfirm(){
    console.log("confirm");
    this.onMenu.emit(this.menu);
  }
  onCancle(){
    this.onMenu.emit(null);
  }
}
