import { AbstractControl, ValidationErrors } from '@angular/forms';

export class CustomValidators {
  static notInFuture(control: AbstractControl): ValidationErrors | null {
    const value = control.value;
    if (value && new Date(value) > new Date()) {
      return { futureDate: true }; 
    }
    return null; 
  }
}
