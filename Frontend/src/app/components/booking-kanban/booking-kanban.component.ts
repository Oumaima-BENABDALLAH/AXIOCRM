import { Component, OnInit } from '@angular/core';
import { BookingTaskService } from '../../services/booking-task.service';
import { BookingTask, BookingTaskStatus } from '../../models/booking-task.model';
import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { MatDialog } from '@angular/material/dialog';
import { AddTaskDialogComponent } from '../add-task-dialog/add-task-dialog.component';
import {HttpClient} from '@angular/common/http';
export interface AssignedUser {
  name: string;
  color: 'blue' | 'orange' | 'purple' | 'green';
}

@Component({
  selector: 'app-booking-kanban',
  templateUrl: './booking-kanban.component.html',
  styleUrls: ['./booking-kanban.component.css']
})

export class BookingKanbanComponent implements OnInit {

  tasks: BookingTask[] = [];
  Status = BookingTaskStatus;
  assignedUsers?: AssignedUser[];
  //  Filtres
  selectedCommercial: number | null = null;
  selectedDate: 'today' | 'week' | '' = '';
   headerUsers = [
    { id: 'u1', name: 'Emma Stone' },
    { id: 'u2', name: 'John Doe' },
    { id: 'u3', name: 'Alex Martin' }
  ];
  commercials = [
    { id: 1, name: 'Emma' },
    { id: 2, name: 'John' },
    { id: 3, name: 'Alex' }
  ];

  columns = [
    { status: BookingTaskStatus.Pending, label: 'Pending' },
    { status: BookingTaskStatus.Accepted, label: 'Accepted' },
    { status: BookingTaskStatus.InProgress, label: 'In Progress' },
    { status: BookingTaskStatus.WaitingClient, label: 'Waiting' },
    { status: BookingTaskStatus.Done, label: 'Done' },
    { status: BookingTaskStatus.Cancelled, label: 'Cancelled' }
  ];
  
  constructor(private service: BookingTaskService , private dialog: MatDialog , private http: HttpClient) {}

  ngOnInit(): void {
    this.load();
  }
  getInitials(name: string): string {
    return name
      .split(' ')
      .map(n => n[0])
      .join('')
      .substring(0, 2)
      .toUpperCase();
  }

  addUserGlobal() {
    console.log('Add global user to kanban');
  }
tasksByColumn: Record<BookingTaskStatus, BookingTask[]> = {
  0: [], 1: [], 2: [], 3: [], 4: [], 5: []
};
trackByTaskId(index: number, task: BookingTask): number {
  return task.id;
}
load() {
  this.service.getKanban().subscribe(tasks => {
    console.log('TASKS FROM API', tasks);

    this.resetColumns();

    tasks.forEach(task => {
      console.log('status:', task.status, typeof task.status);
      this.tasksByColumn[task.status as any]?.push(task);
    });
  });
}
   resetColumns() {
    Object.keys(this.tasksByColumn).forEach(
      k => this.tasksByColumn[k as any] = []
    );
  }

  tasksByStatus(status: BookingTaskStatus) {
    return this.tasks.filter(t => t.status === status);
  }

  drop(event: CdkDragDrop<BookingTask[]>, newStatus: BookingTaskStatus) {

    if (event.previousContainer === event.container) {
      moveItemInArray(
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );
    } else {
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );
      event.container.data[event.currentIndex].status = newStatus;
    }
    
  }
getPriorityLabel(priority: number): string {
  switch (priority) {
    case 0: return 'LOW';
    case 1: return 'MEDIUM';
    case 2: return 'HIGH';
    default: return '';
  }
}
getPriorityClass(priority: number): string {
  switch (priority) {
    case 0: return 'Low';
    case 1: return 'Medium';
    case 2: return 'High';
    default: return '';
  }
}
getAvatarColor(index: number): string {
  const colors = ['blue', 'orange', 'purple', 'green'];
  return colors[index % colors.length];
}
get connectedLists(): string[] {
  return this.columns.map(c => 'list-' + c.status);
}
  openAddTask(status: BookingTaskStatus) {

  const dialogRef = this.dialog.open(AddTaskDialogComponent, {
    width: '400px',
    data: { status }
  });

  dialogRef.afterClosed().subscribe(result => {
    if (!result) return;

   this.service.create(result).subscribe((createdTask: BookingTask) => {
     this.tasksByColumn[createdTask.status].push(createdTask);
    });
  });
}
openEditTask(task: BookingTask) {

  const dialogRef = this.dialog.open(AddTaskDialogComponent, {
    width: '400px',
    data: {
      task: task,
      isEdit: true
    }
  });

  dialogRef.afterClosed().subscribe(result => {

    if (!result) return;

    this.service.update(task.id, result).subscribe(() => {

      // = supprimer l’ancienne version
      this.removeTaskFromColumns(task.id);

      // = mettre à jour l’objet local
      const updatedTask: BookingTask = {
        ...task,
        ...result   // title, description, priority, status, etc.
      };

      // le remettre dans la bonne colonne
      this.tasksByColumn[updatedTask.status].push(updatedTask);

    });

  });
}
removeTaskFromColumns(taskId: number) {
  Object.keys(this.tasksByColumn).forEach(key => {
    this.tasksByColumn[key as any] =
      this.tasksByColumn[key as any].filter(t => t.id !== taskId);
  });
 }
deleteTask(task: BookingTask) {

  const confirmDelete = confirm(
    `Are you sure you want to delete "${task.title}" ?`
  );

  if (!confirmDelete) return;

  this.service.delete(task.id).subscribe({
    next: () => {
      this.removeTaskFromColumns(task.id);
      console.log('Task deleted');
    },
    error: err => {
      console.error('Delete failed', err);
    }
  });
}

}
