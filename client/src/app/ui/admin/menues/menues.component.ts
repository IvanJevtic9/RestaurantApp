import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { Dish, Menu } from 'src/app/models/Food.model';
import { MenuService } from 'src/app/services/menu.service';
import { FormGroup, FormControl, Validators, FormArray } from '@angular/forms';
import { attribute } from '../../../models/Food.model';
import { AdminService } from '../../../services/admin.service';

@Component({
  selector: 'app-menues',
  templateUrl: './menues.component.html',
  styleUrls: ['./menues.component.css']
})
export class MenuesComponent implements OnInit {
  menues: Menu[] = [];
  dishes: Dish[] = [];

  addMenuFlag = false;
  // Index that specifies opend tab
  toggle = -1;
  addDishFlag = false;

  // Att Values
  newAddonGroup: FormGroup = null;

  addAttributeFlag = false;
  addon_values: {name: string, price: number}[] = [];
  addon_category: {name: string, addon_values_count: number}[] = [];
  newAttributeInput: FormGroup = null;

  addDishForm: FormGroup;

  constructor(private menuService: AdminService) { }

  ngOnInit(): void {
    this.menues = this.menuService.getMyMenues();
    this.createNewEmptyDishForm();
    this.createNewEmptyAddon();
  }

  editMenu(){

  }

  deleteMenu(){

  }

  updateMenues(menu: Menu){
    if(menu !== null){
      console.log(menu);
      this.menues.push(menu);
    }
    this.addMenuFlag = false;
  }

  toggleDishMenu(i: number){
    this.toggle = (this.toggle == i) ? -1 : i;
    this.addDishFlag = false;
    if(i != -1){
      this.createNewEmptyDishForm();
      this.addAttributeFlag = false;
    }
  }

  createNewEmptyDishForm(){
    this.addDishForm = new FormGroup({
      name: new FormControl('', [Validators.required]),
      ingredients_list: new FormControl('', [Validators.required]),
      attribute: new FormArray([]),
      price: new FormControl(0)
    });
  }

  createNewEmptyAddon(){
    this.addAttributeFlag = true;
    this.addon_values = [];
    // Attribute group
    this.newAddonGroup = new FormGroup({
      name: new FormControl('', Validators.required),
      should_add_on_price: new FormControl(false),
      values: new FormArray([])
    });
    // Choice
    this.newAttributeInput = this.createNewAttributeValue();
  }

  createNewAttributeValue(){
    return new FormGroup({
      name: new FormControl('', Validators.required),
      price: new FormControl(0, Validators.required)
    })
  }

  createDish(){
    this.addDishFlag = true;
    this.addAttributeFlag = false;
  }

  addAttribute(){
    console.log(this.newAttributeInput);

    if(this.newAttributeInput.value.name === '' || this.newAttributeInput.value.price === ''){
      console.log('ee');
      return;
    }

    const values = this.newAddonGroup.get("values");
    if(values instanceof FormArray){
      console.log('eee');

      values.push(this.newAttributeInput);
      this.addon_values.push({
        name: this.newAttributeInput.value.name,
        price: this.newAttributeInput.value.price
      });
      this.newAttributeInput = this.createNewAttributeValue();
    }
  }

  saveNewCategory(){
    const control = this.addDishForm.get("attribute");
    console.log(control);

    if(
      this.newAddonGroup.value.name === '' ||
      this.newAddonGroup.value.values.length == 0
    ){
      return;
    }

    if(control instanceof FormArray){
      control.push(this.newAddonGroup);
      this.addon_category.push({
        name: this.newAddonGroup.value.name,
        addon_values_count: this.addon_values.length
      })
      this.addAttributeFlag = false;
    }
  }

  cancelAddingNewcategory(){
    this.addAttributeFlag = false;
  }

  onDishSubmit(i: number){
    if(this.addDishForm.value.name === ''){
      return;
    }
    this.menues[i].dishes.push({
      name: this.addDishForm.value.name,
      id: -1,
      price: this.addDishForm.value.price,
      ingredients_list: this.addDishForm.value.ingredients_list,
      attributes: this.addDishForm.value.attribute
    });
    console.log(this.dishes);
    this.onAddDishFinish();

    // this.addDishForm.value.attribute.forEach(element => {
    //   this.dishes[this.dishes.length-1].attributes.push({

    //   });
    //   console.log(element);

    // });
  }

  onAddDishFinish(){
    this.addDishFlag = false;
    this.addAttributeFlag = false;

    this.createNewEmptyAddon();
    this.createNewEmptyDishForm();
    this.addon_values = [];
    this.addon_category = [];
  }

}
