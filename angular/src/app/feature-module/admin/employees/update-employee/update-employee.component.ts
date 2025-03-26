import { Component, EventEmitter, Inject, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { employee } from 'src/app/core/models/employee/employee';

@Component({
  selector: 'app-update-employee',
  templateUrl: './update-employee.component.html',
  styleUrls: ['./update-employee.component.scss']
})
export class UpdateEmployeeComponent {
  @Output() onSubmitSuccess = new EventEmitter<any>();
  updateForm: FormGroup;
  apiErrors: any = {};
  isSubmitting = false;

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<UpdateEmployeeComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { employee: employee }
  ) {
    this.updateForm = this.fb.group({
      nom: [data.employee.nom, Validators.required],
      prenom: [data.employee.prenom, Validators.required],
      email: [data.employee.email, [Validators.required, Validators.email]],
      phoneNumber: [data.employee.phoneNumber, Validators.required],
      salaire: [data.employee.salaire, [Validators.required, Validators.min(0)]],
      birthday: [new Date(data.employee.birthday), Validators.required]
    });
  }

  onSubmit(): void {
    if (this.updateForm.valid) {
      this.isSubmitting = true;
      this.apiErrors = {}; 
      
      const formValue = {
        ...this.updateForm.value,
        birthday: new Date(this.updateForm.value.birthday).toISOString()
      };
      
      // Emit the form data to parent instead of closing
      this.onSubmitSuccess.emit(formValue);
    } else {
      // Mark all fields as touched to show validation messages
      Object.values(this.updateForm.controls).forEach(control => {
        control.markAsTouched();
      });
    }
  }

  setErrors(errors: any): void {
    this.apiErrors = errors;
    this.isSubmitting = false;
    console.log("rooooooooooooooooooo",this.apiErrors);
    // Apply errors to form controls
    Object.keys(errors).forEach(key => {
      const control = this.updateForm.get(key);
      if (control) {
        control.setErrors({ apiError: true });
        control.markAsTouched();
      }
    });
  }

}