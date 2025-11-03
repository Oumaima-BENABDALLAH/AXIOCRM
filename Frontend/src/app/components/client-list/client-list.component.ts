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
    this.countryService.getCountries().subscribe({
      next: (countries) => {
        this.countries = countries;
        console.log("🌍 Countries loaded:", countries);
      },
      error: (err) => {
        console.error("❌ Erreur lors du chargement des pays :", err);
      }
    });

    // Initialisation
    this.initFilters();
    this.initForm();
  }
 // ✅ Voir les détails d’un client (affiche le modal en mode lecture seule)
  onView(client: Client): void {
    console.log('Voir client', client);
    // ici tu peux ouvrir ton modal en mode lecture seule
  }

toggleActions(event: Event) { 
  event.stopPropagation(); const target = event.currentTarget as HTMLElement; target.classList.toggle('active');
 }
loadClient(): void {
  this.isLoading = true;
  this.clientService.getAll().subscribe({
    next: (clients) => {
      this.clients = clients;
      this.isLoading = false;
      this.filter.setValue(this.filter.value); // ⚡ déclenche juste le recalcul
    },
    error: (err) => {
      console.error('❌ Erreur lors du rechargement des clients:', err);
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
      dateOfBirth: [null, [CustomValidators.notInFuture]],
      phone: ['', Validators.required],
      profilePic: [''],
      address: [''],
      city: [''],
      country: [''],
      province: [''],
      postalCode: [''],
      jobTitle: [''],
      hireDate: [null],
      workReferenceNumber: [''],
      occupationGroup: [''],
      department: [''],
      division: ['']
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
    this.clientForm.patchValue(client);
    this.avatarUrl = client.profilePic || null;
    this.openModal();
  }

  saveClient() {
    if (this.clientForm.invalid) {
      this.clientForm.markAllAsTouched();
      return;
    }

    this.isSaving = true;
    const clientData = this.clientForm.getRawValue() as Client;

    const request$ = clientData.id === 0
      ? this.clientService.create(clientData)
      : this.clientService.update(clientData);

    request$.subscribe({
      next: (savedClient) => {
        let updatedClients = [...this.clients];

        if (clientData.id === 0) {
          updatedClients.push(savedClient);
        } else {
          const index = updatedClients.findIndex(c => c.id === savedClient.id);
          if (index !== -1) updatedClients[index] = savedClient;
        }

        this.clients = updatedClients;
        this.clientsSubject.next(updatedClients); // ✅ met à jour instantanément la liste
        this.isSaving = false;
        this.closeModal();
      },
      error: (err) => {
      console.error('❌ Erreur lors de la sauvegarde du client:', err);

      // 🔍 Affiche les détails de validation renvoyés par .NET
      if (err.error?.errors) {
      console.group("🔎 Détails des erreurs de validation .NET");
      console.table(err.error.errors);
      console.groupEnd();
    } else {
    console.warn("⚠️ Pas de détails de validation dans la réponse :", err.error);
  }

  this.isSaving = false;
}
    });
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
