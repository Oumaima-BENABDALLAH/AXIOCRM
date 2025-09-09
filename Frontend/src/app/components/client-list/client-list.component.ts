// src/app/components/client/client-list/client-list.component.ts
import { Component, OnInit ,ElementRef } from '@angular/core';
import { ClientService, Client } from 'src/app/services/client.service';
import { UntypedFormControl , UntypedFormGroup, Validators, UntypedFormBuilder } from '@angular/forms';
import { Observable,  BehaviorSubject, combineLatest} from 'rxjs';
import { CustomValidators } from 'src/app/validators/custom-validators';
import { map, startWith } from 'rxjs/operators';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import * as bootstrap from 'bootstrap';
import { CountryService, Country } from "src/app/services/countryService"


@Component({
  selector: 'app-client-list',
  templateUrl: './client-list.component.html',
  styleUrls: ['./client-list.component.css']
})
export class ClientListComponent implements OnInit {
  clients: Client[] = [];
  clientToDelete: Client | null = null;
  filter = new UntypedFormControl('');
  statusFilter = new UntypedFormControl('');
  designationFilter = new UntypedFormControl('');
  filteredClients$!: Observable<Client[]>;
  selectedClient: Client | null = null;
  isLoading = true;
  clientForm!: UntypedFormGroup;
  avatarUrl: string | null = null;
  isSaving = false;
  countries: Country[] = [];
  statusList: string[] = ["Active", "Inactive", "Away", "Offline"];
  designationList: string[] = [
  "Manager",
  "Employee",
  "Intern",
  "Contractor",
  "Consultant"
];

departmentList: string[] = [
  "IT",
  "HR",
  "Finance",
  "Sales",
  "Marketing",
  "Operations"
];
  constructor(private clientService: ClientService , private sanitizer: DomSanitizer,private fb: UntypedFormBuilder , private el: ElementRef ,private countryService: CountryService ) {}
 
 
ngOnInit(): void {
  this.isLoading = true;

  // Charger les clients
  this.clientService.getAll().subscribe({
    next: (clients) => {
      this.clients = clients;
      this.isLoading = false;

      // Initialiser l'observable combiné
      this.initFilters();

      console.log("✅ Clients loaded successfully:", this.clients);
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
      console.log("🌍 Countries loaded:", this.countries);
    },
    error: (err) => {
      console.error("❌ Erreur lors du chargement des pays :", err);
    }
  });

  this.initForm();
}

// Observable combiné pour tous les filtres
initFilters() {
  this.filteredClients$ = combineLatest([
    this.filter.valueChanges.pipe(startWith('')),
    this.statusFilter.valueChanges.pipe(startWith('')),
    this.designationFilter.valueChanges.pipe(startWith(''))
  ]).pipe(
    map(([term, status, designation]) =>
      this.clients.filter(c =>
        (!term || c.name.toLowerCase().includes(term.toLowerCase()) || c.email.toLowerCase().includes(term.toLowerCase())) &&
        (!status || c.status === status) &&
        (!designation || c.designation === designation)
      )
    )
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
      dateOfBirth: [null, [CustomValidators.notInFuture]],
      phone: ['', Validators.required] ,
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
 loadClient(){
  this.clientService.getAll().subscribe(data => {
    this.clients = data; // ici data est toujours Client[]
    this.filteredClients$ = this.filter.valueChanges.pipe(
      startWith(''),
      map(term =>
        this.clients.filter(c =>
          c.name.toLowerCase().includes(term.toLowerCase()) ||
          c.email.toLowerCase().includes(term.toLowerCase()) 
          //c.phone.includes(term)
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
/*onAdd() {
    this.clientForm.reset({id :0, name :'', email:'', phone : ''})
    this.avatarUrl =  null;
    this.clientForm.patchValue({ profilePic: '' });
    const modalEl = document.getElementById('clientModal');
     if (!modalEl) return;
    const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
    modal.show();
}*/
onAdd() {
  this.clientForm.reset({ id: 0 , phone:  '' });
  this.avatarUrl = null;
  this.clientForm.patchValue({ profilePic: '' });
  this.openModal();
}
onEdit(client: Client) {
  this.clientForm.patchValue({
    id: client.id,
    name: client.name,
    lastName: client.lastName ?? '',
    fullName: client.fullName ?? '',
    email: client.email,
    designation: client.designation,
    status: client.status,
    profilePic: client.profilePic,
    dateOfBirth: client.dateOfBirth ?? '',
    address: client.address ?? '',
    city: client.city ?? '',
    province: client.province ?? '',
    postalCode: client.postalCode ?? '',
    country: client.country ?? '',
    jobTitle: client.jobTitle ?? '',
    hireDate: client.hireDate ?? null,
    workReferenceNumber: client.workReferenceNumber ?? '',
    occupationGroup: client.occupationGroup ?? '',
    department: client.department ?? '',
    division: client.division ?? '',
    phone: client.phone ?? ''
  });

  this.avatarUrl = client.profilePic || null;
  this.openModal();
}


  saveClient() {
  console.log('✅ Tentative de soumission du formulaire.');

  if (this.clientForm.invalid) {
    this.clientForm.markAllAsTouched();
   console.log('❌ Le formulaire est invalide. Voici le détail :');
   Object.keys(this.clientForm.controls).forEach(key => {
     const control = this.clientForm.get(key);
      console.log(`- ${key}:`, control.valid ? '✅ VALIDE' : '❌ INVALIDE', 'Erreurs:', control.errors);
   });
    return;
  }

  // 🔹 Récupérer les valeurs brutes du formulaire
  const formValue = this.clientForm.getRawValue();

  // 🔹 Mapper pour correspondre au modèle backend
  const clientDataBackend: Client = {
    id: formValue.id,
    name: formValue.name,
    lastName: formValue.lastName,
    fullName: formValue.fullName,
    email: formValue.email,
    phone: formValue.phone,
    designation: formValue.designation,
    status: formValue.status,
    profilePic: formValue.profilePic,
    dateOfBirth: formValue.dateOfBirth ? new Date(formValue.dateOfBirth) : null,
    address: formValue.address,
    city: formValue.city,
    province: formValue.province,
    postalCode: formValue.postalCode,
    country: formValue.country,
    department: formValue.department,
    jobTitle: formValue.jobTitle,
    hireDate: formValue.hireDate ? new Date(formValue.hireDate) : null,
    salary: formValue.salary ?? null,
    workReferenceNumber: formValue.workReferenceNumber,
    occupationGroup: formValue.occupationGroup,
    manager: formValue.manager,
    employmentType: formValue.employmentType,
    notes: formValue.notes,
    division: formValue.division,
    orders: [],
   
  };

  // 🔹 Ensuite, utiliser `clientDataBackend` pour créer ou mettre à jour
  if (clientDataBackend.id === 0) {
    this.clientService.create(clientDataBackend).subscribe({
      next: (newClient) => {
        this.clients.push(newClient);
        this.filter.setValue(this.filter.value);
        this.closeModal();
      }
    });
  } else {
    this.clientService.update(clientDataBackend).subscribe({
      next: () => {
        const index = this.clients.findIndex(c => c.id === clientDataBackend.id);
        if (index !== -1) {
          this.clients[index] = clientDataBackend;
          this.filter.setValue(this.filter.value);
        }
        this.closeModal();
      }
    });
  }
}

onDelete(client : Client) {
  if (client && confirm("Voulez-vous supprimer ce client ?")) {
    this.clientService.delete(client.id!).subscribe(() => {
        this.loadClient();
    });
  }
  
}
  setClientToDelete(client: Client) {
    this.clientToDelete = client;
    
  }

  confirmDelete() {
    if (this.clientToDelete?.id) {
      this.clientService.delete(this.clientToDelete.id).subscribe({
        next: () => {
          console.log("Suppression réussie :", this.clientToDelete);
          // ⚡ Retirer le client supprimé du tableau sans recharger toute la page
          this.clients = this.clients.filter(c => c.id !== this.clientToDelete?.id);
          this.clientToDelete = null;
        },
        error: (err) => {
          console.error('Erreur lors de la suppression', err);
        }
      });
    }
  }
openModal() {
const modalEl = document.getElementById('clientModal');
  if (!modalEl) return;

  const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
  modal.show();}

closeModal() {
 const modalEl = document.getElementById('clientModal');
  if (!modalEl) return;

  const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
  modal.hide();
}
onView(client: any) {
  console.log("Voir client :", client);
  // logique pour affichage des détails
}  
 toggleActions(event: Event) {
    event.stopPropagation();
    const target = event.currentTarget as HTMLElement;
    target.classList.toggle('active');
  }
   hideAllActionMenus(event: Event) {
    const allContainers = this.el.nativeElement.querySelectorAll('.actions-container');
    allContainers.forEach((container: HTMLElement) => {
      if (!container.contains(event.target as Node)) {
        container.classList.remove('active');
      }
    });
  }
  onFileSelected(event: Event) {
  const input = event.target as HTMLInputElement;
  if (input.files && input.files[0]) {
    const file = input.files[0];

    // ✅ Vérifier la taille (max 5MB)
    if (file.size > 5 * 1024 * 1024) {
      alert("File size must not exceed 5MB");
      return;
    }

    // ✅ Afficher l’image en preview
    const reader = new FileReader();
    reader.onload = e =>{
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
refreshPage() {
  this.loadClient();
}
formatPhone(phone: string): string {
  if (!phone) return '';
  return phone.startsWith('+') ? phone : '+216' + phone;
}
getFormValidationErrors() {
  Object.keys(this.clientForm.controls).forEach(key => {
    const controlErrors = this.clientForm.get(key)?.errors;
    if (controlErrors != null) {
      Object.keys(controlErrors).forEach(keyError => {
        console.error(
          `❌ Champ: ${key}, Erreur: ${keyError}, Valeur:`,
          this.clientForm.get(key)?.value
        );
      });
    }
  });
}
}
