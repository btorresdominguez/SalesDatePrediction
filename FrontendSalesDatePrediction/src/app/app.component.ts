import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CustomerService } from './customer-service.service';
import { HttpClientModule } from '@angular/common/http';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { MatDialog } from '@angular/material/dialog';
import { OrdersViewComponent } from './orders-view/orders-view.component';
import { Router } from '@angular/router';
import { NgModule } from '@angular/core';
import { OrderUpdateComponent } from './order-update/order-update.component';
import { MatSelectModule } from '@angular/material/select';

interface CustomerOrderPrediction {
  customerId: number;
  customerName: string;
  lastOrderDate: Date;
  nextPredictedOrder: Date;
}

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    CommonModule,
    FormsModule,
    HttpClientModule,
    MatSelectModule,
 
    
  ],
  providers: [CustomerService],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'FrontendSalesDatePrediction';
  displayedColumns: string[] = ['customerName', 'lastOrderDate', 'nextPredictedOrder', 'actions'];
  dataSource = new MatTableDataSource<CustomerOrderPrediction>();

 
  // Order details management
  order = {
    customerId: 0,
    orderDetails: [] as any[], 
    detail: {} as any 
  };

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private customerService: CustomerService, private dialog: MatDialog, private router: Router) {}

  ngOnInit(): void {
 

    this.loadCustomerPredictions();  
   
  }
  loadCustomerPredictions(): void {
    this.customerService.getCustomerPredictions().subscribe(
      (data) => {
        if (Array.isArray(data)) {
          this.dataSource.data = data.map(prediction => ({
            ...prediction,
            lastOrderDate: new Date(prediction.lastOrderDate),
            nextPredictedOrder: new Date(prediction.nextPredictedOrder)
          }));
        } else {
          console.error('Expected an array, but received:', data);
        }
      },
      (error) => {
        console.error('Error fetching customer predictions:', error);
      }
    );
  }  
  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filterPredicate = (data: CustomerOrderPrediction, filter: string) => {
      return data.customerName.toLowerCase().includes(filter);
    };
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  viewOrders(customer: CustomerOrderPrediction): void {
    console.log('Customer passed to viewOrders:', customer);
    this.customerService.getClientOrders(customer.customerName).subscribe(
      (orders) => {
        console.log('Orders fetched:', orders); // Verify the orders
        this.dialog.open(OrdersViewComponent, {
          data: {
            customerName: customer.customerName,
            orders: orders,
          },
          width: '1000px',
        });
      },
      (error) => {
        console.error('Error fetching orders:', error);
      }
    );
  }

  // Method for creating an order using the modal
  newOrder(customer: string): void {
    this.order.customerId = Number(customer); // Set customer ID dynamically

    // Open the confirmation modal
    const dialogRef = this.dialog.open(OrderUpdateComponent, {
      data: {
        customerName: customer,
        //createOrder: this.createOrder.bind(this) // Pass the createOrder function
      },
      width: '1000px',
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        console.log('Order created successfully');
        this.router.navigate(['/orders']); // Navigate to orders view after creation
      }
    });
  }

 


  

 



}
