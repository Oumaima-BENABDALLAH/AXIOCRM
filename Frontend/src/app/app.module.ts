import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { NgApexchartsModule } from 'ng-apexcharts';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './app.component';
import { ProductListComponent } from './components/product-list/product-list.component';
import { ClientListComponent } from './components/client-list/client-list.component';
import { OrderListComponent } from './components/order-list/order-list.component';
import { ClientProductListComponent } from './components/client-product-list/client-product-list.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { DOCUMENT } from '@angular/common';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

const routes: Routes = [
  { path: '', component: DashboardComponent },
  { path: 'products', component: ProductListComponent },
  { path: 'clients', component: ClientListComponent },
  { path: 'orders', component: OrderListComponent },
  { path: 'client-products', component: ClientProductListComponent },
];

@NgModule({
  declarations: [
    AppComponent,
    ProductListComponent,
    ClientListComponent,
    OrderListComponent,
    ClientProductListComponent,
    DashboardComponent,
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot(routes),
    NgApexchartsModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModule,
    BrowserAnimationsModule,
  ],
  providers: [
    provideHttpClient(withInterceptorsFromDi()),
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }
