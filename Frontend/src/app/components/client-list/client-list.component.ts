// src/app/components/client/client-list/client-list.component.ts
import { Component, OnInit } from '@angular/core';
import { ClientService, Client } from 'src/app/services/client.service';
import { UntypedFormControl , UntypedFormGroup, Validators, UntypedFormBuilder } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
declare var $: any;
@Component({
  selector: 'app-client-list',
  templateUrl: './client-list.component.html',
  styleUrls: ['./client-list.component.css']
})
export class ClientListComponent implements OnInit {
  clients: Client[] = [];
  filter = new UntypedFormControl('');
  filteredClients$!: Observable<Client[]>;
  selectedClient: Client | null = null;
  //editingClient: Client = { id: 0, name: '', email: '',phone :'' };
    clientForm!: UntypedFormGroup;

  constructor(private clientService: ClientService , private sanitizer: DomSanitizer,private fb: UntypedFormBuilder) {}

  ngOnInit(): void {
     this.loadClient();
     this.initForm();

  }
initForm() {
    this.clientForm = this.fb.group({
      id: [0],
      name: ['', Validators.required],
      designation: ['', Validators.required], 
      status: ['', Validators.required], 
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, Validators.pattern(/^\+?[0-9\s\-]{7,15}$/)]]
    });
  }
/*   loadClient(){
     this.clientService.getAll().subscribe(data => {
      this.clients = data;
      this.filteredClients$ = this.filter.valueChanges.pipe(
        startWith(''),
        map(term =>
          this.clients.filter(c =>
            c.name.toLowerCase().includes(term.toLowerCase()) ||
            c.email.toLowerCase().includes(term.toLowerCase()) ||
            c.phone.includes(term)
          )
        )
      );
    });
  } */
 loadClient(){
  this.clientService.getAll().subscribe(data => {
    this.clients = data; // ici data est toujours Client[]
    this.filteredClients$ = this.filter.valueChanges.pipe(
      startWith(''),
      map(term =>
        this.clients.filter(c =>
          c.name.toLowerCase().includes(term.toLowerCase()) ||
          c.email.toLowerCase().includes(term.toLowerCase()) ||
          c.phone.includes(term)
        )
      )
    );
  });
}

   highlight(text: string, search: string): SafeHtml {
      if (!search) {
        return text;
      }
        const re = new RegExp(search, 'gi');
        const result = text.replace(re, match => `<mark>${match}</mark>`);
        return this.sanitizer.bypassSecurityTrustHtml(result);
    }
    selectClient(client: Client) {
    this.selectedClient = client;
     } 
    onAdd() {
    this.clientForm.reset({id :0, name :'', email:'', phone : ''})
    $('#clientModal').modal('show'); 
}
  onEdit( client : Client) {
    this.clientForm.setValue ({
      id: client.id,
      name:client.name,
      email: client.email,
      phone: client.phone,
      designation: client.designation, 
      status: client.status 


    });
    $('#clientModal').modal('show'); 
  
}
  saveClient() {
    if (this.clientForm.invalid) {
      this.clientForm.markAllAsTouched();
      return;
    }

    const clientData = this.clientForm.value as Client;

    if (clientData.id === 0) {
      this.clientService.create(clientData).subscribe(newClient => {
        this.clients.push(newClient);
        this.filter.setValue(this.filter.value);
        $('#clientModal').modal('hide');
      });
    } else {
      this.clientService.update(clientData).subscribe(() => {
        const index = this.clients.findIndex(p => p.id === clientData.id);
        if (index !== -1) {
          this.clients[index] = clientData;
          this.filter.setValue(this.filter.value);
        }
        $('#clientModal').modal('hide');
      });
    }
  }
onDelete(client : Client) {
  if (client && confirm("Voulez-vous supprimer ce client ?")) {
    this.clientService.delete(client.id).subscribe(() => {
      this.clients = this.clients.filter(p => p !== client);
      this.filter.setValue(this.filter.value); // rafraîchir le filtre
    });
  }
  
}
closeModal() {
  $('#clientModal').modal('hide');
}
  
}
