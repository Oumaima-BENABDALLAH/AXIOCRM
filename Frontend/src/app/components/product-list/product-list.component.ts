import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ProductService } from 'src/app/services/product.service';
import { UntypedFormControl, UntypedFormGroup, Validators, UntypedFormBuilder,FormArray } from '@angular/forms';
import { Observable,BehaviorSubject,combineLatest   } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { ProductDto  } from 'src/app/models/product.model';
import { debounceTime } from 'rxjs/operators';
import { finalize } from 'rxjs/operators';


declare var $: any;

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  @ViewChild('fileInput') fileInput!: ElementRef;
  uploadProgress: number = 0;  
   private productsSubject = new BehaviorSubject<ProductDto[]>([]);
  products$ = this.productsSubject.asObservable();
  filter = new UntypedFormControl('');
  statusFilter = new UntypedFormControl('');   
  filteredProducts$!: Observable<ProductDto[]>;
  selectedProduct: ProductDto | null = null;

  productForm!: UntypedFormGroup;
  statusList: string[] = ['In Stock', 'Out of Stock', 'Restock'];
  isSaving = false;
  isViewMode = false;
  dropdownOpen = false;
  isFileSelected = false;
  imagePreview: string | ArrayBuffer | null = null;
  selectedColor: { name: string; value: string } | null = null;
  colorList = [
  { name: 'Black', value: '#000000' },
  { name: 'White', value: '#FFFFFF' },
  { name: 'Red', value: '#FF0000' },
  { name: 'Green', value: '#008000' },
  { name: 'Blue', value: '#0000FF' },
  { name: 'Yellow', value: '#FFFF00' },
  { name: 'Orange', value: '#FFA500' },
  { name: 'Pink', value: '#FFC0CB' },
  { name: 'Gray', value: '#808080' },
  { name: 'Brown', value: '#A52A2A' },
  { name: 'Purple', value: '#800080' },
  { name: 'Cyan', value: '#00FFFF' },
  { name: 'Beige', value: '#F5F5DC' },
  { name: 'Gold', value: '#FFD700' },
  { name: 'Silver', value: '#C0C0C0' },
];

  activeActionRow: number | null = null;

  constructor(
    private productService: ProductService,
    private sanitizer: DomSanitizer ,private fb: UntypedFormBuilder 
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.loadProducts();
    
    this.productForm.get('price')!.valueChanges.subscribe(() => this.updateRevenue());
    this.productForm.get('sales')!.valueChanges.subscribe(() => this.updateRevenue());
    this.filteredProducts$ = combineLatest([
      this.products$,
      this.filter.valueChanges.pipe(startWith(''), debounceTime(200)),
      this.statusFilter.valueChanges.pipe(startWith(''))
    ]).pipe(
      map(([products, term, status]) => {
        const textTerm = term?.toLowerCase() || '';
        const statusTerm = status || '';

        return products.filter((p) => {
          const matchesText =
            p.name.toLowerCase().includes(textTerm) ||
            p.price.toString().includes(textTerm);

          const matchesStatus = !statusTerm || p.status === statusTerm;

          return matchesText && matchesStatus;
        });
      })
    );
  }
trackById(index: number, item: ProductDto): number {
  return item.id;
}
   loadProducts() {
    this.productService.getAll().subscribe((products) => {
      this.productsSubject.next(products);
    });
  }

 private updateRevenue() {
    const price = Number(this.productForm.get('price')!.value) || 0;
    const sales = Number(this.productForm.get('sales')!.value) || 0;
    const rev = price * sales;
    this.productForm.get('revenue')!.setValue(rev, { emitEvent: false });
  }
  highlight(text: string, search: string): SafeHtml {
    if (!search) {
      return text;
    }
    const re = new RegExp(search, 'gi');
    const result = text.replace(re, (match) => `<mark>${match}</mark>`);
    return this.sanitizer.bypassSecurityTrustHtml(result);
  }

  
 onFileSelected(event: Event) {
  const file = (event.target as HTMLInputElement).files?.[0];
  if (!file) {
    this.isFileSelected = false;
    this.imagePreview = null;
    return;
  }

  this.isFileSelected = true;

  const reader = new FileReader();
  reader.onload = () => {
    this.imagePreview = reader.result;
    this.productForm.patchValue({ imageUrl: this.imagePreview });
    this.productForm.get('imageUrl')?.updateValueAndValidity();
    this.uploadProgress = 0;
    const interval = setInterval(() => {
      this.uploadProgress += 10;

      if (this.uploadProgress >= 100) {
        clearInterval(interval);
      }
    }, 120);
  };

  reader.readAsDataURL(file);
}
triggerFileInput() {
  (this.fileInput.nativeElement as HTMLInputElement).click();
}
  selectProduct(product: ProductDto) {
    this.selectedProduct = product;
  }

 onAdd() {
    this.productForm.reset({
      id: 0,
      name: '',
      description: '',
      price: 0,
      stockQuantity: 0,
      sales: 0,
      revenue: 0,
      status: 'Out of Stock',
      imageUrl: '',  
      color:  '#000000'
    });
    $('#productModal').modal('show');
  }

 onEdit(product: ProductDto) {
  this.isViewMode = false;
  this.productForm.enable();
  this.selectedProduct = product;
  this.imagePreview = product.imageUrl || null;
  this.selectedColor = this.colorList.find(c => c.value === product.color) || null;
  this.productForm.patchValue({
    id: product.id ?? 0,
    name: product.name,
    description: product.description,
    price: product.price,
    stockQuantity: product.stockQuantity,
    sales: product.sales,
    status: product.status,
    imageUrl: product.imageUrl || '',
    color: product.color || '#000000'
  });

  this.updateRevenue(); 
  $('#productModal').modal('show');
}

saveProduct() {

  console.log("🔵 [DEBUG] saveProduct() déclenché");

  if (this.productForm.invalid) {
    console.warn("⚠ Formulaire invalide :", this.productForm.value);
    this.productForm.markAllAsTouched();
    return;
  }

  const payload = this.productForm.getRawValue();

  console.log(" [DEBUG] RAW FORM VALUE :", payload);
  payload.revenue = payload.price * payload.sales;
  payload.color = payload.color || null;

  console.log("[DEBUG] PAYLOAD FINAL :", payload);

  this.isSaving = true;
  console.log("[DEBUG] isSaving = true");

  const isCreation = payload.id === 0;
  const request = isCreation
    ? this.productService.create(payload)
    : this.productService.update(payload.id, payload);

  console.log(`[DEBUG] API CALL → ${isCreation ? "POST /create" : "PUT /update/"+payload.id}`);

  request.subscribe({
    next: (saved) => {
      console.log("[SUCCESS] Product saved :", saved);

      const list = this.productsSubject.value;
      const index = list.findIndex(p => p.id === saved.id);

      if (index !== -1) {
        console.log("[DEBUG] Product found in the updated list");
        const newList = [...list];
        newList[index] = saved;
        this.productsSubject.next(newList);
      } else {
        console.log("[DEBUG] New product added to the list");
        this.productsSubject.next([...list, saved]);
      }

      this.isSaving = false;
      console.log("[DEBUG] isSaving = false");

      $('#productModal').modal('hide');
    },

    error: (err) => {
      console.error("[ERROR] API ERROR :", err);

      if (err.error) {
        console.error("[ERROR RESPONSE BODY] :", err.error);
      }

      if (err.status) {
        console.error("[HTTP STATUS] :", err.status);
      }

      this.isSaving = false;
      console.log("[DEBUG] isSaving = false (following error)");
    }
  });
}





  private finishSave(error?: any) {
    this.isSaving = false;
    if (error) console.error('Erreur sauvegarde:', error);

    //  petit délai pour éviter conflit de détection Angular
    setTimeout(() => this.closeModalInstant(), 100);
  }

  closeModalInstant() {
  const modal = document.getElementById('productModal');
  if (modal) {
    $(modal).modal('hide'); // cache direct
    modal.classList.remove('show'); //  supprime l'effet fade
    document.body.classList.remove('modal-open');
    document.body.removeAttribute('style');
    const backdrop = document.querySelector('.modal-backdrop');
    if (backdrop) backdrop.remove();
  }
}

   onDelete(product?: ProductDto) {
    const target = product || this.selectedProduct;
    if (target && confirm('Voulez-vous supprimer ce produit ?')) {
      this.productService.delete(target.id).subscribe(() => {
        const current = this.productsSubject.value;
        this.productsSubject.next(current.filter((p) => p.id !== target.id)); //  supprime direct
        if (this.selectedProduct === target) {
          this.selectedProduct = null;
        }
      });
    }
  }

 onView(product: ProductDto) {
  this.isViewMode = true;

  this.productForm.patchValue({
    id: product.id ?? 0,
    name: product.name,
    description: product.description,
    price: product.price,
    stockQuantity: product.stockQuantity,
    sales: product.sales,
    revenue: product.revenue,
    status: product.status,
    imageUrl: product.imageUrl,
    color: product.color
  });

  //  rendre le formulaire désactivé
  this.productForm.disable();

  $('#productModal').modal('show');
}

 toggleActions(event: Event) {
    event.stopPropagation();
    const target = event.currentTarget as HTMLElement;
    target.classList.toggle('active');
  }
  closeModal() {
    $('#productModal').modal('hide');
  }
private initForm() {
    this.productForm = this.fb.group({
      id: [{ value: 0, disabled: true }],
      name: ['', Validators.required],
      description: [''],
      price: [0, [Validators.required, Validators.min(0)]],
      stockQuantity: [0, [Validators.min(0)]],
      sales: [0, [Validators.min(0)]],
      revenue: [{ value: 0, disabled: true }], 
      status: ['Out of Stock'],
      imageUrl: [''], 
      color: ['#000000'], 
    });
  }
toggleDropdown() {
  this.dropdownOpen = !this.dropdownOpen;
}

selectColor(color: { name: string; value: string }) {
  this.selectedColor = color;
  this.productForm.get('color')?.setValue(color.value);
  this.dropdownOpen = false;
}
  

}
