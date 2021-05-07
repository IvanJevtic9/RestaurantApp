import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class AuthService{
  constructor(){}

  authenticate(username: string, password: string): boolean{
    console.log(username + ' ' + password);

    return false;
  }

  navigateToMainPage(){

  }
}
