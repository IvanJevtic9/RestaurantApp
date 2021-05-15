import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { RegistrationModel } from '../models/Registration.model';
import { Config } from './config.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService{
  constructor(private http: HttpClient, private config: Config){}

  authenticate(username: string, password: string): boolean{
    console.log(username + ' ' + password);

    return false;
  }

  navigateToMainPage(){

  }

  register(model: RegistrationModel){
    console.log(model);
    this.http.post(
      this.config.location + this.config.REGISTER_API,
      model
    ).subscribe(response => {
      console.log(response);
    });
    // JA -> http post request -> Server
    // JA <- Observer <-  response <- Server
    // Observer.
  }
}
