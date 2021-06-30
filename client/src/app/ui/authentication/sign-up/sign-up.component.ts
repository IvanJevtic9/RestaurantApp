import { Component, OnInit, ɵɵi18nAttributes } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { PasswordValidator } from './validators.validator';
import { AuthService } from '../../../services/auth.service';
import { RegistrationModel } from '../../../models/Registration.model';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent implements OnInit {

  profile_image: any;

  signUpForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, Validators.minLength(6), PasswordValidator.strong]),
    confirmPassword: new FormControl('', [Validators.required]),
    city: new FormControl('', Validators.required),
    address: new FormControl('', Validators.required),
    postalCode: new FormControl('', [Validators.required]),
    phone: new FormControl('', [Validators.required]),
    type: new FormControl(false, [Validators.required]),
    user: new FormGroup({
      firstName: new FormControl('', Validators.required),
      lastName: new FormControl('', Validators.required),
      dateOfBirth: new FormControl('')
    }),
    restaurant: new FormGroup({
      name: new FormControl('', Validators.required),
      description: new FormControl('')
    })
  });

  isLoading = false;
  isRegisterMode: boolean = true;
  isUserType: boolean = true;
  errorMessages: string[] = [];

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    this.signUpForm.get('restaurant').disable();
  }
  onChangeForm(e: { checked: boolean }) {

    if (e.checked) {
      this.signUpForm.get('restaurant').enable();
      this.signUpForm.get('user').disable();
    } else {
      this.signUpForm.get('restaurant').disable();
      this.signUpForm.get('user').enable();
    }
  }

  onBasicUpload(event){
    this.profile_image = event[0];
    console.log(this.profile_image);
    
  }

  onSubmit(){

    if (!this.signUpForm.valid) {
      this.signUpForm.get("email").markAsTouched();
      this.signUpForm.get("password").markAsTouched();
      this.signUpForm.get("confirmPassword").markAsTouched();
      this.signUpForm.get("city").markAsTouched();
      this.signUpForm.get("postalCode").markAsTouched();
      this.signUpForm.get("phone").markAsTouched();
      this.signUpForm.get("address").markAsTouched();
      if (!this.signUpForm.value.type) {
        this.signUpForm.get("user.firstName").markAsTouched();
        this.signUpForm.get("user.lastName").markAsTouched();
        this.signUpForm.get("user.dateOfBirth").markAsTouched();
      } else {
        this.signUpForm.get("restaurant.name").markAsTouched();
      }

      return;
    }

    this.isLoading = true;


    let formModel = new FormData();

    formModel.append("email", this.signUpForm.get("email").value);
    formModel.append("password", this.signUpForm.get("password").value,);
    formModel.append("confirmPassword", this.signUpForm.get("confirmPassword").value);
    formModel.append("address", this.signUpForm.get("address").value);
    formModel.append("city", this.signUpForm.get("city").value);
    formModel.append("phone", this.signUpForm.get("phone").value);
    formModel.append("postalCode", this.signUpForm.get("postalCode").value);

    if (this.signUpForm.get("type").value) {
      formModel.append("accountType", "Restaurant");
      formModel.append("restaurant.name", this.signUpForm.get("restaurant").get("name").value);
      formModel.append("restaurant.description", this.signUpForm.get("restaurant").get("description").value);
    } else {
      formModel.append("accountType", "User");
      formModel.append("user.firstName", this.signUpForm.value.user.firstName,);
      formModel.append("user.lastName", this.signUpForm.value.user.lastName);
      formModel.append("user.dateOfBirth", this.signUpForm.value.user.dateOfBirth);
    }

    if(this.profile_image !== undefined || this.profile_image !== null){
      console.log(this.profile_image);
      
      formModel.append("imageFile",this.profile_image);
    }

    this.authService.register(formModel)
      .pipe(tap(data => {
        this.isLoading = false;
      }))
      .subscribe(response => {
        this.isRegisterMode = false;
      }, error => {
        this.isLoading = false;
        this.errorMessages = error;
      });

  }

}
