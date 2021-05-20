import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthenticationComponent } from './ui/authentication/authentication.component';
import { MainComponent } from './ui/main/main.component';
import { SignInComponent } from './ui/authentication/sign-in/sign-in.component';
import { SignUpComponent } from './ui/authentication/sign-up/sign-up.component';
import { HomeComponent } from './ui/main/home/home.component';
import { ClientSearchComponent } from './ui/main/client-search/client-search.component';

const routes: Routes = [
  { path: 'login', redirectTo: 'login/sign-in' },

  { path: 'login', component: AuthenticationComponent, children: [
    {path: 'sign-in', component: SignInComponent},
    {path: 'sign-up', component: SignUpComponent},
  ]},
  { path: '', component: MainComponent, children:[
    {path: 'home', component: HomeComponent},
    {path: 'search', component: ClientSearchComponent}
  ]},
  { path: '**', redirectTo: 'home' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
