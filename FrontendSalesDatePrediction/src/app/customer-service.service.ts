import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface CustomerOrderPrediction {
  customerId: number;
  customerName: string;
  lastOrderDate: string;
  nextPredictedOrder: string;
}

export interface CustomerOrder {
  orderId: number;
  requiredDate: string;
  shippedDate: string;
  shipName: string;
  shipAddress: string;
  shipCity: string;
}

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  private baseUrl = 'https://localhost:7093/api/Customer';

  constructor(private http: HttpClient) {}

  getCustomerPredictions(): Observable<CustomerOrderPrediction[]> {
    return this.http.get<CustomerOrderPrediction[]>(`${this.baseUrl}/NextOrderPrediction`);
  }

  getClientOrders(customerName: string): Observable<CustomerOrder[]> {
    return this.http.get<CustomerOrder[]>(`${this.baseUrl}/GetClientOrders/${customerName}`);
  }

  createOrder(order: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/CreateOrder`, order);
  }

  getEmployees(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/GetEmployees`);
  }

  getShippers(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/GetShippers`);
  }

  getProducts(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/GetProducts`);
  }
}