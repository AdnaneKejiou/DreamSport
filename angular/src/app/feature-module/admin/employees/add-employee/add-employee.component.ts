import { Component, EventEmitter, Inject, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AddEmployee } from 'src/app/core/models/employee/addEmployee';

@Component({
  selector: 'app-add-employee',
  templateUrl: './add-employee.component.html',
  styleUrls: ['./add-employee.component.scss']
})
export class AddEmployeeComponent {
  @Output() onSubmitSuccess = new EventEmitter<any>();
  addForm: FormGroup;
  apiErrors: any = {};
  isSubmitting = false;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<AddEmployeeComponent>,
    private snackBar: MatSnackBar,
    @Inject(MAT_DIALOG_DATA) public data: AddEmployee // Use Employee interface
  ) {
    this.addForm = this.fb.group({
      prenom: ['', Validators.required],
      nom: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', Validators.required],
      cin: ['', Validators.required],
      username: ['', Validators.required],
      salaire: [null, [Validators.required, Validators.min(0)]],
      birthday: [null, Validators.required],
    });
  }

  
  onSubmit(): void {
    if (this.addForm.valid) {
      this.isSubmitting = true;
      this.apiErrors = {}; 
      const formValue = {
        ...this.addForm.value,
        birthday: new Date(this.addForm.value.birthday).toISOString()
      };
      this.onSubmitSuccess.emit(formValue);
    } else {
      // Mark all fields as touched to show validation messages
      Object.values(this.addForm.controls).forEach(control => {
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
      const control = this.addForm.get(key);
      if (control) {
        control.setErrors({ apiError: true });
        control.markAsTouched();
      }
    });
  }

  close(): void {
    this.dialogRef.close();
  }
}