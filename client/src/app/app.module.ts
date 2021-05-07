import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthenticationComponent } from './ui/authentication/authentication.component';
import { MainComponent } from './ui/main/main.component';
import { SignInComponent } from './ui/authentication/sign-in/sign-in.component';
import { SignUpComponent } from './ui/authentication/sign-up/sign-up.component';

// Primeface
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {CheckboxModule} from 'primeng/checkbox';
import {ToastModule} from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,
    AuthenticationComponent,
    MainComponent,
    SignInComponent,
    SignUpComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CheckboxModule,
    ToastModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    FormsModule,
  ],
  providers: [MessageService],
  bootstrap: [AppComponent]
})
export class AppModule { }
