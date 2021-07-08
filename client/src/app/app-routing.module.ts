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
import { UserComponent } from './ui/user/user.component';
import { UserDataComponent } from './ui/user/user-data/user-data.component';
import { UserPasswordComponent } from './ui/user/user-password/user-password.component';
import { VendorGuard } from './vendor-guard.service';
import { CustomerGuard } from './customer-guard.service';
import { CartPreviewComponent } from './ui/main/cart-preview/cart-preview.component';
import { OrderPreviewComponent } from './ui/user/order-preview/order-preview.component';

const routes: Routes = [
  { path: 'login', redirectTo: 'login/sign-in' },
  { path: 'admin', redirectTo: 'admin/menues' },

  { path: 'login', component: AuthenticationComponent, children: [
    {path: 'sign-in', component: SignInComponent},
    {path: 'sign-up', component: SignUpComponent},
  ]},
  { path: '', component: MainComponent, children:[
    {path: 'home', component: HomeComponent},
    {path: 'restaurant/:id', component: ClientSearchComponent},
    {path: 'cart', component: CartPreviewComponent},
    {path: 'my-orders', component: OrderPreviewComponent},
    {path: 'user', canActivate: [CustomerGuard], component: UserComponent, children: [
      {path: 'details', component: UserDataComponent},
      {path: 'change-password', component: UserPasswordComponent}
    ]}
  ]},
  { path: 'admin',    component: AdminComponent, canActivate: [VendorGuard], children: [
    {path: 'menues',  component: MenuesComponent},
    {path: 'about',   component: AboutUsComponent},
    {path: 'orders',   component: AdminOrdersComponent},
    {path: 'gallery', component: GalleryComponent},
    {path: 'user', component: UserComponent, children: [
      {path: 'details', component: UserDataComponent},
      {path: 'change-password', component: UserPasswordComponent},
    ]}
  ]},
  { path: '**', redirectTo: 'home' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
