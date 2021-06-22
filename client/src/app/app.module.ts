import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthenticationComponent } from './ui/authentication/authentication.component';
import { MainComponent } from './ui/main/main.component';
import { SignInComponent } from './ui/authentication/sign-in/sign-in.component';
import { SignUpComponent } from './ui/authentication/sign-up/sign-up.component';
import { SpinnerComponent } from './shared/components/spinner/spinner.component';
import { HeaderComponent } from './ui/main/header/header.component';
import { HomeComponent } from './ui/main/home/home.component';
import { ClientSearchComponent } from './ui/main/client-search/client-search.component';

// Primeface
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CheckboxModule } from 'primeng/checkbox';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { InputSwitchModule } from 'primeng/inputswitch';
import { CalendarModule } from 'primeng/calendar';
import { InputMaskModule } from 'primeng/inputmask';
import { AvatarModule } from 'primeng/avatar';
import { RippleModule } from 'primeng/ripple';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { MessagesModule } from 'primeng/messages';
import { MessageModule } from 'primeng/message';
import { MenuModule } from 'primeng/menu';
import { TabViewModule } from 'primeng/tabview';
import { DropdownModule } from 'primeng/dropdown';
import { DialogModule } from 'primeng/dialog';

import { AdminComponent } from './ui/admin/admin.component';
import { DishOverviewComponent } from './ui/admin/dish-overview/dish-overview.component';
import { DishEditComponent } from './ui/admin/dish-edit/dish-edit.component';
import { AboutUsComponent } from './ui/admin/about-us/about-us.component';
import { MenuesComponent } from './ui/admin/menues/menues.component';
import { MenuFormComponent } from './shared/menu-form/menu-form.component';
@NgModule({
  declarations: [
    AppComponent,
    AuthenticationComponent,
    MainComponent,
    SignInComponent,
    SignUpComponent,
    SpinnerComponent,
    HeaderComponent,
    HomeComponent,
    ClientSearchComponent,
    AdminComponent,
    DishOverviewComponent,
    DishEditComponent,
    AboutUsComponent,
    MenuesComponent,
    MenuFormComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CheckboxModule,
    ToastModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    FormsModule,
    InputSwitchModule,
    CalendarModule,
    HttpClientModule,
    InputMaskModule,
    AvatarModule,
    MenuModule,
    RippleModule,
    AutoCompleteModule,
    TabViewModule,
    DialogModule,
    MessagesModule,
    MessageModule,
    DropdownModule
  ],
  providers: [MessageService],
  bootstrap: [AppComponent]
})
export class AppModule { }
