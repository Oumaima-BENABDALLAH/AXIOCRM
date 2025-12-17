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
    // copie de l'objet pour éviter de modifier directement les données d'origine
    this.event = { ...data };
    this.eventService.getCommercials().subscribe(res => {
      this.resources = res;
    });
    this.dialogRef.updateSize('600px', 'auto');
  }

save() {
  this.dialogRef.close({
    id: this.event.id,               // ✅ OBJET MODIFIÉ
    title: this.event.title,
    start: this.event.start,
    end: this.event.end,
    color: this.event.color,
    description: this.event.description,
    resourceId: this.event.resourceId
  });
}

  cancel() {
    this.dialogRef.close(null);
  }
  close() {
  this.dialogRef.close(null);
}
}