import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators, AbstractControl } from '@angular/forms';
import { BehaviorSubject, combineLatest, Observable, Subscription, of } from 'rxjs';
import { map, startWith, debounceTime } from 'rxjs/operators';
import { OrderService } from 'src/app/services/order.service';
import { OrderDto } from 'src/app/models/order.model';
import { ClientService, Client } from 'src/app/services/client.service';
import { ProductService } from 'src/app/services/product.service';
import { ProductDto } from 'src/app/models/product.model';

declare var $: any;

@Component({
  selector: 'app-order-list',
  templateUrl: './order-list.component.html',
  styleUrls: ['./order-list.component.css']
})
export class OrderListComponent implements OnInit, OnDestroy {
  private ordersSubject = new BehaviorSubject<OrderDto[]>([]);
  orders$ = this.ordersSubject.asObservable();
  orderForm!: FormGroup;

  clients: Client[] = [];
  products: ProductDto[] = [];
  showProductSelector = false;
  selectedOrders: number[] = [];
  selectAll: boolean = false;  
  selectedOrder: any = null;  
  filteredOrders$!: Observable<OrderDto[]>;
  showClientDetails = false;
  isSaving = false;
  isViewMode = false;
  isEditing = false;

  filter = this.fb.control('');

  paymentMethods = ['Cash', 'Card'];

  colorList = [
    { name: 'Black', value: '#000000' },
    { name: 'White', value: '#FFFFFF' },
    { name: 'Red', value: '#FF0000' },
    // ... etc
  ];

  private subs: Subscription[] = [];

  constructor(
    private orderService: OrderService,
    private fb: FormBuilder,
    private clientService: ClientService,
    private productService: ProductService
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.setupFiltering();
    this.loadOrders();
    this.loadClients();
    this.loadProducts();

    // guard valueChanges observables (product array may be empty)
   const productsChange$ = this.orderProductsArray ? this.orderProductsArray.valueChanges.pipe(startWith(this.orderProductsArray.value)): of([]);
      const deliveryPriceControl = this.orderForm.get('deliveryMethod.price');
    const deliveryChange$ = deliveryPriceControl ? deliveryPriceControl.valueChanges.pipe(startWith(this.getDeliveryCost())) : of(this.getDeliveryCost());

    this.subs.push(
      combineLatest([productsChange$, deliveryChange$]).subscribe(() => {
        this.updateTotalAmount();
        // if payment method is cash, keep cash amount synced (numeric)
        if (this.orderForm.get('paymentMethod')?.value === 'Cash') {
          const total = Number(this.orderForm.get('totalAmount')?.value) || 0;
          // keep numeric value
          this.orderForm.get('cashAmount')?.setValue(Number(total.toFixed(2)), { emitEvent: false });
        }
      })
    );

    // react to payment method changes
  
  }

  ngOnDestroy(): void {
    this.subs.forEach(s => s.unsubscribe());
  }

  private initForm() {
    this.orderForm = this.fb.group({
      id: [0],
      clientId: [null, Validators.required],
      clientInfo: this.fb.group({
        name: [''],
        lastName: [''],
        email: [''],
        phone: [''],
        address: [''],
        city: [''],
        province: [''],
        country: [''],
        postalCode: ['']
      }),
      orderDate: [this.getToday()],
      paymentMethod: ['Cash', Validators.required],
      totalAmount: [0],
      cardType: [''],
      cashAmount: [null],
      paymentDate: [null],
      cardNumber: [''],
      cardHolder: [''],
      expiryDate: [''],
      cvv: [''],
      orderProducts: this.fb.array([]),
      deliveryMethodId: [null],
      deliveryMethod: this.fb.group({
        id: [null],
        name: [''],
        price: [0],
        estimatedDays: [0]
      })
    });

  }
toggleRowSelection(order: OrderDto, event: any) {
  const checked = event.target.checked;

  if (checked) {
    // Ajouter dans la liste
    if (!this.selectedOrders.includes(order.id)) {
      this.selectedOrders.push(order.id);
    }
  } else {
    // Retirer de la liste
    this.selectedOrders = this.selectedOrders.filter(id => id !== order.id);
  }

  // Mettre à jour selectAll automatiquement
  this.selectAll = this.selectedOrders.length === this.ordersSubject.value.length;

  // Fix: selectedOrder = first selected only
  this.selectedOrder =
    this.selectedOrders.length > 0
      ? this.ordersSubject.value.find(o => o.id === this.selectedOrders[0])
      : null;
}

toggleSelectAll(event: any) {
  this.selectAll = event.target.checked;

  if (this.selectAll) {
    // sélectionner tous
    this.selectedOrders = this.ordersSubject.value.map(o => o.id);
    this.selectedOrder = this.ordersSubject.value[0]; // première ligne
  } else {
    // vider
    this.selectedOrders = [];
    this.selectedOrder = null;
  }
}

// Si on clique sur une ligne directement (ex: pour actions)
selectOrder(order: OrderDto) {
  this.selectedOrders = [order.id];
  this.selectAll = false;
  this.selectedOrder = order;
}
onPaymentMethodChange(method: string) {
  const cardFields = ['cardNumber', 'cardHolder', 'expiryDate', 'cvv', 'cardType'];
  const cashFields = ['cashAmount', 'paymentDate'];

  if (method === 'Card') {
    // Activer champs carte
    cardFields.forEach(f => {
      this.orderForm.get(f)?.setValidators([Validators.required]);
      this.orderForm.get(f)?.enable({ emitEvent: false });
    });

    // Désactiver cash
    cashFields.forEach(f => {
      this.orderForm.get(f)?.clearValidators();
      this.orderForm.get(f)?.disable({ emitEvent: false });
    });
  }

  else if (method === 'Cash') {
    // Activer champs cash
    cashFields.forEach(f => {
      this.orderForm.get(f)?.setValidators([Validators.required]);
      this.orderForm.get(f)?.enable({ emitEvent: false });
    });

    // Désactiver carte
    cardFields.forEach(f => {
      this.orderForm.get(f)?.clearValidators();
      this.orderForm.get(f)?.disable({ emitEvent: false });
    });
  }

  // revalidation
  this.orderForm.updateValueAndValidity({ onlySelf: false, emitEvent: false });
}

  getToday(): string {
    const today = new Date();
    today.setMinutes(today.getMinutes() - today.getTimezoneOffset());
    return today.toISOString().slice(0, 10);
  }

  selectDeliveryMethod(type: string) {
    const selectedMethod = type === 'standard'
      ? { id: 1, name: 'Standard', price: 7, estimatedDays: 5 }
      : { id: 2, name: 'Express', price: 10, estimatedDays: 2 };

    this.orderForm.patchValue({
      deliveryMethodId: selectedMethod.id,
      deliveryMethod: selectedMethod
    });
  }

 onClientChange(event: any): void {
  const clientId = Number(event.target.value);

  console.log(" Client sélectionné :", clientId);

  if (!clientId || clientId === 0 || isNaN(clientId)) {
    this.orderForm.patchValue({ clientId: null });
    this.showClientDetails = false;
    this.orderForm.get('clientInfo')?.reset();
    return;
  }

  this.orderForm.patchValue({ clientId });

  const selectedClient = this.clients.find(c => c.id === clientId);

  if (selectedClient) {
    this.showClientDetails = true;
    this.orderForm.get('clientInfo')?.patchValue({
      email: selectedClient.email || '',
      phone: selectedClient.phone || '',
      address: selectedClient.address || '',
      city: selectedClient.city || '',
      province: selectedClient.province || '',
      country: selectedClient.country || '',
      postalCode: selectedClient.postalCode || ''
    });
  }
}

  addProduct() {
    this.orderProductsArray.push(this.fb.group({
      productId: [0],
      productName: ['', Validators.required],
      quantity: [1, [Validators.required, Validators.min(1)]],
      price: [0],
      color: [''],
      imageUrl: ['']
    }));

    this.updateTotalAmount();
  }

  get orderProducts() {
  return this.orderProductsArray.value;
}

  get orderProductsArray(): FormArray {
    return this.orderForm.get('orderProducts') as FormArray;
  }

onAdd() {
  this.showClientDetails = false;

  this.isViewMode = false;
    
  this.orderForm.reset({
    id: 0,
    clientId: null,
    orderDate: this.getToday(),
    paymentMethod: 'Cash',
    paymentDate: null
  });
 this.orderForm.enable();  
  this.orderForm.setControl('orderProducts', this.fb.array([]));

  $('#orderModal').modal('show');
}

  loadOrders(): void {
    this.orderService.getOrders().subscribe({
      next: (orders) => this.ordersSubject.next(orders),
      error: (err) => console.error('Erreur loadOrders', err)
     
    });
    
  }

  loadClients(): void {
    this.clientService.getAll().subscribe({ next: c => this.clients = c, error: e => console.error(e) });
  }

  loadProducts(): void {
    this.productService.getAll().subscribe({ next: p => this.products = p, error: e => console.error(e) });
  }

  setupFiltering(): void {
    this.filteredOrders$ = combineLatest([
      this.orders$,
      this.filter.valueChanges.pipe(startWith(''))
    ]).pipe(
      map(([orders, search]) => {
        const term = (search || '').toLowerCase();
        return orders.filter(o =>
          (o.paymentMethod || '').toLowerCase().includes(term) ||
          (o.clientId?.toString() || '').includes(term) ||
          (o.orderProducts|| []).some(p => (p.productName || '').toLowerCase().includes(term))
        );
      })
    );
  }

  onEdit(order: OrderDto) {
    this.isEditing = true;
    this.isViewMode = false;
    this.selectedOrder = order;
    this.orderForm.enable();
    this.orderForm.reset();
    this.orderProductsArray.clear();

    this.orderForm.patchValue({
      id: order.id,
      clientId: order.clientId,

      // 🟢 Toujours envoyer yyyy-MM-dd
      orderDate: order.orderDate?.slice(0, 10),

      paymentMethod: order.paymentMethod,

      cashAmount: order.paymentMethod === "Cash" ? order.cashAmount ?? null : null,

      // 🟢 IMPORTANT : jamais d’ISO → slice(0,10)
      paymentDate:
        order.paymentMethod === "Cash" && order.paymentDate
          ? order.paymentDate.slice(0, 10)
          : null,

      cardNumber: order.paymentMethod === "Card" ? order.cardNumber : null,
      cardHolder: order.paymentMethod === "Card" ? order.cardHolder : null,
      expiryDate: order.paymentMethod === "Card" ? order.expiryDate : null,
      cvv: order.paymentMethod === "Card" ? order.cvv : null,

      totalAmount: order.totalAmount ?? 0,

      // 🟢 FIX : toujours récupérer deliveryMethodId d’abord
      deliveryMethodId:
        order.deliveryMethodId ??
        order.deliveryMethod?.id ??
        null,
    });

    // -------- PRODUITS --------
    (order.orderProducts || []).forEach(p => {
  const prod = this.products.find(x => x.id === p.productId);

  this.orderProductsArray.push(
    this.fb.group({
      productId: [p.productId],
      productName: [prod?.name || p.productName],
      quantity: [p.quantity],
      price: [p.unitPrice ?? p.unitPrice ?? prod?.price ?? 0],
      color: [p.color ?? prod?.color ?? ''],
      imageUrl: [p.imageUrl ?? prod?.imageUrl ?? '']
    })
  );
});


    // -------- DELIVERY --------
    this.orderForm.get("deliveryMethod")?.patchValue({
      id:
        order.deliveryMethodId ??
        order.deliveryMethod?.id ??
        null,
      name: order.deliveryMethod?.name ?? "",
      price: order.deliveryMethod?.price ?? 0,
      estimatedDays: order.deliveryMethod?.estimatedDays ?? 0,
    });

    // -------- CLIENT --------
    const client = this.clients.find(c => c.id === order.clientId);
    if (client) {
      this.showClientDetails = true;
      this.orderForm.get("clientInfo")?.patchValue({
        email: client.email,
        phone: client.phone,
        address: client.address,
        city: client.city,
        province: client.province,
        country: client.country,
        postalCode: client.postalCode,
      });
    }

    // clientId readonly
    this.orderForm.get("clientId")?.disable({ emitEvent: false, onlySelf: true });
    this.orderForm.get("clientId")?.addValidators([Validators.required]);

    // Recalcul règles payement


    // Recalcul total
    this.updateTotalAmount();

    $('#orderModal').modal('show');
  }

  onView(order: OrderDto): void {
    this.isViewMode = true;
    this.selectedOrder = order;
    this.orderForm.patchValue(order as any);
    this.orderForm.disable();
    $('#orderModal').modal('show');
  }

  onDelete(order: OrderDto): void {
    if (!confirm(`Supprimer la commande #${order.id} ?`)) return;
    this.orderService.deleteOrder(order.id).subscribe(() => this.loadOrders());
  }

  private updateTotalAmount() {
    const total = this.getTotalToPay();
    this.orderForm.get('totalAmount')?.setValue(total, { emitEvent: false });
  }

onSave() {
  console.log("========== 🟦 START onSave() 🟦 ==========");

  this.onPaymentMethodChange(this.orderForm.value.paymentMethod);

  this.orderForm.updateValueAndValidity({ onlySelf: false, emitEvent: false });

  if (this.orderForm.invalid) {
    console.warn("⚠ FORMULAIRE INVALIDE !");
    this.logFormErrors(this.orderForm);
    return;
  }

  this.orderForm.enable({ emitEvent: false });

  const fv = this.orderForm.getRawValue();

  // ==============================
  // 🟢 DTO FINAL
  // ==============================
  const dto: any = {
    id: fv.id ?? 0,
    clientId: Number(fv.clientId),
    orderDate: fv.orderDate,
    paymentMethod: fv.paymentMethod,
    deliveryMethodId: fv.deliveryMethodId ?? null,

    // CASH
    cashAmount:
      fv.paymentMethod === 'Cash' ? Number(fv.cashAmount || 0) : null,

    paymentDate:
      fv.paymentMethod === 'Cash' ? fv.paymentDate ?? null : null,

    // CARD
    cardNumber: fv.paymentMethod === 'Card' ? fv.cardNumber : null,
    cardHolder: fv.paymentMethod === 'Card' ? fv.cardHolder : null,
    expiryDate: fv.paymentMethod === 'Card' ? fv.expiryDate : null,
    cvv: fv.paymentMethod === 'Card' ? fv.cvv : null,
    cardType: fv.paymentMethod === 'Card' ? fv.cardType : null,

    totalAmount: this.getTotalToPay(),

    // ===============================
    // 🟦 PRODUITS 100% CORRECTS
    // ===============================
   orderProducts: this.orderProductsArray.controls.map(p => ({
  productId: p.value.productId,
  quantity: p.value.quantity,
  unitPrice: p.value.price ?? 0,
  color: p.value.color ?? null,
  imageUrl: p.value.imageUrl ?? null
}))

  };

  console.log("📦 DTO FINAL :", dto);

  this.isSaving = true;

  const request =
    !dto.id || dto.id === 0
      ? this.orderService.createOrder(dto)
      : this.orderService.updateOrder(dto);

  request.subscribe({
    next: () => {
      this.isSaving = false;
      console.log("✅ SAVE OK");
      $('#orderModal').modal('hide');
      this.loadOrders();
    },
    error: (err) => {
      this.isSaving = false;
      console.error("❌ BACKEND ERROR :", err);
      console.error("📤 DTO AU MOMENT DE L'ERREUR :", dto);
      alert('Erreur : ' + (err?.error || err?.message || err?.statusText));
    }
  });

  console.log("========== 🟧 END onSave() 🟧 ==========");
}


  logFormErrors(form: FormGroup | FormArray, prefix = '') {
    Object.keys(form.controls).forEach(key => {
      const control: any = form.get(key);
      if (control instanceof FormGroup || control instanceof FormArray) {
        this.logFormErrors(control, `${prefix}${key}.`);
      } else {
        if (control?.invalid) {
          console.error(`❌ Champ invalide : ${prefix}${key}`, control.errors);
        }
      }
    });
  }

  trackInvalidControls(form: FormGroup | FormArray, path: string = '') {
    Object.keys(form.controls).forEach(key => {
      const control = form.get(key);
      const fullPath = path ? `${path}.${key}` : key;

      if (control instanceof FormGroup || control instanceof FormArray) {
        this.trackInvalidControls(control, fullPath);
      } else {
        if (control?.invalid) {
          console.error(`❌ INVALID → ${fullPath}`, control.errors);
        }

        // Only warn NULL/UNDEFINED for controls that have validators (required etc.)
        const hasValidators = !!control?.validator;
        if (hasValidators && (control?.value === null || control?.value === undefined)) {
          console.warn(`⚠ NULL/UNDEFINED → ${fullPath}`);
        }

        if (typeof control?.value === "number" && isNaN(control.value)) {
          console.warn(`⚠ NaN VALUE → ${fullPath}`);
        }
      }
    });
  }

  resetForm(): void {
    this.showClientDetails = false;
  this.selectedOrder = null;

  this.orderForm.reset({
    id: 0,
    clientId: null,
    orderDate: this.getToday(),
    paymentMethod: 'Cash',
    paymentDate: null
  });

  this.orderForm.setControl('orderProducts', this.fb.array([]));

  $('#orderModal').modal('show');
   
  }

  addProductToOrder(p: ProductDto) {
    const group = this.fb.group({
      productId: [p.id],
      productName: [p.name],
      imageUrl: [p.imageUrl],
      price: [p.price],
      color: [p.color],
      quantity: [1]
    });

    this.orderProductsArray.push(group);
    this.updateTotalAmount();
    this.closeProductSelector();
  }

  increaseQty(i: number) {
    const item = this.orderProductsArray.at(i);
    const qty = (item.get('quantity')!.value || 0) + 1;
    item.patchValue({ quantity: qty });
    this.updateTotalAmount();
  }

  decreaseQty(i: number) {
    const item = this.orderProductsArray.at(i);
    const qty = (item.get('quantity')!.value || 0) - 1;
    if (qty <= 0) {
      this.orderProductsArray.removeAt(i);
    } else {
      item.patchValue({ quantity: qty });
    }
    this.updateTotalAmount();
  }

  removeProduct(i: number) {
    this.orderProductsArray.removeAt(i);
    this.updateTotalAmount();
  }

  getTotal(): number {
    return this.orderProductsArray.controls
      .map(p => (p.value.price || 0) * (p.value.quantity || 0))
      .reduce((a, b) => a + b, 0);
  }

  getDeliveryCost(): number {
    return Number(this.orderForm.get('deliveryMethod.price')?.value || 0);
  }

  getTotalToPay(): number {
    return this.getTotal() + this.getDeliveryCost();
  }

  openProductSelector() {this.loadProducts();  this.showProductSelector = true; }
  closeProductSelector() { this.showProductSelector = false; }
  toggleActions(event: Event): void { event.stopPropagation(); const target = event.currentTarget as HTMLElement; target.classList.toggle('active'); }
  getColorName(color: string | null): string {
    if (!color) return "N/A";

    const colorMap: any = {
      black: "Black",
      white: "White",
      red: "Red",
      blue: "Blue",
      green: "Green",
      yellow: "Yellow"
    };

    return colorMap[color.toLowerCase()] ?? color;
  }
  onAddInvoice(order: OrderDto) {
  if (!order || !order.id) {
    this.showToast("Please select an order first!", "error");
    return;
  }

  this.orderService.generateInvoice(order.id).subscribe({
    next: () => {
      this.showToast("Invoice generated successfully!", "success");
      this.loadOrders(); // 🔄 Recharge la liste
    },
    error: (err) => {
      console.error("Invoice generation error", err);
      this.showToast("Failed to generate invoice!", "error");
    }
  });
}
showToast(message: string, type: "success" | "error") {
  const bg = type === "success" ? "green" : "red";

  const toast = document.createElement("div");
  toast.innerText = message;
  toast.style.position = "fixed";
  toast.style.bottom = "20px";
  toast.style.right = "20px";
  toast.style.padding = "12px 20px";
  toast.style.color = "white";
  toast.style.borderRadius = "6px";
  toast.style.backgroundColor = bg;
  toast.style.zIndex = "9999";
  toast.style.boxShadow = "0 4px 10px rgba(0,0,0,0.3)";
  toast.style.opacity = "0";
  toast.style.transition = "opacity .3s";

  document.body.appendChild(toast);

  setTimeout(() => toast.style.opacity = "1", 10);
  setTimeout(() => {
    toast.style.opacity = "0";
    setTimeout(() => toast.remove(), 300);
  }, 2500);
}

}
