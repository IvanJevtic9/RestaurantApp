import { HttpClient, HttpErrorResponse, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { RegistrationModel } from '../models/Registration.model';
import { Config } from './config.service';
import { catchError, map } from 'rxjs/operators';
import { throwError } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class AuthService{
  token: string;

  private TOO_MANY_ATTEMPTS_TRY_LATER: 'TOO_MANY_ATTEMPTS_TRY_LATER : Too many unsuccessful login attempts. Please try again later.';
  private INVALID_PASSWORD = 'INVALID_PASSWORD';
  private EMAIL_NOT_FOUND = 'EMAIL_NOT_EXSIST';
  private USER_DISABLED = 'USER_DISABLED';
  private EMAIL_EXISTS = 'This email has been already registered.';
  private EMAIL_NOT_ACTIVE = 'USER_NOT_ACTIVE';

  processErrorMassage(errorMessages, msg: string) {
    switch (msg) {
      case this.EMAIL_EXISTS: errorMessages.push('Nalog sa datom email adresom vec postoji.'); break;
      case this.INVALID_PASSWORD: errorMessages.push('Pogresna kombinacija email i lozinke.'); break;
      case this.EMAIL_NOT_FOUND: errorMessages.push('Pogresna kombinacija email i lozinke.'); break;
      case this.USER_DISABLED: errorMessages.push('Ovaj nalog je trenutno suspendovan.'); break;
      case this.EMAIL_NOT_ACTIVE: errorMessages.push('Korisnički nalog nije aktiviran. Molimo da ga prvo aktivirajte.'); break;
      case this.TOO_MANY_ATTEMPTS_TRY_LATER: errorMessages.push('Previse neuspesnih pokusaja. Molimo pokusajte kasnije.'); break;
      default: errorMessages.push('Dogodila se nepoznata greska prilikom slanja zahteva. Molimo pokusajte kasnije.');
    }
  }

  constructor(private http: HttpClient, private config: Config){}

  authenticate(username: string, password: string): boolean{
    console.log(username + ' ' + password);

    return false;
  }

  navigateToMainPage(){

  }

  register(model: RegistrationModel){
    console.log(model);
    return this.http.post(
      this.config.location + this.config.REGISTER_API,
      model
    ).pipe(
      catchError(this.errorHandling.bind(this))
    );
  }


  errorHandling(errorRes: HttpErrorResponse) {
    console.log(errorRes);

    const errorMessages = [];

    if (!errorRes.error) {
      return throwError(errorMessages);
    }

    let arr = Object.keys(errorRes.error.errors);
    arr.forEach(key => {
      this.processErrorMassage(errorMessages, errorRes.error.errors[key]);
    });

    if (errorMessages.length === 0) { errorMessages.push('Server je trenutno nedostupan.'); }
    return throwError(errorMessages);
  }
}
