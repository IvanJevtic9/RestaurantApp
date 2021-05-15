import { Injectable } from '@angular/core';

@Injectable({providedIn: 'root'})
export class Config {
  location = 'http://localhost:44367';
  // API
  REGISTER_API = '/api/account/register';
}
