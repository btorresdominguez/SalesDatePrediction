import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { OrdersViewComponent } from './orders-view/orders-view.component';
import { OrderUpdateComponent } from './order-update/order-update.component';

export const routes: Routes = [
    { path: '', component: AppComponent } ,// Ruta para el componente
    { path: '', redirectTo: '/sales-prediction', pathMatch: 'full' }, 
    { path: 'orders/:id', component: OrdersViewComponent },
    { path: 'order-update', component: OrderUpdateComponent },
  ];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
