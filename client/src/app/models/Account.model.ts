import jwt_decode from "jwt-decode";

export enum AccountType{
  USER = 0,
  RESTAURANT = 1
}

export class Account{
  private id: string;
  private email: string
  private phone: string;
  private city:	string;
  private address: string;
  private postalCode:	string;
  private accountType: AccountType;
  private expDate: Date;

  private restaurant?: {
    name:	string,
    description?: string
  }
  private user?: {
    firstName: string,
    lastName:	string,
    dateOfBirth: Date
  }

  //Izvlacanje iz tokena potrebne informacije za Usera ili Preduzetnika
  constructor (private token: string){
    const tmp = jwt_decode<any>(token);

    this.email = tmp.email;
    this.phone = tmp.phone;
    this.city = tmp.city;
    this.address = tmp.address;
    this.postalCode = tmp.postalCode;
    this.accountType = (tmp.accountType === "User") ? AccountType.USER : AccountType.RESTAURANT;

    this.expDate = new Date(tmp.exp);
    this.id = tmp.id;

    if(this.accountType === AccountType.USER){
      this.user = {
        firstName : tmp.firstName,
        lastName : tmp.lastName,
        dateOfBirth : tmp.dateOfBirth
      }
    }else{
      this.restaurant = {
        name: tmp.name,
        description: tmp.description
      }
    }

  }


  public get getId() : string {
    return this.id;
  }

  public set setId(v : string) {
    this.id = v;
  }

  public get getPhone() : string {
    return this.phone;
  }

  public set setPhone(v : string) {
    this.phone = v;
  }

  public get getCity() : string {
    return this.city;
  }

  public set setCity(v : string) {
    this.city = v;
  }

  public get getAddress() : string {
    return this.address;
  }

  public set setAddress(v : string) {
    this.address = v;
  }

  public get getPostalCode() : string {
    return this.postalCode;
  }

  public set setPostalCode(v : string) {
    this.postalCode = v;
  }

  public get isUserType() : boolean {
    return this.accountType === AccountType.USER;
  }

  public get isTokenExpired() : boolean {
    return this.expDate >= new Date(Date.now());
  }

  public get getRestaurantData() : {
    name:	string,
    description?: string
  } {
      return this.restaurant;
  }

  public set setRestaurantData(data:{
    name:	string,
    description?: string
   } )  {
      this.restaurant = data;
  }

  public get getUserData() : {
    firstName: string,
    lastName:	string,
    dateOfBirth: Date
  }{
    return this.user;
  }

  public set setUserData(data : {
    firstName: string,
    lastName:	string,
    dateOfBirth: Date
  }){
    this.user = data;
  }
}
