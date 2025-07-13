import { Component, OnInit } from '@angular/core';
import { ProductService } from 'src/app/services/product.service';
import  { UntypedFormControl } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { Product } from 'src/app/models/product.model';

declare var $: any; // Pour utiliser jQuery et le modal Bootstrap


@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  products:Product[] = [];
  filter = new UntypedFormControl('');
  filteredProducts$!: Observable<Product[]>;
  selectedProduct: Product | null = null;
  editingProduct: Product = { id: 0, name: '', price: 0 };

  


  constructor( private productService : ProductService , private sanitizer: DomSanitizer) { }

  ngOnInit() :void {
    this.loadProducts();
  }
loadProducts() {
  this.productService.getAll().subscribe(data => {
    console.log('✅ Données reçues de l\'API :', data);

    if (Array.isArray(data)) {
      this.products = data;
    } else if (data && data.$values && Array.isArray(data.$values)) {
      this.products = data.$values;
    } else {
      console.warn('❌ Structure inattendue des données reçues', data);
      this.products = [];
    }

    this.filteredProducts$ = this.filter.valueChanges.pipe(
      startWith(''),
      map(term =>
        this.products.filter(p =>
          p.name.toLowerCase().includes(term.toLowerCase()) ||
          p.price.toString().includes(term)
        )
      )
    );
    this.filter.setValue('');
  }, error => {
    console.error('Erreur lors du chargement des produits', error);
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
  selectProduct(product: Product) {
  this.selectedProduct = product;
   } 
   onAdd() {
    this.editingProduct = {id :0, name :'', price:0}
    $('#productModal').modal('show'); 
}
  onEdit() {
  if (this.selectedProduct) {
    this.editingProduct = {...this.selectedProduct};
    $('#productModal').modal('show'); 
  }
}
saveProduct(){
  if(this.editingProduct.id ===0){
    this.productService.create(this.editingProduct).subscribe(newProduct =>{
      this.products.push(newProduct);
      this.filter.setValue(this.filter.value);
      $('#productModal').modal('show');
    });
  }
    else {
      // Modifier
      this.productService.update(this.editingProduct).subscribe(() => {
        const index = this.products.findIndex(p => p.id === this.editingProduct.id);
        if (index !== -1) {
          this.products[index] = { ...this.editingProduct };
          this.filter.setValue(this.filter.value);
        }
        $('#productModal').modal('hide');
      });
  }
}
onDelete() {
  if (this.selectedProduct && confirm("Voulez-vous supprimer ce produit ?")) {
    this.productService.delete(this.selectedProduct.id).subscribe(() => {
      this.products = this.products.filter(p => p !== this.selectedProduct);
      this.filter.setValue(this.filter.value); // rafraîchir le filtre
      this.selectedProduct = null;
    });
  }
  
}
closeModal() {
  $('#productModal').modal('hide');
}
onProductSelected(product: Product) {
    console.log('Produit reçu de l’enfant :', product);
  }
}
