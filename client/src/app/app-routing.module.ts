import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthenticationComponent } from './ui/authentication/authentication.component';
import { MainComponent } from './ui/main/main.component';
import { SignInComponent } from './ui/authentication/sign-in/sign-in.component';
import { SignUpComponent } from './ui/authentication/sign-up/sign-up.component';

const routes: Routes = [
  { path: 'login', component: AuthenticationComponent, children: [
    {path: 'sign-in', component: SignInComponent},
    {path: 'sign-up', component: SignUpComponent},
  ]},
  { path: 'home', component: MainComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
