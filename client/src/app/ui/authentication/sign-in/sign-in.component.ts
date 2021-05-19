import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css']
})
export class SignInComponent implements OnInit {
  errorMessage: string;

  private static ERROR_MSG = 'Kombinacija korisničkog imena i lozinke nije ispravna!';

  constructor(private messageService: MessageService, private authService: AuthService) {}

  ngOnInit(): void {
  }

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
      data => {
        console.log(data);
      },
      error => {
        this.errorMessage = error[0];
      }
    );

  }

}
