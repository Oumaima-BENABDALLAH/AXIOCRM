
import { Component, OnInit, ElementRef } from '@angular/core';
import { ClientService, Client } from 'src/app/services/client.service';
import { UntypedFormControl, UntypedFormGroup, Validators, UntypedFormBuilder } from '@angular/forms';
import { Observable, BehaviorSubject, combineLatest } from 'rxjs';
import { CustomValidators } from 'src/app/validators/custom-validators';
import { map, startWith } from 'rxjs/operators';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import * as bootstrap from 'bootstrap';
import { CountryService, Country } from 'src/app/services/countryService';

declare var $: any;

@Component({
  selector: 'app-client-list',
  templateUrl: './client-list.component.html',
  styleUrls: ['./client-list.component.css']
})
export class ClientListComponent implements OnInit {
  private clientsSubject = new BehaviorSubject<Client[]>([]);
  filteredClients$!: Observable<Client[]>;
  clients: Client[] = [];
  countries: Country[] = [];
  filter = new UntypedFormControl('');
  statusFilter = new UntypedFormControl('');
  designationFilter = new UntypedFormControl('');
  clientToDelete: Client | null = null;
  selectedClient: Client | null = null;
  clientForm!: UntypedFormGroup;
  avatarUrl: string | null = null;
  isSaving = false;
  isLoading = true;
  isViewMode = false;
  statusList: string[] = ["Active", "Inactive", "Away", "Offline"];
  designationList: string[] = ["Manager", "Employee", "Intern", "Contractor", "Consultant"];
  departmentList: string[] = ["IT", "HR", "Finance", "Sales", "Marketing", "Operations"];

  constructor(
    private clientService: ClientService,
    private sanitizer: DomSanitizer,
    private fb: UntypedFormBuilder,
    private el: ElementRef,
    private countryService: CountryService
  ) {}

  ngOnInit(): void {
    this.isLoading = true;

    this.clientService.getAll().subscribe({
      next: (clients) => {
        this.clients = clients;
        this.clientsSubject.next(clients);
        this.isLoading = false;
        console.log("Clients loaded successfully:", clients);
      },
      error: (err) => {
        console.error("Error loading clients :", err);
        this.isLoading = false;
      }
    });

    this. loadCountries();
    this.initFilters();
    this.initForm();
  }

  loadCountries(): void {
       this.countryService.getCountries().subscribe({
      next: (countries) => {
        this.countries = countries;
        console.log("Countries loaded:", countries);
      },
      error: (err) => {
        console.error("Error loading countries :", err);
      }
    });
  }
 
   onView(client: Client) {
    this.isViewMode = true;
     const formatDate = (dateString: any) => {
     if (!dateString) return null;
       const d = new Date(dateString);
       return d.toISOString().split('T')[0]; 
     };
    this.clientForm.patchValue({
      id: client.id ?? 0,
      name: client.name,
      lastName: client.lastName,
      fullName: client.fullName ?? `${client.name} ${client.lastName}`,
      email:client.email,
      phone: client.phone,
      designation :client.designation,
      status:client.status,
      dateOfBirth: formatDate(client.dateOfBirth),
      profilePic:client.profilePic,   
      company: client.company,
      address: client.address,
      city: client.city,
      postalCode: client.postalCode,
      province: client.province,
      jobTitle: client.jobTitle,
      country: client.country,
      hireDate: formatDate(client.hireDate),
      workReferenceNumber: client.workReferenceNumber,
      occupationGroup: client.occupationGroup,
      department: client.department,
    });
    this.clientForm.disable();
    $('#clientModal').modal('show');
  }

toggleActions(event: Event) { 
  event.stopPropagation(); const target = event.currentTarget as HTMLElement; target.classList.toggle('active');
 }
loadClient() {
  this.isLoading = true;
  this.clientService.getAll().subscribe({
    next: (clients) => {
      this.clients = clients;
      this.clientsSubject.next(this.clients);
      this.isLoading = false;
    },
    error: (err) => {
      console.error('Error loading clients', err);
      this.isLoading = false;
    }
  });
}
  initFilters() {
    this.filteredClients$ = combineLatest([
      this.clientsSubject.asObservable(),
      this.filter.valueChanges.pipe(startWith('')),
      this.statusFilter.valueChanges.pipe(startWith('')),
      this.designationFilter.valueChanges.pipe(startWith(''))
    ]).pipe(
      map(([clients, term, status, designation]) => {
        const textTerm = (term || '').toLowerCase().trim();
        const statusTerm = status || '';
        const designationTerm = designation || '';

        return clients.filter(c => {
          const matchesText =
            c.name?.toLowerCase().includes(textTerm) ||
            c.email?.toLowerCase().includes(textTerm);
          const matchesStatus = !statusTerm || c.status === statusTerm;
          const matchesDesignation = !designationTerm || c.designation === designationTerm;
          return matchesText && matchesStatus && matchesDesignation;
        });
      })
    );
  }

  initForm() {
    this.clientForm = this.fb.group({
      id: [0],
      name: ['', [Validators.required, Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.maxLength(50)]],
      fullName: [{ value: '', disabled: true }],
      designation: ['', Validators.required],
      status: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      dateOfBirth: ['', [CustomValidators.notInFuture]],
      phone: ['', Validators.required],
      profilePic: [''],
      address: ['', Validators.required],
      city: ['',Validators.required],
      country: ['',Validators.required],
      province: ['',Validators.required],
      postalCode: ['',Validators.required],
      jobTitle: ['',Validators.required],
      hireDate: ['',Validators.required],
      workReferenceNumber: ['',Validators.required],
      occupationGroup: ['',Validators.required],
      department: ['',Validators.required],
      division: ['',Validators.required]
    });

    this.clientForm.get('name')?.valueChanges.subscribe(() => this.updateFullName());
    this.clientForm.get('lastName')?.valueChanges.subscribe(() => this.updateFullName());
  }

  updateFullName() {
    const name = this.clientForm.get('name')?.value || '';
    const lastName = this.clientForm.get('lastName')?.value || '';
    this.clientForm.get('fullName')?.setValue(`${name} ${lastName}`.trim(), { emitEvent: false });
  }

  onAdd() {
    this.clientForm.reset({ id: 0, phone: '' });
    this.avatarUrl = null;
    this.clientForm.patchValue({ profilePic: '' });
    this.openModal();
  }

onEdit(client: Client) {
  this.isViewMode = false;
  this.clientForm.enable();
  this.clientForm.patchValue({
    ...client,
    phone: client.phone || '',
    dateOfBirth: client.dateOfBirth ? this.formatDateForInput(client.dateOfBirth) : '',
    hireDate: client.hireDate ? this.formatDateForInput(client.hireDate) : ''
  });

  this.avatarUrl = client.profilePic || null;
  this.openModal();
}
private normalizePhone(phone: any): string {
  if (!phone) return '';
  if (typeof phone === 'string') return phone.trim();

  if (typeof phone === 'object') {
    const dial = phone.dialCode || '';
    const num = phone.phoneNumber || '';
    return `${dial}${num}`.trim();
  }
  return '';
}
private formatDateForInput(dateValue: string |Date): string {
  if (!dateValue) return '';
  
  const d = (typeof dateValue === 'string') ? new Date(dateValue) : dateValue;
  
  if (isNaN(d.getTime())) return ''; 
  
  const year = d.getFullYear();
  const month = String(d.getMonth() + 1).padStart(2, '0');
  const day = String(d.getDate()).padStart(2, '0');
  
  return `${year}-${month}-${day}`;
}
saveClient() {
  if (this.clientForm.invalid) {
    this.clientForm.markAllAsTouched();
    return;
  }

  this.isSaving = true;
  const clientData = this.clientForm.getRawValue();
  const payload: any = { ...clientData };

  delete payload.fullName;
  if (payload.dateOfBirth) {
    payload.dateOfBirth = new Date(payload.dateOfBirth).toISOString();
  }
  if (payload.hireDate) {
    payload.hireDate = new Date(payload.hireDate).toISOString();
  }
  if (payload.phone) {
    payload.phone = payload.phone.toString().replace(/\s+/g, ''); 
  }

  const request$ = payload.id === 0
    ? this.clientService.create(payload)
    : this.clientService.update(payload);

  request$.subscribe({
    next: (savedClient) => {
      this.isSaving = false;
      this.updateClientList(savedClient);
      this.closeModal();
    },
    error: (err) => {
      this.isSaving = false;
      console.error('Error while saving the client :', err);
    }
  });
}

private updateClientList(savedClient: any) {
  if (!savedClient) return;
  const index = this.clients.findIndex(c => c.id === savedClient.id);

  if (index >= 0) {
    this.clients[index] = { ...savedClient };
  } else {
    this.clients.unshift(savedClient);
  }
  this.clientsSubject.next([...this.clients]);
}

  onDelete(client: Client) {
    if (confirm("Do you want to delete this client ?")) {
      this.clientService.delete(client.id!).subscribe(() => {
        const updated = this.clients.filter(c => c.id !== client.id);
        this.clients = updated;
        this.clientsSubject.next(updated);
      });
    }
  }

  openModal() {
    const modalEl = document.getElementById('clientModal');
    if (!modalEl) return;
    const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
    modal.show();
  }

  closeModal() {
    const modalEl = document.getElementById('clientModal');
    if (!modalEl) return;
    const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
    modal.hide();
  }

  highlight(text: string, search: string): SafeHtml {
    if (!search) return text;
    const re = new RegExp(search, 'gi');
    const result = text.replace(re, match => `<mark>${match}</mark>`);
    return this.sanitizer.bypassSecurityTrustHtml(result);
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      const file = input.files[0];
      if (file.size > 5 * 1024 * 1024) {
        alert("File size must not exceed 5MB");
        return;
      }
      const reader = new FileReader();
      reader.onload = e => {
        this.avatarUrl = e.target?.result as string;
        this.clientForm.patchValue({ profilePic: this.avatarUrl });
      };
      reader.readAsDataURL(file);
    }
  }

  clearAvatar(fileInput: HTMLInputElement) {
    this.avatarUrl = null;
    this.clientForm.patchValue({ profilePic: '' });
    fileInput.value = "";
  }
}
