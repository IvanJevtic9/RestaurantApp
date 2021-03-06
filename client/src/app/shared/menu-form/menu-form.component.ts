import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { Dish, Menu } from 'src/app/models/Food.model';
import { FileUploadService } from '../../services/file.service';

@Component({
  selector: 'app-menu-form',
  templateUrl: './menu-form.component.html',
  styleUrls: ['./menu-form.component.css']
})
export class MenuFormComponent implements OnInit {
  @Output('menu') onMenu: EventEmitter<{menu: Menu, file: File}> = new EventEmitter();
  @Input('editMenu') editMenu: Menu;

  display = false;
  errorFlag = false;

  menu: Menu = {
    id: -1,
    image: undefined,
    name: "",
    dishes: []
  };

  imgURL: any = null;
  fileToUpload: File = null;

  constructor(private uploader: FileUploadService) { }

  ngOnInit(): void {
    if(this.editMenu !== undefined){
      this.imgURL = this.editMenu.image;
      this.menu.id = this.editMenu.id;
      this.menu.name = this.editMenu.name;
      this.menu.image = this.editMenu.image;
    }
  }

  handleFileInput(files: any){
    this.fileToUpload = files[0];

    if(this.uploader.checkFileType(files)){
      this.uploader.uploadFile(files[0], (img) => {
        this.imgURL  = img;
      });
    }
  }

  removeImg(){
    this.imgURL = null;
    this.fileToUpload = null;
  }

  onConfirm(){
    if(this.menu.name === ''){
      this.errorFlag = true
      return;
    }
    this.menu.image = this.imgURL;
    this.onMenu.emit({menu: this.menu, file: this.fileToUpload});
  }

  onCancel(){
    this.onMenu.emit((this.editMenu !== undefined) ?  {menu: this.editMenu, file: null} : null);
  }
}
