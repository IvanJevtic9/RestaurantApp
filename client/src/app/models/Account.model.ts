import jwt_decode from "jwt-decode";

export enum AccountType{
  USER = 0,
  RESTAURANT = 1
}

export class Account{
  public id: string;
  public email: string
  public phone: string;
  public city:	string;
  public address: string;
  public postalCode:	string;
  public restaurant?: {
    restaurantId: number,
    name:	string,
    description?: string
  }
  public user?: {
    userId: number,
    firstName: string,
    lastName:	string,
    dateOfBirth: Date
  }

  private accountType: AccountType;
  public expDate: Date;

  //Izvlacanje iz tokena potrebne informacije za Usera ili Preduzetnika
  constructor (public token: string){
    const tmp = jwt_decode<any>(token);

    this.email = tmp.email;
    this.phone = tmp.phone;
    this.city = tmp.city;
    this.address = tmp.address;
    this.postalCode = tmp.postalCode;
    this.accountType = (tmp.accountType === "User") ? AccountType.USER : AccountType.RESTAURANT;
    this.expDate = new Date(tmp.exp * 1000);

    this.id = tmp.id;

    if(this.accountType === AccountType.USER){
      this.user = {
        userId: +tmp.userId,
        firstName : tmp.firstName,
        lastName : tmp.lastName,
        dateOfBirth : tmp.dateOfBirth
      }
    }else{
      this.restaurant = {
        restaurantId: +tmp.restaurantId,
        name: tmp.name,
        description: tmp.description
      }
    }

  }

  public get isUserType() : boolean {
    return this.accountType === AccountType.USER;
  }

  public get isTokenExpired() : boolean {
    return this.expDate >= new Date(Date.now());
  }

}
