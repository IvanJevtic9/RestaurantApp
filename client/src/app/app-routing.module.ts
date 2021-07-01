import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthenticationComponent } from './ui/authentication/authentication.component';
import { MainComponent } from './ui/main/main.component';
import { SignInComponent } from './ui/authentication/sign-in/sign-in.component';
import { SignUpComponent } from './ui/authentication/sign-up/sign-up.component';
import { HomeComponent } from './ui/main/home/home.component';
import { ClientSearchComponent } from './ui/main/client-search/client-search.component';
import { AdminComponent } from './ui/admin/admin.component';
import { MenuesComponent } from './ui/admin/menues/menues.component';
import { AboutUsComponent } from './ui/admin/about-us/about-us.component';
import { AdminOrdersComponent } from './ui/admin/admin-orders/admin-orders.component';
import { GalleryComponent } from './ui/admin/gallery/gallery.component';

const routes: Routes = [
  { path: 'login', redirectTo: 'login/sign-in' },
  { path: 'admin', redirectTo: 'admin/menues' },

  { path: 'login', component: AuthenticationComponent, children: [
    {path: 'sign-in', component: SignInComponent},
    {path: 'sign-up', component: SignUpComponent},
  ]},
  { path: '', component: MainComponent, children:[
    {path: 'home', component: HomeComponent},
    {path: 'restaurant/:id', component: ClientSearchComponent}
  ]},
  { path: 'admin',    component: AdminComponent, children: [
    {path: 'menues',  component: MenuesComponent},
    {path: 'about',   component: AboutUsComponent},
    {path: 'orders',   component: AdminOrdersComponent},
    {path: 'gallery', component: GalleryComponent}
  ]},
  { path: '**', redirectTo: 'home' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
