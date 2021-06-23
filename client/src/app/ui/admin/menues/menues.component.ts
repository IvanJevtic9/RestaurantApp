import { Component, OnInit } from '@angular/core';
import { Dish, Menu } from 'src/app/models/Food.model';
import { MenuService } from 'src/app/services/menu.service';

@Component({
  selector: 'app-menues',
  templateUrl: './menues.component.html',
  styleUrls: ['./menues.component.css']
})
export class MenuesComponent implements OnInit {
  menues: Menu[] = [];
  dishes: Dish[] = [];

  addMenuFlag = false;
  
  constructor(private menuService: MenuService) { }

  ngOnInit(): void {
    this.dishes = this.menuService.getMyDishes(0);
  }

  updateMenues(menu: Menu){
    // TODO send menu data to server, update local list
    if(menu !== null){
      // TODO add or update values... depending from id
      this.menues.push(menu); 
    }
    this.addMenuFlag = false;
  }

  editMenu(){

  }

  deleteMenu(){
    
  }
}
