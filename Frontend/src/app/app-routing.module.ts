import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProductListComponent } from './components/product-list/product-list.component';
import { ClientListComponent } from './components/client-list/client-list.component';
import { OrderListComponent  } from './components/order-list/order-list.component';
import { OrderCreateComponent } from './components/order-create/order-create.component';


const routes: Routes = [

  {path:'product', component : ProductListComponent},
  {path : '',redirectTo :'/product',pathMatch :'full'},
  {path:'client', component : ClientListComponent},
  {path : '',redirectTo :'/client',pathMatch :'full'},
  { path: 'order', component: OrderListComponent },
    {path : '',redirectTo :'/order',pathMatch :'full'},

  { path: 'order/create', component: OrderCreateComponent },
  { path: '', redirectTo: '/order', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
