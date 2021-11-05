import { Inject, Optional } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Customer } from 'src/app/shared/models/Customer';

export enum CustomerType {
  NEW_CUSTOMER="NEW CUSTOMER",
  LOYAL_CUSTOMER="LOYAL CUSTOMER", 
  DISCOUNT_CUSTOMER="DISCOUNT CUSTOMER", 
  POTENTIAL_CUSTOMER="POTENTIAL CUSTOMER"
  }

@Component({
  selector: 'app-add-edit-customer-dialog-box',
  templateUrl: './add-edit-customer-dialog-box.component.html',
  styleUrls: ['./add-edit-customer-dialog-box.component.css']
})
export class AddEditCustomerDialogBoxComponent {

  action: string;
  localData: any;
  customer: Customer = new Customer();
  customerType = CustomerType;
  enumKeys=[];

  constructor(    
    public dialogRef: MatDialogRef<AddEditCustomerDialogBoxComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: Customer
  ) {
    this.localData = { ...data };
    this.action = this.localData.action;
    this.enumKeys = Object.keys(this.customerType);
  }

  doAction() {
    this.dialogRef.close({ event: this.action, data: this.localData });
  }

  closeDialog() {
    this.dialogRef.close({ event: "Cancel" });
  }
}
