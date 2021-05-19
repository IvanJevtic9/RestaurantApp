import { Injectable } from '@angular/core';

@Injectable({providedIn: 'root'})
export class Config {
  private location = 'https://localhost:44367';
  // API
  REGISTER_API = this.location + '/api/account/register';
  LOGIN_API = this.location + '/api/account/login';
}
