import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProductListComponent } from './components/product-list/product-list.component';
import { ClientListComponent } from './components/client-list/client-list.component';
import { OrderListComponent  } from './components/order-list/order-list.component';
import { DashboardHomeComponent  } from './components/dashboard-home/dashboard-home.component';
import { HomeComponent } from './home/home.component';



const routes: Routes = [

  { path: 'product', component: ProductListComponent },
  { path: 'client', component: ClientListComponent },
  { path: 'order', component: OrderListComponent },
  { path: 'home', component: HomeComponent },
  { path: 'dashboard', component: DashboardHomeComponent },
  { path: '', redirectTo: '/home', pathMatch: 'full' },

 
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
