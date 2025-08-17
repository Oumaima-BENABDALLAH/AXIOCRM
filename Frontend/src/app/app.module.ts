import { BrowserModule } from '@angular/platform-browser';
import { NgModule ,APP_INITIALIZER  } from '@angular/core';
import { NgApexchartsModule } from 'ng-apexcharts';
import { RouterModule, Routes } from '@angular/router';
import { StatCardComponent } from './components/stat-card/stat-card.component';
import { ChartLineComponent } from './components/chart-line/chart-line.component';
import { ChartRadialComponent } from './components/chart-radial/chart-radial.component';
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
import { DashboardMetricCardComponent } from './components/dashboard-metric-card/dashboard-metric-card.component';
import { SparklineCardComponent } from './components/sparkline-card/sparkline-card.component';
import {AvailableBalanceCardComponent} from './components/available-balance-card/available-balance-card.component'
import { HighlightCardComponent } from './components/highlight-card/highlight-card.component';
import { ChartsSectionComponent } from './components/charts-section/charts-section.component';
import { TotalProjectsCardComponent} from './components/total-projects-card/total-projects-card.component';
import { NotificationCardComponent} from './components/notification-toast/notification-toast.component';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { JwtInterceptor } from './AuthJwt/jwt.interceptor';
import { LoginComponent } from './components/login/login.component';
import { SignupComponent } from './components/signup/signup.component';
import { ResetPasswordComponent  } from './components/reset-password/reset-password.component';
import { ForgotPasswordComponent   } from './components/forgot-password/forgot-password.component';
import { GoogleLoginComponent   } from './components/google-login/google-login.component';
import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule } from '@angular/common/http';
import keycloakConfig from './keycloak.config';
import { KeycloakAngularModule, KeycloakService } from 'keycloak-angular';


import { NgChartsModule } from 'ng2-charts';

@NgModule({
  declarations: [
    AppComponent,
    ProductListComponent,
    ClientListComponent,
    OrderListComponent,
    ClientProductListComponent,
    DashboardComponent,
    ChartLineComponent,
    ChartRadialComponent,
    DashboardMetricCardComponent,
    SparklineCardComponent,
    ChartsSectionComponent,AvailableBalanceCardComponent ,StatCardComponent,NotificationCardComponent,LoginComponent,SignupComponent,ResetPasswordComponent,ForgotPasswordComponent,GoogleLoginComponent  ],
  imports: [
        HttpClientModule,
    AppRoutingModule,
    BrowserModule,
    NgApexchartsModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModule,
    BrowserAnimationsModule,
    HighlightCardComponent, NgChartsModule,TotalProjectsCardComponent/*, KeycloakAngularModule*/
  ],
   providers: [
   /* {
      provide: APP_INITIALIZER,
      useFactory: initializeKeycloak,
      multi: true,
      deps: [KeycloakService]
    },*/
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
