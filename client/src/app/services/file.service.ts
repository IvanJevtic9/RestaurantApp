import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FileUploadService {

  constructor() { }

  checkFileType(files: FileList){
    if (files.length === 0) {
      return;
    }
    const fileToUpload = files.item(0);

    const mimeType = files[0].type;
    if (mimeType.match(/image\/*/) == null) {
      console.log('Bad image type');
      return false;
    }
    return true;
  }

  uploadFile(file: File, callback: any){
    const reader = new FileReader();
    reader.onload = () => {
      callback(reader.result);
    };
    reader.readAsDataURL(file);
  }
}
