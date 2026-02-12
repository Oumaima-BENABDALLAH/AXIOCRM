import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BookingTask } from '../models/booking-task.model';
import { BookingTaskStatus ,PlanifyTaskDto } from '../models/booking-task.model';


@Injectable({ providedIn: 'root' })
export class BookingTaskService {

  private api = 'https://localhost:7063/api/booking-tasks';

  constructor(private http: HttpClient) {}

  getKanban() {
    return this.http.get<BookingTask[]>(`${this.api}/kanban`);
  }
delete(id: number) {
  return this.http.delete(`${this.api}/${id}`);
}
  create(dto: any) {
    return this.http.post<BookingTask>(this.api, dto);
  }

  updateStatus(id: number, status: BookingTaskStatus) {
    return this.http.put(`${this.api}/${id}/status`, status);
  }
 update(id: number, dto: any) {
  return this.http.put<void>(`${this.api}/${id}`, dto);
}
  planify(id: number, dto: PlanifyTaskDto) {
    return this.http.post(`${this.api}/${id}/planify`, dto);
  }
}
