import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';

import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatPaginatorModule, MatSortModule, MatTableModule } from '@angular/material';
import { CustomFormsModule } from 'ng2-validation';
import { CommonModule } from '@angular/common';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { CustomerComponent } from './components/customer/customer.component';
import { AlertComponent } from './shared/components/alert/alert.component';
import { AddEditCustomerDialogBoxComponent } from './components/customer/add-edit-customer-dialog-box/add-edit-customer-dialog-box.component';
import { CustomerService } from './shared/services/customer.service';
import { AlertService } from './shared/services/alert.service';

@NgModule({
  declarations: [
    AppComponent,
    NavBarComponent,
    CustomerComponent,   
    AddEditCustomerDialogBoxComponent,
    AlertComponent
  ],
  imports: [
    FormsModule,
    CustomFormsModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    NgbModule,
    AngularFontAwesomeModule,
    MatTableModule,
    BrowserAnimationsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    CommonModule,
    MatPaginatorModule,    
    MatSortModule
  ],
  entryComponents: [AddEditCustomerDialogBoxComponent],
  providers: [AlertService, CustomerService],
  bootstrap: [AppComponent]
})
export class AppModule { }
