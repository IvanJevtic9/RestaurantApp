import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { PasswordValidator } from './validators.validator';
import { AuthService } from '../../../services/auth.service';
import { RegistrationModel } from '../../../models/Registration.model';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent implements OnInit {

  signUpForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, Validators.minLength(6), PasswordValidator.strong] ),
    confirmPassword: new FormControl('', [Validators.required]),
    city: new FormControl('', Validators.required),
    address: new FormControl('', Validators.required),
    postalCode: new FormControl('', [Validators.required]),
    phone: new FormControl('', [Validators.required]),
    type: new FormControl(false, [Validators.required]),
    user: new FormGroup({
      firstName: new FormControl('', Validators.required),
      lastName: new FormControl('' , Validators.required),
      dateOfBirth: new FormControl('')
    }),
    restaurant: new FormGroup({
      name: new FormControl('', Validators.required),
      description: new FormControl('')
    })
  }
  );

  isUserType: boolean = true;

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
  }

  onSubmit(){
    let temp: RegistrationModel = {
      email: this.signUpForm.get("email").value,
      password: this.signUpForm.get("password").value,
      confirmPassword: this.signUpForm.get("confirmPassword").value,
      address: this.signUpForm.get("address").value,
      city: this.signUpForm.get("city").value,
      phone: this.signUpForm.get("phone").value,
      postalCode: this.signUpForm.get("postalCode").value,
      accountType: '' + this.signUpForm.get("type").value,
    };

    if(this.signUpForm.get("type").value){
      temp.restaurant = {
        name:  this.signUpForm.get("restaurant").get("name").value,
        description: this.signUpForm.get("restaurant").get("description").value
      }
    } else {
      temp.user = {
        firstName:  this.signUpForm.value.user.firstName,
        lastName:  this.signUpForm.value.user.lastName,
        dateOfBirth: this.signUpForm.value.user.dateOfBirth
      }
    }

    this.authService.register(temp);

  }

}
