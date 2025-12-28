import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProductListComponent } from './components/product-list/product-list.component';
import { ClientListComponent } from './components/client-list/client-list.component';
import { OrderListComponent  } from './components/order-list/order-list.component';
import { InvoiceComponent } from './components/invoice/invoice.component';
import { DashboardComponent  } from './components/dashboard/dashboard.component';
import { LoginComponent } from './components/login/login.component';
import { SignupComponent } from './components/signup/signup.component';
import { ResetPasswordComponent  } from './components/reset-password/reset-password.component';
import { ForgotPasswordComponent   } from './components/forgot-password/forgot-password.component';
import {SchedulerComponent} from './components/scheduler/scheduler.component'
import { EmailHistoryComponent } from './components/email-history/email-history.component';
import { AppAuthGuard } from './auth-guard.guard';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
   { path: 'signup', component: SignupComponent },
   { path: 'reset-password', component: ResetPasswordComponent  },
   { path: 'forgot-password', component: ForgotPasswordComponent  },
  { path: 'product', component: ProductListComponent  },
  { path: 'client', component: ClientListComponent },
  { path: 'order', component: OrderListComponent },
  { path: 'invoice', component: InvoiceComponent },
  { path: 'dashboard', component: DashboardComponent }, 
  { path: 'scheduler', component: SchedulerComponent },
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'email-history', component: EmailHistoryComponent },
  { path: '**', redirectTo: '/login' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
