import { Component, OnInit } from '@angular/core';
import { ProductService } from 'src/app/services/product.service';
import { UntypedFormControl, UntypedFormGroup, Validators, UntypedFormBuilder } from '@angular/forms';
import { Observable,BehaviorSubject,combineLatest   } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { ProductDto  } from 'src/app/models/product.model';




declare var $: any;

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  //products: ProductDto[] = [];
   private productsSubject = new BehaviorSubject<ProductDto[]>([]);
  products$ = this.productsSubject.asObservable();
  filter = new UntypedFormControl('');
  statusFilter = new UntypedFormControl('');   // ✅ Ajouté
  filteredProducts$!: Observable<ProductDto[]>;
  selectedProduct: ProductDto | null = null;
  //editingProduct: Product = { id: 0, name: '', price: 0 };
  productForm!: UntypedFormGroup;
  // ✅ Liste des statuts
  statusList: string[] = ['In Stock', 'Out of Stock', 'Restock'];
  isSaving = false;
  isViewMode = false;
  // Pour gérer l'ouverture/fermeture des actions (3 points)
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

    // ✅ Combine produits + filtres
    this.filteredProducts$ = combineLatest([
      this.products$,
      this.filter.valueChanges.pipe(startWith('')),
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

   loadProducts() {
    this.productService.getAll().subscribe((products) => {
      // ✅ On push les données dans le BehaviorSubject
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
      status: 'Out of Stock'
    });
    $('#productModal').modal('show');
  }


onEdit(product: ProductDto) {
    this.isViewMode = false;
    this.productForm.enable();
    // patchValue pour remplir le formulaire (revenu est désactivé)
    this.productForm.patchValue({
      id: product.id ?? 0,
      name: product.name,
      description: product.description,
      price: product.price,
      stockQuantity: product.stockQuantity,
      sales: product.sales,
      status: product.status
    });
    this.updateRevenue(); // pour afficher revenue calculé dans le champ désactivé
    $('#productModal').modal('show');
  }

   saveProduct() {
    if (this.productForm.invalid) {
      this.productForm.markAllAsTouched();
      return;
    }

    const formValue = this.productForm.getRawValue() as ProductDto;
    formValue.id = formValue.id ?? 0;
    formValue.revenue = (Number(formValue.price) || 0) * (Number(formValue.sales) || 0);

    this.isSaving = true;

    if (formValue.id === 0) {
      // ✅ CREATE
      this.productService.create(formValue).subscribe({
        next: (created) => {
          const current = this.productsSubject.value;
          this.productsSubject.next([...current, created]); // 🔥 ajoute direct sans F5
          this.isSaving = false;
          $('#productModal').modal('hide');
        },
        error: (err) => {
          console.error(err);
          this.isSaving = false;
        }
      });
    } else {
      // ✅ UPDATE
      this.productService.update(formValue).subscribe({
        next: (updated) => {
          const current = this.productsSubject.value;
          const index = current.findIndex((p) => p.id === updated.id);
          if (index !== -1) {
            const newProducts = [...current];
            newProducts[index] = updated;
            this.productsSubject.next(newProducts); // 🔥 mise à jour auto
          }
          this.isSaving = false;
          $('#productModal').modal('hide');
        },
        error: (err) => {
          console.error(err);
          this.isSaving = false;
        }
      });
    }
  }


   onDelete(product?: ProductDto) {
    const target = product || this.selectedProduct;
    if (target && confirm('Voulez-vous supprimer ce produit ?')) {
      this.productService.delete(target.id).subscribe(() => {
        const current = this.productsSubject.value;
        this.productsSubject.next(current.filter((p) => p.id !== target.id)); // 🔥 supprime direct
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
    status: product.status
  });

  // ✅ rendre le formulaire désactivé
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
      revenue: [{ value: 0, disabled: true }], // calculé
      status: ['Out of Stock']
    });
  }
}
