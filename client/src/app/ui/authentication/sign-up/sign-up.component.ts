import { Component, OnInit, ɵɵi18nAttributes } from '@angular/core';
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
  errorMessages: string[] = [];

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    this.signUpForm.get('restaurant').disable();
  }
  onChangeForm(e:{checked: boolean}){

    if (e.checked) {
      this.signUpForm.get('restaurant').enable();
      this.signUpForm.get('user').disable();
    } else {
      this.signUpForm.get('restaurant').disable();
      this.signUpForm.get('user').enable();
    }
  }

  onSubmit(){

    if(!this.signUpForm.valid){
      this.signUpForm.get("email").markAsTouched();
      this.signUpForm.get("password").markAsTouched();
      this.signUpForm.get("confirmPassword").markAsTouched();
      this.signUpForm.get("city").markAsTouched();
      this.signUpForm.get("postalCode").markAsTouched();
      this.signUpForm.get("phone").markAsTouched();
      this.signUpForm.get("address").markAsTouched();
      if(!this.signUpForm.value.type){
        this.signUpForm.get("user.firstName").markAsTouched();
        this.signUpForm.get("user.lastName").markAsTouched();
        this.signUpForm.get("user.dateOfBirth").markAsTouched();
      } else {
        this.signUpForm.get("restaurant.name").markAsTouched();
      }

      return;
    }

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
      temp.accountType = "Restaurant";
      temp.restaurant = {
        name:  this.signUpForm.get("restaurant").get("name").value,
        description: this.signUpForm.get("restaurant").get("description").value
      }
    } else {
      temp.accountType = "User";
      temp.user = {
        firstName:  this.signUpForm.value.user.firstName,
        lastName:  this.signUpForm.value.user.lastName,
        dateOfBirth: this.signUpForm.value.user.dateOfBirth
      }
    }

    this.authService.register(temp)
    .subscribe( response => {
      // TODO show registration is succesfull, and link to login page
    }, error => {
      console.log('error');
      this.errorMessages = error;
    });

  }

}
