import { HttpClient, HttpErrorResponse, HttpHeaders, JsonpClientBackend } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { RegistrationModel } from '../models/Registration.model';
import { Config } from './config.service';
import { catchError, map, tap } from 'rxjs/operators';
import { BehaviorSubject, throwError } from "rxjs";
import { Account } from "../models/Account.model";
import { stringify } from "@angular/compiler/src/util";
import { MessageService } from "primeng/api";



@Injectable({
  providedIn: 'root'
})
export class AuthService{
  account: BehaviorSubject<Account>;

  private autoLogoutTimer: any = null;

  public ACCOUNT = "account";

  private INVALID_PASSWORD = 'Invalid email or password.';
  private EMAIL_EXISTS = 'This email has been already registered.';
  private TOO_MANY_ATTEMPTS_TRY_LATER: 'TOO_MANY_ATTEMPTS_TRY_LATER : Too many unsuccessful login attempts. Please try again later.';
  private USER_DISABLED = 'USER_DISABLED';
  private EMAIL_NOT_ACTIVE = 'USER_NOT_ACTIVE';

  processErrorMassage(errorMessages, msg: string) {
    switch (msg) {
      case this.EMAIL_EXISTS: errorMessages.push('Nalog sa datom email adresom vec postoji.'); break;
      case this.INVALID_PASSWORD: errorMessages.push('Pogresna kombinacija email i lozinke.'); break;
      case this.USER_DISABLED: errorMessages.push('Ovaj nalog je trenutno suspendovan.'); break;
      case this.EMAIL_NOT_ACTIVE: errorMessages.push('Korisnički nalog nije aktiviran. Molimo da ga prvo aktivirajte.'); break;
      case this.TOO_MANY_ATTEMPTS_TRY_LATER: errorMessages.push('Previse neuspesnih pokusaja. Molimo pokusajte kasnije.'); break;
      default: errorMessages.push('Dogodila se nepoznata greska prilikom slanja zahteva. Molimo pokusajte kasnije.');
    }
  }

  constructor(
    private http: HttpClient,
    private config: Config,
    private messageService: MessageService
  ){
    const acc = JSON.parse(localStorage.getItem(this.ACCOUNT));
    this.account = new BehaviorSubject<Account>(acc);
  }

  authenticate(username: string, password: string){
    return this.http.post<any>(
      this.config.LOGIN_API,
      { email: username, password: password }
    ).pipe(
      catchError(this.errorHandling.bind(this)),
      map( data => {
        const account = new Account(data.data.token);
        console.log('Logovao se: ');
        console.log(account);
        this.updateAccountData(account);

        this.autoLogout();

        return this.account.value.isUserType;
      }
    ));
  }

  private autoLogout(){
    const expireDuration = this.account.value.expDate.getTime() - new Date().getTime();
    console.log(expireDuration);

    this.autoLogoutTimer = setTimeout(() => {
      this.autoLogoutTimer = null;
      this.logout();
    }, expireDuration);
  }

  logout(){
    if(this.autoLogoutTimer !== null) {
      clearTimeout(this.autoLogoutTimer);
    } else {
      this.messageService.add({
        sticky: true,
        summary:'Odjavljeni ste.',
        detail: 'Vaša sesija je istekla. Molimo prijavite se ponovo.',
        severity: 'warn'
      });
    }
    localStorage.removeItem(this.ACCOUNT);
    this.account.next(null);
  }

  updateAccountData(account: Account){
    this.account.next(account);
    localStorage.setItem(this.ACCOUNT, JSON.stringify(account));
  }

  register(model: RegistrationModel){
    console.log(model);
    return this.http.post(
      this.config.REGISTER_API,
      model
    ).pipe(
      catchError(this.errorHandling.bind(this))
    );
  }


  errorHandling(errorRes: HttpErrorResponse) {
    const errorMessages = [];

    if (!errorRes.error) {
      console.log("Error data doesn't have valid format.");
      errorMessages.push('Server je trenutno nedostupan.');
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
