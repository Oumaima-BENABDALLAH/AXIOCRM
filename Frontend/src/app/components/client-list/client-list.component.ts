// src/app/components/client/client-list/client-list.component.ts
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
  // ✅ Data
  private clientsSubject = new BehaviorSubject<Client[]>([]);
  filteredClients$!: Observable<Client[]>;
  clients: Client[] = [];
  countries: Country[] = [];

  // ✅ Filters
  filter = new UntypedFormControl('');
  statusFilter = new UntypedFormControl('');
  designationFilter = new UntypedFormControl('');

  // ✅ UI & Form
  clientToDelete: Client | null = null;
  selectedClient: Client | null = null;
  clientForm!: UntypedFormGroup;
  avatarUrl: string | null = null;
  isSaving = false;
  isLoading = true;
  isViewMode = false;

  // Dropdown lists
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

    // Charger les clients
    this.clientService.getAll().subscribe({
      next: (clients) => {
        this.clients = clients;
        this.clientsSubject.next(clients);
        this.isLoading = false;
        console.log("✅ Clients loaded successfully:", clients);
      },
      error: (err) => {
        console.error("❌ Erreur lors du chargement des clients :", err);
        this.isLoading = false;
      }
    });

    // Charger la liste des pays
    this. loadCountries();

    // Initialisation
    this.initFilters();
    this.initForm();
  }

  loadCountries(): void {
       this.countryService.getCountries().subscribe({
      next: (countries) => {
        this.countries = countries;
        console.log("🌍 Countries loaded:", countries);
      },
      error: (err) => {
        console.error("❌ Erreur lors du chargement des pays :", err);
      }
    });
  }
 // ✅ Voir les détails d’un client (affiche le modal en mode lecture seule)
 
   onView(client: Client) {
    this.isViewMode = true;
     const formatDate = (dateString: any) => {
     if (!dateString) return null;
       const d = new Date(dateString);
       return d.toISOString().split('T')[0]; // ✅ garde seulement "YYYY-MM-DD"
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
  
    //  rendre le formulaire désactivé
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
      console.error('❌ Erreur lors du chargement des clients:', err);
      this.isLoading = false;
    }
  });
}


  // ✅ Observable combiné pour tous les filtres
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

  // ✅ Formulaire
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

  // ✅ CRUD
  onAdd() {
    this.clientForm.reset({ id: 0, phone: '' });
    this.avatarUrl = null;
    this.clientForm.patchValue({ profilePic: '' });
    this.openModal();
  }

onEdit(client: Client) {
  this.isViewMode = false;
  this.clientForm.enable();

  // ✅ Conversion correcte des dates avant injection dans le formulaire
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

  // Si le backend renvoie un objet
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
  
  if (isNaN(d.getTime())) return ''; // Sécurité en cas de date invalide
  
  const year = d.getFullYear();
  const month = String(d.getMonth() + 1).padStart(2, '0');
  const day = String(d.getDate()).padStart(2, '0');
  
  return `${year}-${month}-${day}`;
}

// save client

/* saveClient() {
  if (this.clientForm.invalid) {
    this.clientForm.markAllAsTouched();
    return;
  }

  this.isSaving = true;
  const clientData = this.clientForm.getRawValue();

  // --- normalize payload
 const payload: any = { ...clientData };
  if (payload.phone && typeof payload.phone === 'object') {
    payload.phone = payload.phone.phoneNumber || `${payload.phone.dialCode || ''}${payload.phone.phoneNumber || ''}`;
  }
  delete payload.fullName;
  if (payload.dateOfBirth) payload.dateOfBirth = new Date(payload.dateOfBirth).toISOString();
  if (payload.hireDate) payload.hireDate = new Date(payload.hireDate).toISOString();

  const request$ = payload.id === 0
    ? this.clientService.create(payload)
    : this.clientService.update(payload);

  request$.subscribe({
    next: (savedClient) => {
      this.isSaving = false;
      if (!savedClient || !savedClient.id) {
        console.warn('Backend did not return updated client, reloading list.');
        this.loadClient();
        this.closeModal();
        return;
      }
      const updatedClients = [...this.clients];
      if (payload.id === 0) {
        updatedClients.push(savedClient);
      } else {
        const index = updatedClients.findIndex(c => c.id === savedClient.id);
        if (index !== -1) updatedClients[index] = savedClient;
      }
      this.clients = updatedClients;
      this.clientsSubject.next(updatedClients);
      this.closeModal();
    },
    error: (err) => {
      console.error('❌ Erreur lors de la sauvegarde du client:', err);
      this.isSaving = false;

      if (!err.error) {
        alert('Erreur serveur : ' + (err.message || 'unknown'));
        return;
      }

      const modelErrors = err.error.errors;
      if (modelErrors) {
        console.group('🔎 Détails des erreurs de validation .NET');
        console.table(modelErrors);
        console.groupEnd();
        for (const field in modelErrors) {
          if (!modelErrors.hasOwnProperty(field)) continue;
          const key = field.split('.').pop()!;
          const control = this.clientForm.get(key);
          const messages = modelErrors[field] as string[];
          if (control) {
            control.setErrors({ server: messages.join(' | ') });
          } else {
            // message global
            console.warn(`Server validation for ${key}:`, messages);
          }
        }
        return;
      }

      alert('Erreur inconnue lors de la sauvegarde');
    }
  });
}*/
saveClient() {
  if (this.clientForm.invalid) {
    this.clientForm.markAllAsTouched();
    return;
  }

  this.isSaving = true;
  const clientData = this.clientForm.getRawValue();
  const payload: any = { ...clientData };

  delete payload.fullName;

  // 🔹 Convertir les dates en ISO
  if (payload.dateOfBirth) {
    payload.dateOfBirth = new Date(payload.dateOfBirth).toISOString();
  }
  if (payload.hireDate) {
    payload.hireDate = new Date(payload.hireDate).toISOString();
  }

  // 🔹 Nettoyer le champ téléphone
  if (payload.phone) {
    payload.phone = payload.phone.toString().replace(/\s+/g, ''); // supprime espaces éventuels
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
      console.error(' Erreur lors de la sauvegarde du client:', err);
    }
  });
}

private updateClientList(savedClient: any) {
  if (!savedClient) return;

  // Chercher l'index du client existant
  const index = this.clients.findIndex(c => c.id === savedClient.id);

  if (index >= 0) {
    // 🔹 Cas modification → remplacer l’ancien client
    this.clients[index] = { ...savedClient };
  } else {
    // 🔹 Cas création → ajouter le nouveau client
    this.clients.unshift(savedClient);
  }

  // 🔹 Notifier les abonnés (table, affichage, etc.)
  this.clientsSubject.next([...this.clients]);
}

  onDelete(client: Client) {
    if (confirm("Voulez-vous supprimer ce client ?")) {
      this.clientService.delete(client.id!).subscribe(() => {
        const updated = this.clients.filter(c => c.id !== client.id);
        this.clients = updated;
        this.clientsSubject.next(updated);
      });
    }
  }

  // ✅ Modal
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

  // ✅ Utils
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
