import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProductListComponent } from './components/product-list/product-list.component';
import { ClientListComponent } from './components/client-list/client-list.component';
import { OrderListComponent  } from './components/order-list/order-list.component';
import { DashboardComponent  } from './components/dashboard/dashboard.component';
import { LoginComponent } from './components/login/login.component';
import { SignupComponent } from './components/signup/signup.component';
import { ResetPasswordComponent  } from './components/reset-password/reset-password.component';
import { ForgotPasswordComponent   } from './components/forgot-password/forgot-password.component';


const routes: Routes = [
  { path: 'login', component: LoginComponent },
   { path: 'signup', component: SignupComponent },
   { path: 'reset-password', component: ResetPasswordComponent },
   { path: 'forgot-password', component: ForgotPasswordComponent },
  { path: 'product', component: ProductListComponent },
  { path: 'client', component: ClientListComponent },
  { path: 'order', component: OrderListComponent },
  { path: 'dashboard', component: DashboardComponent },

  // ✅ Redirection par défaut vers /login
  { path: '', redirectTo: '/login', pathMatch: 'full' },

  // ✅ Pour gérer les chemins inconnus (404)
  { path: '**', redirectTo: '/login' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
