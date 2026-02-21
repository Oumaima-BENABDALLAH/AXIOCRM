import { Component, ViewChild, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { FullCalendarComponent } from '@fullcalendar/angular';
import { CalendarOptions } from '@fullcalendar/core';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import { EventService } from '../../services/Event.service';
import { ScheduleEventDto } from '../../models/schedule-event.model';
import { EventDialogComponent } from '../event-dialog/event-dialog.component';
import { NotificationService } from '../../services/notification.service';
@Component({
  selector: 'app-scheduler',
  templateUrl: './scheduler.component.html',
  styleUrls: ['./scheduler.component.css']
})
export class SchedulerComponent implements OnInit {

  @ViewChild('calendar') calendarComponent!: FullCalendarComponent;

  calendarOptions: CalendarOptions = {
    plugins: [dayGridPlugin, interactionPlugin],
    initialView: 'dayGridMonth',
    editable: true,
    selectable: true,
    eventDisplay: 'block',

    events: [],
  
    eventClick: undefined,
    dateClick: (info) => this.onDateClick(info),
    eventContent: (arg) => {
    return {
      html: `
        <div class="fc-event-custom">
          <span class="fc-title">${arg.event.title}</span>
          <div class="fc-actions">
            <i class="material-icons edit-icon" data-action="edit">edit</i>
            <i class="material-icons delete-icon" data-action="delete">delete</i>
          </div>
        </div>
      `
    };
  },

  eventDidMount: (info) => {
    const editBtn = info.el.querySelector('.edit-icon');
    const deleteBtn = info.el.querySelector('.delete-icon');

    editBtn?.addEventListener('click', (e) => {
      e.stopPropagation();
      this.openEditDialog(info.event);
    });

    deleteBtn?.addEventListener('click', (e) => {
      e.stopPropagation();
      this.confirmDelete(info.event);
    });
  }
  };

  constructor(
    private dialog: MatDialog,
    private eventService: EventService,
    private notificationService :NotificationService
  ) {}

  ngOnInit(): void {
    this.loadEvents();
  }
  openEditDialog(fcEvent: any) {
  const dialogRef = this.dialog.open(EventDialogComponent, {
    width: '650px',
    data: {
      id: Number(fcEvent.id),
      title: fcEvent.title,
      start: fcEvent.startStr.slice(0, 16),
      end: fcEvent.endStr?.slice(0, 16),
      color: fcEvent.backgroundColor,
      description: fcEvent.extendedProps?.description,
      resourceId: fcEvent.extendedProps?.resourceId,
      AdminNotified: fcEvent.extendedProps?.AdminNotified ?? false,
      lastAdminNotifiedAt: fcEvent.extendedProps?.lastAdminNotifiedAt ?? null
    }
  });

  dialogRef.afterClosed().subscribe(result => {
    if (!result) return;
    fcEvent.setProp('title', result.title);
    fcEvent.setStart(result.start);
    fcEvent.setEnd(result.end);
    fcEvent.setProp('backgroundColor', result.color);
    fcEvent.setProp('borderColor', result.color);
    fcEvent.setExtendedProp('description', result.description);
    fcEvent.setExtendedProp('resourceId', result.resourceId);
    fcEvent.setExtendedProp('AdminNotified', result.AdminNotified);
    fcEvent.setExtendedProp('lastAdminNotifiedAt', result.lastAdminNotifiedAt);
    this.eventService.update(result).subscribe();
  });
}
confirmDelete(fcEvent: any) {
  const confirmed = confirm('Do you really want to delete this event ?');

  if (!confirmed) return;

  this.eventService.delete(Number(fcEvent.id)).subscribe(() => {
    fcEvent.remove(); 
  });
}
  loadEvents() {
    this.eventService.getAll().subscribe(events => {
      const calendarApi = this.calendarComponent.getApi();
      calendarApi.removeAllEvents();

      events.forEach(e => {
        calendarApi.addEvent({
          id: String(e.id),
          title: e.title,
          start: e.start,
          end: e.end,
          backgroundColor: e.color,
          borderColor: e.color,
          extendedProps: {
            description: e.description,
            resourceId: e.resourceId,
            AdminNotified: e.AdminNotified,
            lastAdminNotifiedAt :e.lastAdminNotifiedAt
          }
        });
      });
    });
  }

  onEventClick(info: any) {
    const fcEvent = info.event;

    const dialogRef = this.dialog.open(EventDialogComponent, {
      width: '650px',
      data: {
        id: Number(fcEvent.id),
        title: fcEvent.title,
        start: fcEvent.startStr.slice(0, 16),
        end: fcEvent.endStr?.slice(0, 16),
        color: fcEvent.backgroundColor,
        description: fcEvent.extendedProps?.description,
        resourceId: fcEvent.extendedProps?.resourceId,
        AdminNotified: fcEvent.extendedProps?.AdminNotified ?? false,
        lastAdminNotifiedAt :fcEvent.extendedProps?.lastAdminNotifiedAt ?? null

      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (!result) return;
      fcEvent.setProp('title', result.title);
      fcEvent.setStart(result.start);
      fcEvent.setEnd(result.end);
      fcEvent.setProp('backgroundColor', result.color);
      fcEvent.setProp('borderColor', result.color);
      fcEvent.setExtendedProp('description', result.description);
      fcEvent.setExtendedProp('resourceId', result.resourceId);
      fcEvent.setExtendedProp('AdminNotified', result.AdminNotified); 
      fcEvent.setExtendedProp('lastAdminNotifiedAt', result.lastAdminNotifiedAt); 
      this.eventService.update(result).subscribe();
    });
  }

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
        resourceId: undefined,
        AdminNotified: false,
       lastAdminNotifiedAt: null,
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (!result) return;

      this.eventService.create(result).subscribe(created => {
        const calendarApi = this.calendarComponent.getApi();
        calendarApi.addEvent({
          id: String(created.id),
          title: created.title,
          start: created.start,
          end: created.end,
          backgroundColor: created.color,
          borderColor: created.color,
          extendedProps: {
            description: created.description,
            resourceId: created.resourceId
          }
        });
      });
    });
  }
}
