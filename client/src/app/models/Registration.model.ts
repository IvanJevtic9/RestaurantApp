export interface RegistrationModel{
  email:	string,
  password:	string
  confirmPassword: string,
  phone: string,
  city:	string,
  address: string,
  postalCode:	string,
  accountType: string,
  restaurant?: {
    name:	string,
    description: string
  },
  user?: {
    firstName: string,
    lastName:	string,
    dateOfBirth: Date
  }
}
