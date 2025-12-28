import { Component, OnInit } from '@angular/core';
import { EmailHistoryGroup} from '../../models/email-history.model';
import {EmailHistoryService} from '../../services/email-history.services';
@Component({
  selector: 'app-email-history',
  templateUrl: './email-history.component.html',
  styleUrl: './email-history.component.css'
})
export class EmailHistoryComponent implements OnInit {

  history: (EmailHistoryGroup & { expanded: boolean })[] = [];
  loading = false;
  error = '';

  constructor(private emailHistoryService: EmailHistoryService) {}


ngOnInit(): void {
    this.loadEmailHistory();
  }

  loadEmailHistory(): void {
    this.emailHistoryService.getEmailHistory().subscribe(data => {

     
      this.history = data.map(group => ({
        ...group,
        expanded: true 
      }));

      console.log(this.history);
    });
  }

  toggleGroup(group: any): void {
    group.expanded = !group.expanded;
  }
}