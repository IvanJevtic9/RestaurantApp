import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { Dish, Menu } from 'src/app/models/Food.model';
import { FormGroup, FormControl, Validators, FormArray } from '@angular/forms';
import { AdminService } from '../../../services/admin.service';
import { ConfirmationService } from 'primeng/api';
import { FileUploadService } from '../../../services/file.service';
import { attribute } from '../../../models/Food.model';

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
  editDishIndex = -1; // edit Dish

  // Att Values
  newAddonGroup: FormGroup = null;

  addAttributeFlag = false;
  editAttributeIndex = -1;
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
    if(this.editDishIndex != -1){
      this.onUpdateDish(i);
      return;
    }
    const dish: Dish = {
      name: this.addDishForm.value.name,
      id: -1,
      price: this.addDishForm.value.price,
      image: this.addDishForm.value.image,
      ingredients_list: this.addDishForm.value.ingredients_list,
      attributes: this.addDishForm.value.attribute
    };

    this.menues[i].dishes.push(dish);
    this.menuService.createNewDish(this.menues[i].id, dish, this.dishImage).subscribe(data => {
      console.log(data);
      dish.id = data.data.id;
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
    this.editDishIndex = -1;
  }

  deleteDish(menu_index: number, dish_index: number){
    console.log(this.menues[menu_index].dishes[dish_index]);
    this.menuService.deleteDish(this.menues[menu_index].dishes[dish_index].id);
    this.menues[menu_index].dishes.splice(dish_index, 1);
  }

  editDish(menu_index: number, dish_index: number){
    this.editDishIndex = dish_index;
    this.createDish()

    console.log(this.menues[menu_index].dishes[dish_index]);

    const attribute = new FormArray([]);
    this.addon_category = [];

    this.menues[menu_index].dishes[dish_index].attributes.forEach(el => {
      const values = new FormArray([]);
      const newGroup = new FormGroup({
        name: new FormControl(el.name, Validators.required),
        should_add_on_price: new FormControl(el.should_add_on_price),
        values: values
      });

      el.values.forEach((val) => {

        values.push(new FormGroup({
          name: new FormControl(val.name, Validators.required),
          price: new FormControl(val.price, Validators.required)
        }))
      });

      this.addon_category.push({
        name: el.name,
        addon_values_count: el.values.length
      });
      attribute.push(newGroup);
    });

    console.log(attribute);

    this.addDishForm = new FormGroup({
      name: new FormControl(this.menues[menu_index].dishes[dish_index].name, [Validators.required]),
      ingredients_list: new FormControl(this.menues[menu_index].dishes[dish_index].ingredients_list, [Validators.required]),
      image: new FormControl(this.menues[menu_index].dishes[dish_index].image),
      attribute: attribute,
      price: new FormControl(this.menues[menu_index].dishes[dish_index].price)
    });
  }

  onUpdateDish(menu_index: number){
    this.menues[menu_index].dishes[this.editDishIndex].name = this.addDishForm.value.name;
    this.menues[menu_index].dishes[this.editDishIndex].price = this.addDishForm.value.price;
    this.menues[menu_index].dishes[this.editDishIndex].ingredients_list = this.addDishForm.value.ingredients_list;
    this.menues[menu_index].dishes[this.editDishIndex].attributes = this.addDishForm.value.attribute;
    this.menues[menu_index].dishes[this.editDishIndex].image = this.addDishForm.value.image;

    this.menuService
      .updateExsistingDish(this.menues[menu_index].dishes[this.editDishIndex], this.dishImage)
      .subscribe(data => {
        console.log(data);
      })
    this.onAddDishClear();
  }

  editAddition(addition_index: number){
    this.addAttributeFlag = true;
    this.addAttributeFlag = true;
    this.editAttributeIndex = addition_index;
    this.addon_values = [];
    let values: FormArray;
    let atr: FormGroup;


    const control = this.addDishForm.get("attribute");
    console.log(control);
    if(control instanceof FormArray){
      const m = control.at(addition_index);
      if(m instanceof FormGroup){
        atr = m;
        const mvalue = atr.get("values");
        if(mvalue instanceof FormArray){
          values = mvalue;
          for(let i = 0; i < mvalue.length; ++i){
            this.addon_values.push({
              name: mvalue.at(i).value.name,
              price: mvalue.at(i).value.price
            });
          }
        }
      }
    }

    // Attribute group
    this.newAddonGroup = new FormGroup({
      name: new FormControl(atr.value.name, Validators.required),
      should_add_on_price: new FormControl(atr.value.should_add_on_price),
      values: values
    });

    // Choice
    this.newAttributeInput =  new FormGroup({
      name: new FormControl('', Validators.required),
      price: new FormControl(0, Validators.required)
    })
  }


  saveNewCategory(){
    if(
      this.newAddonGroup.value.name === '' ||
      this.newAddonGroup.value.values.length == 0
    ){
      return;
    }

    const control = this.addDishForm.get("attribute");
    if(control instanceof FormArray){
      if(this.editAttributeIndex != -1){
        this.addon_category[this.editAttributeIndex].name = this.newAddonGroup.value.name;
        this.addon_category[this.editAttributeIndex].addon_values_count = this.addon_values.length;
        control.removeAt(this.editAttributeIndex)
        control.insert(this.editAttributeIndex, this.newAddonGroup);
      }
      else {
        control.push(this.newAddonGroup);
        this.addon_category.push({
          name: this.newAddonGroup.value.name,
          addon_values_count: this.addon_values.length
        })
      }
      this.editAttributeIndex = -1;
      this.addAttributeFlag = false;
    }
  }

  removeAddition(addition_index: number){
    this.addon_category.splice(addition_index, 1);
    const control = this.addDishForm.get("attribute");
    if(control instanceof FormArray){
      control.removeAt(addition_index);
    }
  }
}
