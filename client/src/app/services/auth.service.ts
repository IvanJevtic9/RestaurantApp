import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { RegistrationModel } from '../models/Registration.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService{
  constructor(private http: HttpClient){}

  authenticate(username: string, password: string): boolean{
    console.log(username + ' ' + password);

    return false;
  }

  navigateToMainPage(){

  }

  register(model: RegistrationModel){
    console.log(model);

  }
}
