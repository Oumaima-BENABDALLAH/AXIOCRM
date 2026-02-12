import { Component, Input } from '@angular/core';
import { BookingTask } from '../../models/booking-task.model';

@Component({
  selector: 'app-booking-card',
  templateUrl: './booking-card.component.html',
  styleUrls: ['./booking-card.component.css']
})
export class BookingCardComponent {
  @Input() task!: BookingTask;
  getInitialsFromName(name: string): string {
  if (!name) return '';

  const parts = name.trim().split(' ');

  if (parts.length === 1) {
    return parts[0].charAt(0).toUpperCase();
  }

  return (
    parts[0].charAt(0).toUpperCase() +
    parts[1].charAt(0).toUpperCase()
  );
}
getAvatarColor(name: string): string {
  const colors = ['#6366f1', '#ec4899', '#22c55e', '#f59e0b'];
  return colors[name.length % colors.length];
}
}

