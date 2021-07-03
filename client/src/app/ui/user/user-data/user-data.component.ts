import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { Account } from 'src/app/models/Account.model';
import { AuthService } from 'src/app/services/auth.service';
import { PasswordValidator } from '../../authentication/sign-up/validators.validator';

@Component({
  selector: 'app-user-data',
  templateUrl: './user-data.component.html',
  styleUrls: ['./user-data.component.css']
})
export class UserDataComponent implements OnInit {
  isAutenticated: Subscription;
  account: Account = null;
  phoneOldValue: string = '';
  errorMessage = null;
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

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    this.isAutenticated = this.authService.account.subscribe(account => {
      if (account === null || account === undefined) { return; }

      this.account = account;

      if(this.account.isUserType){
        this.signUpForm.get('restaurant').enable();
        this.signUpForm.get('user').disable();  
      } else {
        this.signUpForm.get('restaurant').disable();
        this.signUpForm.get('user').enable();
      }

      this.signUpForm.controls['email'].setValue("Should set to something");
      this.signUpForm.controls['email'].disable();

      // this.signUpForm.controls['address'].setValue(this.user.address);
      // this.signUpForm.controls['name'].setValue(this.user.name);
      // this.signUpForm.controls['surname'].setValue(this.user.surename);
      // this.signUpForm.controls['phone'].setValue(this.user.phone);
    });
  }

  onBasicUpload(event) {
    this.profile_image = event[0];
    console.log(this.profile_image);

  }

  ngOnDestroy() {
    this.isAutenticated.unsubscribe();
  }

  onPhoneInput(){
    const phoneValue = this.signUpForm.get('phone').value;

    if(/^\d*$/.test(phoneValue)){
      this.phoneOldValue = phoneValue;
    }
    else {
      this.signUpForm.get('phone').setValue(this.phoneOldValue);
    }
  }

  onSubmit() {
    if (!this.signUpForm.valid) { return; }

    let formData: {username? :string, name? :string, surename? :string, address? :string, phone? :string, } = {};

    if(this.signUpForm.value.address !== this.account.address)
      formData.address = this.signUpForm.value.address;

    // if(this.signUpForm.value.name !== this.account.name)
    //   formData.name = this.signUpForm.value.name;

    // if(this.signUpForm.value.surname !== this.account.surename)
    //   formData.surename = this.signUpForm.value.surname;

    if(this.signUpForm.value.phone !== this.account.phone)
      formData.phone = this.signUpForm.value.phone;

    // if(this.signUpForm.value.username !== this.account.username)
    //   formData.username = this.signUpForm.value.username;

    if(Object.keys(formData).length === 0) return;

    // this.authService.changeUserDetails(formData);
  }

}
