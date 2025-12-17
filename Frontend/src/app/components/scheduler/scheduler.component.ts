import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CalendarOptions } from '@fullcalendar/core';

import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';

import { EventService } from '../../services/Event.service';
import { ScheduleEventDto } from '../../models/schedule-event.model';
import { EventDialogComponent } from '../event-dialog/event-dialog.component';

@Component({
  selector: 'app-scheduler',
  templateUrl: './scheduler.component.html',
  styleUrls: ['./scheduler.component.css']
})
export class SchedulerComponent implements OnInit {

  calendarOptions: CalendarOptions = {
    plugins: [dayGridPlugin, interactionPlugin],
    initialView: 'dayGridMonth',
    editable: true,
    selectable: true,
    eventDisplay: 'block',
    eventClick: (info) => this.onEventClick(info),
    dateClick: (info) => this.onDateClick(info),
    eventContent: (arg) => this.renderEventContent(arg),
    events: []
  };

  constructor(
    private dialog: MatDialog,
    private eventService: EventService
  ) {}

  ngOnInit(): void {
    this.loadEvents();
  }

  // ==============================
  // LOAD EVENTS (SAFE)
  // ==============================
loadEvents() {
  this.eventService.getAll().subscribe(events => {
    this.calendarOptions = {
      ...this.calendarOptions,
      events: events.map(e => ({
        id: String(e.id),
        title: e.title,
        start: e.start,
        end: e.end,

        allDay: false, // ⭐ PROPRIÉTÉ FULLCALENDAR UNIQUEMENT

        backgroundColor: e.color,
        borderColor: e.color,
        extendedProps: {
          description: e.description,
          resourceId: e.resourceId
        }
      }))
    };
  });
}

  // ==============================
  // EVENT CLICK → UPDATE
  // ==============================
  onEventClick(info: any) {
    const event = info.event;

    const dialogRef = this.dialog.open(EventDialogComponent, {
      width: '600px',
      data: {
        id: Number(event.id), 
        title: event.title,
        start: event.start?.toISOString().slice(0, 16),
        end: event.end?.toISOString().slice(0, 16),
        color: event.backgroundColor,
        description: event.extendedProps?.description,
        resourceId: event.extendedProps?.resourceId
      } as ScheduleEventDto
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.updateEvent(result);
      }
    });
  }

  // ==============================
  // UPDATE EVENT
  // ==============================
updateEvent(event: ScheduleEventDto) {

  const payload: ScheduleEventDto = {
    ...event,
    start: new Date(event.start).toISOString(),
    end: new Date(event.end).toISOString()
  };

  this.eventService.update(payload).subscribe({
    next: () => this.loadEvents(),
    error: err => console.error('UPDATE FAILED', err)
  });
}


  // ==============================
  // DATE CLICK → CREATE
  // ==============================
  onDateClick(info: any) {
    const dialogRef = this.dialog.open(EventDialogComponent, {
      width: '650px',
      data: {
        id: 0,
        title: '',
        start: info.dateStr + 'T08:00',
        end: info.dateStr + 'T09:00',
        color: '#3788d8',
        description: '',
        resourceId: undefined
      } as ScheduleEventDto
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.createEvent(result);
      }
    });
  }

  // ==============================
  // CREATE EVENT
  // ==============================
  createEvent(event: ScheduleEventDto) {
    this.eventService.create(event).subscribe(() => {
      this.loadEvents(); //  reload
    });
  }

  // ==============================
  // CUSTOM RENDER
  // ==============================
  renderEventContent(arg: any) {
    const description = arg.event.extendedProps?.description;

    return {
      html: `
        <div class="fc-event-title">${arg.event.title}</div>
        ${description ? `<div class="fc-event-description">${description}</div>` : ''}
      `
    };
  }
}
