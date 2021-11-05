import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Customer } from '../models/Customer';
import { ServerConfig } from './server-config';
import { catchError, retry } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  private readonly _apiURL = "customers";
  constructor(private _http: HttpClient, private _serverConfig: ServerConfig) {}

  getCustomers(): Observable<any> {
    return this._http
      .get<Customer[]>(
        this._serverConfig.getBaseUrl() + this._apiURL,
        this._serverConfig.getRequestOptions()
      )
      .pipe(retry(1), catchError(this.handleError));
  }

  getCustomer(customerId: string): Observable<any> {
    return this._http
      .get<Customer>(
        this._serverConfig.getBaseUrl() + this._apiURL + "/" + customerId,         
        this._serverConfig.getRequestOptions()
      )
      .pipe(retry(1), catchError(this.handleError));
  }

  addCustomer(customer: Customer): Promise<any> {
    return this._http
      .post(
        this._serverConfig.getBaseUrl() + this._apiURL,
        customer,
        this._serverConfig.getRequestOptions()
      )
      .toPromise();
  }

  updateCustomer(customer: Customer) {
    return this._http
      .put(
        this._serverConfig.getBaseUrl() + this._apiURL + "/",
        customer,
        this._serverConfig.getRequestOptions()
      )
      .toPromise();
  }

  deleteCustomer(customer: Customer) {
    return this._http
      .delete<Customer>(
        this._serverConfig.getBaseUrl() +
          this._apiURL +
          "/" +
          customer.customerId,
        this._serverConfig.getRequestOptions()
      )
      .toPromise();
  }

  private handleError(error: any) {
    return Observable.throw(error);
  }
}
