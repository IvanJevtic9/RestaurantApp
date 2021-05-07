import { Component, OnDestroy, OnInit } from '@angular/core';

@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  styleUrls: ['./authentication.component.css']
})
export class AuthenticationComponent implements OnInit, OnDestroy {

  constructor() { }

  ngOnInit(): void {
    console.log('Auth comp. je kreirana');
  }

  ngOnDestroy(){
    console.log('Auth comp. je izbrisana');
  }

}
