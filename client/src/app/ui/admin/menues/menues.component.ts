import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { Dish, Menu } from 'src/app/models/Food.model';
import { FormGroup, FormControl, Validators, FormArray } from '@angular/forms';
import { AdminService } from '../../../services/admin.service';
import { ConfirmationService } from 'primeng/api';
import { FileUploadService } from '../../../services/file.service';

export enum AddMenuState{
  NO_ADDING,
  ADD_NEW,
  UPDATE_EXSISTING
};

@Component({
  selector: 'app-menues',
  templateUrl: './menues.component.html',
  styleUrls: ['./menues.component.css'],
  providers:[ConfirmationService]
})
export class MenuesComponent implements OnInit {
  menues: Menu[] = [];
  dishes: Dish[] = [];

  menuAdditionState: AddMenuState = AddMenuState.NO_ADDING;

  // Index that specifies opend menu tab
  toggle = -1;
  editIndex = -1;
  addDishFlag = false;
  editDishFlag = false; // edit Dish

  // Att Values
  newAddonGroup: FormGroup = null;

  addAttributeFlag = false;
  addon_values: {name: string, price: number}[] = [];
  addon_category: {name: string, addon_values_count: number}[] = [];
  newAttributeInput: FormGroup = null;

  addDishForm: FormGroup;
  dishImage: File = null;

  constructor(
    private menuService: AdminService,
    private confirmationService: ConfirmationService,
    private uploader: FileUploadService
  ) { }

  ngOnInit(): void {
    this.menuService.getMyMenues().subscribe(data => {
      this.menues = data;
    });
    this.createNewEmptyDishForm();
    this.createNewEmptyAddon();
  }

  editMenu(index: number){
    this.toggle = -1;
    this.menuAdditionState = AddMenuState.UPDATE_EXSISTING;
    this.editIndex = index;

  }

  deleteMenu(index: number){
    this.confirmationService.confirm({
      target: event.target,
      message: 'Zaista želite izbrisati sekciju menija?',
      icon: 'pi pi-exclamation-triangle',
      acceptLabel: 'Da',
      rejectLabel: 'Ne',
      accept: () => {
        this.menuService.deleteMenu(this.menues[index].id);
        this.menues.splice(index, 1);
      },
      reject: () => {
        //reject action
      }
    });
  }

  createNewMenu(){
    this.menuAdditionState = AddMenuState.ADD_NEW;
    // TODO make distincion between updating menu
  }

  updateMenues(data: {menu: Menu, file: File}){
    if(this.menuAdditionState === AddMenuState.UPDATE_EXSISTING){
      if(
        data.menu.name !== this.menues[this.editIndex].name ||
        data.file !== null
      ){
        let name = data.menu.name;
        const image = data.file;

        if(data.menu.name !== this.menues[this.editIndex].name){
          this.menues[this.editIndex].name = data.menu.name;
        } else {
          name = null;
        }

        if(data.file !== null){
          this.menues[this.editIndex].image = data.menu.image;
        }

        this.menuService.updateMenu(data.menu.id, name, image).subscribe((response) => console.log(response));
      }
    }
    else if(data != null && data.menu !== null){
      this.menuService.createNewMenu(data).subscribe(result => {
        data.menu.id = result.data.id;
        console.log(result);
      });
      this.menues.push(data.menu);
    }
    this.menuAdditionState = AddMenuState.NO_ADDING;
    this.editIndex = -1;
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
      image: new FormControl(''),
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

  uploadDishImage(files: any){
    this.dishImage = files[0];

    if(this.uploader.checkFileType(files)){
      this.uploader.uploadFile(files[0], (img) => {

        this.addDishForm.value.image  = img;
      });
    }
  }

  removeImage(){
    this.addDishForm.value.image = '';
    this.dishImage = null;
  }

  createDish(){
    this.addDishFlag = true;
    this.addAttributeFlag = false;
  }

  onDishSubmit(i: number){
    if(this.addDishForm.value.name === ''){
      return;
    }
    const dish: Dish = {
      name: this.addDishForm.value.name,
      id: -1,
      price: this.addDishForm.value.price,
      ingredients_list: this.addDishForm.value.ingredients_list,
      attributes: this.addDishForm.value.attribute
    };

    this.menues[i].dishes.push(dish);
    this.menuService.createNewDish(this.menues[i].id, dish, this.dishImage).subscribe(data => {
      console.log(data);
      dish.id = data.data.id;
      dish.image = data.data.imageUrl;
    })

    this.onAddDishClear();
  }

  onAddDishClear(){
    this.addDishFlag = false;
    this.addAttributeFlag = false;

    this.createNewEmptyAddon();
    this.createNewEmptyDishForm();
    this.addon_values = [];
    this.addon_category = [];
    this.dishImage = null;
  }

  deleteDish(menu_index: number, dish_index: number){
    console.log(this.menues[menu_index].dishes[dish_index]);
    this.menuService.deleteDish(this.menues[menu_index].dishes[dish_index].id);
    this.menues[menu_index].dishes.splice(dish_index, 1);
  }

  editDish(menu_index: number, dish_index: number){

    this.addDishForm.value.name = this.menues[menu_index].dishes[dish_index].name;
    this.addDishForm.value.ingredients_list = this.menues[menu_index].dishes[dish_index].ingredients_list;
    // this.addDishForm.value.name = this.menues[menu_index].dishes[dish_index].name;
    // this.addDishForm.value.name = this.menues[menu_index].dishes[dish_index].name;
    // this.addDishForm.value.name = this.menues[menu_index].dishes[dish_index].name;
    // this.addDishForm.value.name = this.menues[menu_index].dishes[dish_index].name;
  }

}
