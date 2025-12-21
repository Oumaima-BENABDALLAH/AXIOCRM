import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ScheduleEventDto } from '../models/schedule-event.model';
import { Resource } from '../models/schedule-event.model';
@Injectable({
  providedIn: 'root'
})
export class EventService {

  private apiUrl = 'https://localhost:7063/api/scheduler';

  constructor(private http: HttpClient) {}

  getAll(): Observable<ScheduleEventDto[]> {
    return this.http.get<ScheduleEventDto[]>(this.apiUrl);
  }

 create(event: ScheduleEventDto): Observable<ScheduleEventDto> {
  return this.http.post<ScheduleEventDto>(this.apiUrl, event);
}


update(event: ScheduleEventDto) {
  return this.http.put(
    `${this.apiUrl}/${event.id}`,
    event
  );
}
delete(id: number) {
  return this.http.delete(`${this.apiUrl}/${id}`);
}
  getCommercials(): Observable<Resource[]> {
  return this.http.get<Resource[]>(
    `${this.apiUrl}/resources/commercials`
  );
}
}
