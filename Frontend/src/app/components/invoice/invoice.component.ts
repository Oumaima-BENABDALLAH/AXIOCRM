import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Observable, BehaviorSubject, combineLatest } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import * as html2pdf from 'html2pdf.js';
import { ActivatedRoute, Router } from '@angular/router';
import { InvoiceService } from '../../services/invoice.service';
import { InvoiceDto } from '../../models/invoice.model';
declare var $: any;
@Component({
  selector: 'app-invoice',
  templateUrl: './invoice.component.html',
  styleUrls: ['./invoice.component.css']
})
export class InvoiceComponent implements OnInit {

  invoices: InvoiceDto[] = [];
  filteredInvoices$!: Observable<InvoiceDto[]>;
  filter = new FormControl('');
  selectedInvoice: InvoiceDto | null = null;
  selectedInvoiceModal: any = null;
  invoice!: InvoiceDto;
  loading = true;
  error = '';

  private refresh$ = new BehaviorSubject<void>(undefined);

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private invoiceService: InvoiceService
  ) {}

  ngOnInit(): void {

    const invoiceId = Number(this.route.snapshot.paramMap.get('id'));

    if (invoiceId) {
      this.loadInvoice(invoiceId);   
      return;
    }
    this.loadInvoices();

   this.filteredInvoices$ = combineLatest([
  this.refresh$,
  this.filter.valueChanges.pipe(startWith(''))
]).pipe(
  map(([_, search]) => {
    const s = (search || '').toLowerCase();

    return this.invoices.filter(inv =>
      inv.invoiceNumber.toLowerCase().includes(s) ||
      String(inv.orderId).includes(s) ||
      (inv.issueDate?.toString().toLowerCase().includes(s))
    );
  })
);
  }
 toggleActions(event: Event): void { event.stopPropagation(); const target = event.currentTarget as HTMLElement; target.classList.toggle('active'); }

  loadInvoices(): void {
    this.invoiceService.getAll().subscribe({
      next: (data) => {
        this.invoices = data;
        this.refresh$.next();  
      },
      error: () => {
        this.error = "Unable to load invoices.";
      }
    });
  }

  selectInvoice(invoice: InvoiceDto): void {
    this.selectedInvoice = invoice;
    console.log('selectedInvoice', this.selectedInvoice);
  }

  toggleRowSelection(invoice: InvoiceDto, event: any): void {
    event.stopPropagation();
    this.selectedInvoice = event.target.checked ? invoice : null;
  }

  toggleSelectAll(event: any): void {
    if (event.target.checked && this.invoices.length > 0) {
      this.selectedInvoice = this.invoices[0];
    } else {
      this.selectedInvoice = null;
    }
  }

openInvoicePreview(invoice: any) {
  this.selectedInvoice = invoice;
  $('#invoiceModal').modal('show');
}

  onView(invoice: InvoiceDto): void {
    this.router.navigate(['/invoices', invoice.id]);
  }



  loadInvoice(id: number): void {
    this.loading = true;

    this.invoiceService.getById(id).subscribe({
      next: (data) => {
        this.invoice = data;
        this.loading = false;
      },
      error: () => {
        this.error = "Unable to load invoice.";
        this.loading = false;
      }
    });
  }

  getItemTotal(price: number, qty: number): number {
    return price * qty;
  }
 downloadInvoice() {
  const element = document.querySelector('.invoice-card');
  if (!element) return;
  const h2p: any = (html2pdf as any).default || html2pdf;

  const options = {
    margin: 0.5,
    filename: 'invoice.pdf',
    image: { type: 'jpeg', quality: 0.98 },
    html2canvas: { scale: 2 },
    jsPDF: { unit: 'in', format: 'letter', orientation: 'portrait' }
  };

  h2p().from(element).set(options).save();
}
  printInvoice() {
  const printContent = document.querySelector('.right-section') as HTMLElement;

  if (!printContent) return;

  const windowPrint = window.open('', '', 'width=900,height=700');

  if (windowPrint) {
    windowPrint.document.write(`
      <html>
        <head>
          <title>Invoice</title>
          <style>
            body {
              font-family: Arial, sans-serif;
              padding: 20px;
            }
            .invoice-card {
              border: 1px solid #ddd;
              border-radius: 12px;
              padding: 25px;
            }
          </style>
        </head>
        <body>
          ${printContent.innerHTML}
        </body>
      </html>
    `);

    windowPrint.document.close();
    windowPrint.focus();

    setTimeout(() => {
      windowPrint.print();
      windowPrint.close();
    }, 300);
  }
}

}
