import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { CustomerService } from '../customer-service.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatDialogModule } from '@angular/material/dialog';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { trigger, state, style, animate, transition } from '@angular/animations';

interface Employee {
  empid: number;
  fullName: string;
}
interface Shippers {
  shipperId: number;
  companyName: string;
}
interface Products {
  productid: number;
  productname: string;
}

@Component({
  selector: 'app-order-update',
  templateUrl: './order-update.component.html',
  styleUrls: ['./order-update.component.css'],
  standalone: true,
  imports: [
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatTableModule,
    CommonModule,
    FormsModule,
    MatPaginatorModule,
    MatDialogModule,
    HttpClientModule,
  ],
  providers: [CustomerService],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ])
  ]
})
export class OrderUpdateComponent implements OnInit {
  order: any = {
    empid: 0,
    shipperId: 0,
    shipname: '',
    shipaddress: '',
    shipcity: '',
    shipcountry: '',
    orderdate: new Date().toISOString(), // Set default to now
    requireddate: new Date().toISOString(), // Set default to now
    shippeddate: new Date().toISOString(), // Set default to now
    freight: 0,
    orderDetails: []
  };
  customerName: string;
  employees: Employee[] = []; 
  shippers: Shippers[] = [];
  products: Products[] = [];

  displayedColumns: string[] = ['productid', 'unitprice', 'qty', 'discount', 'actions'];
  dataSource = new MatTableDataSource<any>();

  detailsChanged: boolean = false;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  addDetail(): void {
    this.order.orderDetails.push({
      productid: null,
      unitprice: 0,
      qty: 1,
      discount: 0
    });
    this.dataSource.data = [...this.order.orderDetails]; // Actualiza el datasource
  }
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { customerName: string; orders: any[] },
    public dialogRef: MatDialogRef<OrderUpdateComponent>,
    private customerService: CustomerService
  ) {
    console.log('Datos recibidos en el modal:', this.data);
    this.customerName = this.data.customerName;

    if (this.data.orders && this.data.orders.length > 0) {
      this.order = { ...this.data.orders[0] };
      this.order.orderDetails = this.order.orderDetails || []; // Adjust this to match the expected format
    }
  }

  ngOnInit(): void {
    this.dataSource.data = this.order.orderDetails; // Bind to orderDetails
    this.loadEmployees();
    this.loadShippers();
    this.loadProducts();

    // Add a default detail if none exists
    if (this.order.orderDetails.length === 0) {
      this.addDefaultDetail();
    }
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  onDetailChange() {
    this.detailsChanged = true;
  }

  onClose(): void {
    this.dialogRef.close();
  }

  onSave(): void {
    this.createOrder(); // Call the create order method
  }

  onSubmit(): void {
    console.log('Pedido actualizado:', this.order);
  }

  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  // Method to add a default detail
  addDefaultDetail(): void {
    this.order.orderDetails.push({
      productid: 0,
      unitprice: 0,
      qty: 1, // Default quantity set to 1
      discount: 0
    });
    this.dataSource.data = [...this.order.orderDetails];
  }

  // Load Employees
  loadEmployees(): void {
    this.customerService.getEmployees().subscribe(
      (data: Employee[]) => {
        this.employees = data;
        console.log('Employees:', this.employees);
      },
      (error: any) => console.error('Error fetching employees:', error)
    );
  }

  // Load Shippers
  loadShippers(): void {
    this.customerService.getShippers().subscribe(
      (data: Shippers[]) => {
        this.shippers = data;
        console.log('Shippers:', this.shippers);
      },
      (error) => console.error('Error fetching shippers:', error)
    );
  }

  // Load Products
  loadProducts(): void {
    this.customerService.getProducts().subscribe(
      (data: Products[]) => {
        this.products = data;
        console.log('Products:', this.products);
      },
      (error) => console.error('Error fetching products:', error)
    );
  }

  // Function to create the order
  createOrder(): void {
    // Check if the selected empid is valid
    const selectedEmployee = this.employees.find(employee => employee.empid === this.order.empid);
  
  if (!selectedEmployee) {
    // Show the empid of the selected employee in the alert message
    alert(`Selected employee with empid ${this.order.empid} is not valid.`);
    return; // Exit if the employee is not valid
  }
  
    // Ensure the order is structured correctly
    const orderToCreate = {
      empid: this.order.empid,
      shipperId: this.order.shipperId,
      shipname: this.order.shipname,
      shipaddress: this.order.shipaddress,
      shipcity: this.order.shipcity,
      shipcountry: this.order.shipcountry,
      orderdate: this.order.orderdate,
      requireddate: this.order.requireddate,
      shippeddate: this.order.shippeddate,
      freight: this.order.freight,
      orderDetails: this.order.orderDetails // Use the correct property
    };
  
    this.customerService.createOrder(orderToCreate).subscribe(
      response => {
        console.log('Order created successfully', response);
        this.dialogRef.close(); // Close the dialog after successful creation
      },
      error => {
        console.error('Error creating order', error);
      }
    );
  }
  
}