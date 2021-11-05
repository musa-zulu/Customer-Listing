import { Component, OnInit, ViewChild } from "@angular/core";
import { MatDialog, MatTable, MatTableDataSource } from "@angular/material";
import { Customer } from "src/app/shared/models/Customer";
import { AlertService } from "src/app/shared/services/alert.service";
import { CustomerService } from "src/app/shared/services/customer.service";
import { AddEditCustomerDialogBoxComponent } from "./add-edit-customer-dialog-box/add-edit-customer-dialog-box.component";


@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.css']
})
export class CustomerComponent implements OnInit {
  customers: Customer[];
  customer: Customer = new Customer();
  pageLength: number = 0;

  @ViewChild(MatTable, { static: true }) table: MatTable<any>;

  filteredCustomers: Customer[] = [];
  displayedColumns: string[] = [
    "firstName",
    "lastName",
    "email",
    "cellphone",
    "amountTotal",
    "action"
  ];
  tableDataResource = new MatTableDataSource<Customer>();
  customerDetails: string;

  constructor(
    private _customersService: CustomerService,
    public dialog: MatDialog,
    protected _alertService: AlertService
  ) {}

  ngOnInit() {
    this.getCustomers();
  }

  getCustomers() {
    this._customersService.getCustomers().subscribe((customers) => {
      this.customers = customers.data;
      console.log();
      this.pageLength = this.customers.length;
      this.onPageChanged(null);
    });
  }

  openDialog(action: any, customer) {
    customer.action = action;
    let width = "400px";
    let height = "620px"
    if (action === 'Delete') {
      width = "350px";
      height = "350px";
    }
    const dialogRef = this.dialog.open(AddEditCustomerDialogBoxComponent, {
      width: width,
      height: height,
      data: customer,
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result.event !== "Cancel") {
        let regex: RegExp = new RegExp(/^[0-9]+(\.[0-9]*){0,1}$/g);
        let isValid = (result.data.firstName !== "" && result.data.lastName !== ""
              && result.data.email !== "" );
        if (regex.test(result.data.cellphone) || result.data.cellphone === "" || result.data.cellphone === undefined) {
          if (action === "Add" && isValid) {
            this.addCustomer(result.data);
          } else if (action === "Update" && isValid) {
            this.updateCustomer(result.data);
          } else if (action === "Delete") {
            this.deleteCustomer(result.data);
          }
        } else {
          this._alertService.error("Invalid input!!!");
        }
      }
    });
  }

  async addCustomer(customer: Customer) {
    await this._customersService
      .addCustomer(customer)
      .then((result) => {
        this._alertService.success("Customer was saved successfully !!");
      })
      .catch((error) => {
        this._alertService.error("Data was not saved, please try again");
      });
    this.refreshTable();
  }

  async updateCustomer(customer: Customer) {
    this._customersService
      .updateCustomer(customer)
      .then((result) => {
        this._alertService.success("Customer was updated successfully !!");
      })
      .catch((error) => {
        this._alertService.error("Data was not updated, please try again");
      });
    this.refreshTable();
  }

  async deleteCustomer(customer: Customer) {
    this._customersService
      .deleteCustomer(customer)
      .then((result) => {
        this._alertService.success("Customer was deleted successfully !!");
      })
      .catch((error) => {
        this._alertService.error("Data was not deleted, please try again");
      });
    this.refreshTable();
  }

  refreshTable() {
    this.getCustomers();
  }

  private initializeTable(customers: Customer[]) {
    this.tableDataResource = new MatTableDataSource<Customer>(customers);
  }

  filter(query: any) {
    let searchTerm = query.target.value.toLocaleLowerCase();
    const filteredCustomers = searchTerm
      ? this.customers.filter((p) => p.firstName.toLowerCase().includes(searchTerm) 
      || p.lastName.toLowerCase().includes(searchTerm)
      || p.email.toLowerCase().includes(searchTerm))
      : this.customers;

    this.initializeTable(filteredCustomers);
  }

  onPageChanged(e: { pageIndex: number; pageSize: number }): void {
    let filteredCustomers = [];
    if (e == null) {
      let firstCut = 0;
      let secondCut = firstCut + 10;
      filteredCustomers = this.customers.slice(firstCut, secondCut);
    } else {
      let firstCut = e.pageIndex * e.pageSize;
      let secondCut = firstCut + e.pageSize;
      filteredCustomers = this.customers.slice(firstCut, secondCut);
    }
    this.initializeTable(filteredCustomers);
  }
}
