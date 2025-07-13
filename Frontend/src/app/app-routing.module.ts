import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProductListComponent } from './components/product-list/product-list.component';
import { ClientListComponent } from './components/client-list/client-list.component';
import { OrderListComponent  } from './components/order-list/order-list.component';
import { DashboardComponent  } from './components/dashboard/dashboard.component';




const routes: Routes = [

  { path: 'product', component: ProductListComponent },
  { path: 'client', component: ClientListComponent },
  { path: 'order', component: OrderListComponent },
  { path: 'dashboard', component: DashboardComponent },
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },

 
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
