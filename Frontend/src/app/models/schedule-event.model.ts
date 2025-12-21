export interface ScheduleEvent {
  id: number;
  title: string;
  start: string;
  end: string;
  color: string;
  description?: string;
  resourceId?: string;
  resource?: Resource;
  reminderSent: boolean;
}

export interface ScheduleEventDto {
  id: number;
  title: string;
  start: string;
  end: string;
  color: string;
  description?: string;
  resourceId?: string;
  reminderSent: boolean;
}

export interface Resource {
  id: string;
  name: string;
  events?: ScheduleEvent[];
}
