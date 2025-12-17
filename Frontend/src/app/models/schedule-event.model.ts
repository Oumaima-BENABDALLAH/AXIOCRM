export interface ScheduleEvent {
  id: number;
  title: string;
  start: string;
  end: string;
  color: string;
  description?: string;
  resourceId?: string;
  resource?: Resource;
}

export interface ScheduleEventDto {
  id: number;
  title: string;
  start: string;
  end: string;
  color: string;
  description?: string;
  resourceId?: string;
}

export interface Resource {
  id: string;
  name: string;
  events?: ScheduleEvent[];
}
