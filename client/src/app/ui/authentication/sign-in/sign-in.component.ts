import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css']
})
export class SignInComponent {
  errorMessage: string;
  userSub: Subscription;

  private static ERROR_MSG = 'Kombinacija korisničkog imena i lozinke nije ispravna!';

  constructor(
    private messageService: MessageService,
    private authService: AuthService,
    private router: Router
  ) {}

  onSubmit(form: NgForm){
    if(form.value.username ===  '' || form.value.password ===  '' ){
      form.form.get('username').markAsTouched();
      form.form.get('password').markAsTouched();
      return;
    }

    this.authService.authenticate(
      form.value.username,
      form.value.password
    ).subscribe(
      isUserType => {
        if(isUserType){
          this.router.navigate(['home']);
        } else {
          this.router.navigate(['search']);
        }
      },
      error => {
        console.log(error);
        this.errorMessage = error[0];
      }
    );

  }

}
