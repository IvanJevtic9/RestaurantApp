import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { CanActivate, RouterStateSnapshot, ActivatedRouteSnapshot, Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { AuthService } from './services/auth.service';

@Injectable({providedIn: 'root'})
export class CustomerGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot)
  : Observable<boolean> | Promise<boolean> | boolean {
    return this.authService.account.pipe(map(user => {
      if (!user) {
        this.router.navigate(['/home']);
        return false;
      }
      return user.isUserType;
    }));
  }

}
