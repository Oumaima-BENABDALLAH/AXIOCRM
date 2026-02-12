import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { BookingTaskStatus, TaskPriority } from '../../models/booking-task.model';
import { ClientService, Client } from 'src/app/services/client.service';
import { AuthService} from 'src/app/services/auth.service';
import {HttpClient} from '@angular/common/http';
@Component({
  selector: 'app-add-task-dialog',
  templateUrl: './add-task-dialog.component.html'
})
export class AddTaskDialogComponent {

  title = '';
  description = '';
  dueDate?: string;
  priority = TaskPriority.Medium;
  clients: Client[] = [];
  users: Client[] = [];

  clientId!: number;
  commercials: any[] = [];
  commercialId?: string;
  status: BookingTaskStatus = BookingTaskStatus.Pending;
  TaskStatus = BookingTaskStatus;
  Priority = TaskPriority;

  constructor(
    public dialogRef: MatDialogRef<AddTaskDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { status: BookingTaskStatus , task?: any;isEdit?: boolean; }, private http: HttpClient ,private authService: AuthService) {
    this.loadClients();
    this.authService
    .getCommercials()
    .subscribe(data => (this.commercials = data));
    if (data?.isEdit && data.task) {
    this.title        = data.task.title;
    this.description  = data.task.description;
    this.dueDate      = data.task.dueDate? data.task.dueDate.substring(0, 10): undefined;
    this.priority     = data.task.priority;
    this.status       = data.task.status;
    this.clientId     = data.task.clientId;
    this.commercialId = data.task.commercialId;
  }
  else if (data?.status !== undefined) {
    this.status = data.status;
  }
  }
  save() {
  this.dialogRef.close({
    title: this.title,
    description: this.description,
    dueDate: this.dueDate,
    priority: this.priority,
    status: Number(this.status),
    clientId: this.clientId,
    commercialId: this.commercialId
  });
  }
 loadClients() {
  this.http.get<Client[]>('https://localhost:7063/api/client')
    .subscribe(x => this.clients = x);
}


  close() {
    this.dialogRef.close();
  }
}
