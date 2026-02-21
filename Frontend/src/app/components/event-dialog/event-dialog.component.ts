import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ScheduleEventDto } from '../../models/schedule-event.model';
import { Resource } from '../../models/schedule-event.model';
import { EventService } from '../../services/Event.service';

@Component({
  selector: 'app-event-dialog',
  templateUrl: './event-dialog.component.html',
  styleUrls: ['./event-dialog.component.css']
})
export class EventDialogComponent {

  event: ScheduleEventDto;
  resources: Resource[] = [];

  constructor( private  eventService : EventService,
    public dialogRef: MatDialogRef<EventDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ScheduleEventDto
  ) {
  
    this.event = { ...data };
    this.eventService.getCommercials().subscribe(res => {
      this.resources = res;
    });
    this.dialogRef.updateSize('650px', 'auto');
  }

save() {
   if (this.event.AdminNotified && !this.event.lastAdminNotifiedAt) {
    this.event.lastAdminNotifiedAt = new Date().toISOString();
  }

  if (!this.event.AdminNotified) {
    this.event.lastAdminNotifiedAt = null;
  }
  this.dialogRef.close(this.event);
}

  cancel() {
    this.dialogRef.close(null);
  }
  close() {
  this.dialogRef.close(null);
}
get dialogTitle(): string {
  return this.event.id ? 'Edit Event' : 'Create Event';
}
}