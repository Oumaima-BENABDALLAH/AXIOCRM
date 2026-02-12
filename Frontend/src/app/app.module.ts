import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgApexchartsModule } from 'ng-apexcharts';
import { NgChartsModule } from 'ng2-charts';
import { FullCalendarModule } from '@fullcalendar/angular';
import { MatDialogModule } from '@angular/material/dialog';

import { AppRoutingModule } from './app-routing.module';
import { JwtInterceptor } from './AuthJwt/jwt.interceptor';
import { KeycloakService } from 'keycloak-angular';

import { AppComponent } from './app.component';
import { ProductListComponent } from './components/product-list/product-list.component';
import { ClientListComponent } from './components/client-list/client-list.component';
import { OrderListComponent } from './components/order-list/order-list.component';
import { InvoiceComponent } from './components/invoice/invoice.component';
import { ClientProductListComponent } from './components/client-product-list/client-product-list.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { ChartLineComponent } from './components/chart-line/chart-line.component';
import { ChartRadialComponent } from './components/chart-radial/chart-radial.component';
import { DashboardMetricCardComponent } from './components/dashboard-metric-card/dashboard-metric-card.component';
import { SparklineCardComponent } from './components/sparkline-card/sparkline-card.component';
import { AvailableBalanceCardComponent } from './components/available-balance-card/available-balance-card.component';
import { StatCardComponent } from './components/stat-card/stat-card.component';
import { HighlightCardComponent } from './components/highlight-card/highlight-card.component';
import { ChartsSectionComponent } from './components/charts-section/charts-section.component';
import { TotalProjectsCardComponent } from './components/total-projects-card/total-projects-card.component';
import { NotificationToastComponent } from './components/notification-toast/notification-toast.component';
import { LoginComponent } from './components/login/login.component';
import { SignupComponent } from './components/signup/signup.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { GoogleLoginComponent } from './components/google-login/google-login.component';
import { SchedulerComponent } from './components/scheduler/scheduler.component';
import { EventDialogComponent } from './components/event-dialog/event-dialog.component';
import { EmailHistoryComponent } from './components/email-history/email-history.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { BookingKanbanComponent } from './components/booking-kanban/booking-kanban.component';
import { BookingCardComponent } from './components/booking-card/booking-card.component';
import { AddTaskDialogComponent } from './components/add-task-dialog/add-task-dialog.component';
import { DragDropModule } from '@angular/cdk/drag-drop';
@NgModule({
  declarations: [
    AppComponent,
    ProductListComponent,
    ClientListComponent,
    OrderListComponent,
    InvoiceComponent,
    ClientProductListComponent,
    DashboardComponent,
    ChartLineComponent,
    ChartRadialComponent,
    DashboardMetricCardComponent,
    SparklineCardComponent,
    AvailableBalanceCardComponent,
    StatCardComponent,
    ChartsSectionComponent,
    NotificationToastComponent,
    LoginComponent,
    SignupComponent,
    ResetPasswordComponent,
    ForgotPasswordComponent,
    GoogleLoginComponent,SchedulerComponent, EventDialogComponent ,EmailHistoryComponent ,BookingKanbanComponent,BookingCardComponent,AddTaskDialogComponent   
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModule,
    BrowserAnimationsModule,
    NgApexchartsModule,
    NgChartsModule,
    FullCalendarModule,  TotalProjectsCardComponent, HighlightCardComponent,MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatOptionModule,
    MatButtonModule,MatIconModule,MatCheckboxModule,DragDropModule,MatDialogModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
