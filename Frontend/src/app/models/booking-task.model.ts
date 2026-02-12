export enum BookingTaskStatus {
  Pending = 0,
  Accepted = 1,
  InProgress = 2,
  WaitingClient = 3,
  Done = 4,
  Cancelled = 5
}
export enum TaskPriority {
  Low = 0,
  Medium = 1,
  High = 2
}

export interface BookingTask {
  id: number;
  title: string;
  description?: string;

  status: BookingTaskStatus;

  clientId?: number;
  clientName?: string;

  commercialId: string;
  commercialName?: string;

  scheduleEventId?: number;

  createdAt: string;
  dueDate?: string;
  priority: TaskPriority;
}
export interface PlanifyTaskDto {
  start: string;
  end: string;
  color?: string;
}
export interface Client {
  id: number;
  name: string;
}

export interface User {
  id: string;
  fullName: string
}

