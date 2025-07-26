import {
  Component,
  Input,
  OnInit,
  AfterViewInit,
  ViewChild,
  ElementRef
} from '@angular/core';
import {
  ApexAxisChartSeries,
  ApexChart,
  ApexStroke,
  ApexTooltip,
  ApexNonAxisChartSeries,
  ApexResponsive
} from 'ng-apexcharts';

@Component({
  selector: 'app-sparkline-card',
  templateUrl: './sparkline-card.component.html',
  styleUrls: ['./sparkline-card.component.css']
})
export class SparklineCardComponent implements OnInit, AfterViewInit {
  @Input() title: string = 'Metric';
  @Input() value: string = '$0';
  @Input() change: string = '+0%';
  @Input() changeColor: 'green' | 'red' = 'green';
  @Input() data: number[] = [];
  @Input() color: string = '#22c55e';
  @Input() variant: 'default' | 'gradient'|'balance' = 'default';
  @Input() chartType: 'area' | 'line' | 'pie' | 'bar' = 'area';
  @Input() compact: boolean = false;  
  @Input() highlightValue: boolean = false;


  @ViewChild('customCanvas') customCanvasRef!: ElementRef<HTMLCanvasElement>;

  public series: ApexAxisChartSeries = [];
  public pieSeries: ApexNonAxisChartSeries = [];
  public pieChart: ApexChart = {
    type: 'pie',
    width: 100
  };
  public pieLabels: string[] = ['A', 'B', 'C', 'D'];
  public pieColors: string[] = ['#6366f1', '#3b82f6', '#06b6d4', '#10b981'];
  public pieResponsive: ApexResponsive[] = [
    {
      breakpoint: 480,
      options: {
        chart: { width: 80 },
        legend: { position: 'bottom' }
      }
    }
  ];

  public chart: ApexChart = {
    type: 'line',
    height: 35,
    width: 80,
    sparkline: { enabled: true }
  };
  public stroke: ApexStroke = { width: 2 };
  public tooltip: ApexTooltip = { enabled: false };

  ngOnInit() {
    this.series = [{ data: this.data }];
    this.color = this.changeColor === 'green' ? '#22c55e' : '#ef4444';
    if (this.chartType === 'pie') {
      this.pieSeries = this.data;
    }
  }

  ngAfterViewInit(): void {
    if (this.variant === 'gradient' || this.variant === 'balance') {
      this.drawChart();
    }
  }

  isPositive(): boolean {
    return this.change.startsWith('+');
  }

 /* drawChart(): void {
    const canvas = this.customCanvasRef?.nativeElement;
    const ctx = canvas?.getContext('2d');
    if (!ctx || this.data.length === 0) return;

    // Dynamically sync canvas size
    canvas.width = canvas.offsetWidth;
    canvas.height = canvas.offsetHeight;

    const width = canvas.width;
    const height = canvas.height;

    const maxVal = Math.max(...this.data);
    const minVal = Math.min(...this.data);
    const scaleY = maxVal - minVal === 0 ? 1 : height / (maxVal - minVal);
    const stepX = width / (this.data.length - 1);

    const getX = (i: number) => i * stepX;
    const getY = (val: number) => (maxVal - val) * scaleY;

    ctx.clearRect(0, 0, width, height);

    // === Gradient background
    ctx.beginPath();
    ctx.moveTo(getX(0), getY(this.data[0]));

    for (let i = 0; i < this.data.length - 1; i++) {
      const x0 = getX(i);
      const y0 = getY(this.data[i]);
      const x1 = getX(i + 1);
      const y1 = getY(this.data[i + 1]);
      const cx = (x0 + x1) / 2;
      ctx.quadraticCurveTo(x0, y0, cx, (y0 + y1) / 2);
    }

    ctx.lineTo(getX(this.data.length - 1), height);
    ctx.lineTo(0, height);
    ctx.closePath();

    const gradient = ctx.createLinearGradient(0, 0, 0, height);
    gradient.addColorStop(0, 'rgba(255, 0, 128, 0.2)');
    gradient.addColorStop(1, 'rgba(255, 0, 128, 0.05)');
    ctx.fillStyle = gradient;
    ctx.fill();

    // === Line stroke
    ctx.beginPath();
    ctx.moveTo(getX(0), getY(this.data[0]));

    for (let i = 0; i < this.data.length - 1; i++) {
      const x0 = getX(i);
      const y0 = getY(this.data[i]);
      const x1 = getX(i + 1);
      const y1 = getY(this.data[i + 1]);
      const cx = (x0 + x1) / 2;
      ctx.quadraticCurveTo(x0, y0, cx, (y0 + y1) / 2);
    }

    ctx.strokeStyle = '#5b21b6';
    ctx.lineWidth = 2.5;
    ctx.stroke();
  }*/
 drawChart(): void {
  const canvas = this.customCanvasRef?.nativeElement;
  const ctx = canvas?.getContext('2d');

  if (!ctx || this.data.length === 0) return;

  canvas.width = canvas.offsetWidth;
  canvas.height = canvas.offsetHeight;

  const width = canvas.width;
  const height = canvas.height;

  // 🔁 Utiliser les données inversées si variant === 'balance'
  const chartData = this.variant === 'balance' ? [...this.data].reverse() : this.data;

  const maxVal = Math.max(...chartData);
  const minVal = Math.min(...chartData);
  const padding = height * 0.1;
  const usableHeight = height - 2 * padding;
  const scaleY = maxVal - minVal === 0 ? 1 : usableHeight / (maxVal - minVal);

  const stepX = width / (chartData.length - 1);
  const getX = (i: number) => i * stepX;
  const getY = (val: number) => padding + (maxVal - val) * scaleY;

  ctx.clearRect(0, 0, width, height);

  // === 🎨 FOND DÉGRADÉ
  ctx.beginPath();
  ctx.moveTo(getX(0), getY(chartData[0]));

  for (let i = 0; i < chartData.length - 1; i++) {
    const x0 = getX(i);
    const y0 = getY(chartData[i]);
    const x1 = getX(i + 1);
    const y1 = getY(chartData[i + 1]);
    const cx = (x0 + x1) / 2;
    const cy = (y0 + y1) / 2;
    ctx.quadraticCurveTo(x0, y0, cx, cy);
  }

  ctx.lineTo(getX(chartData.length - 1), height);
  ctx.lineTo(0, height);
  ctx.closePath();

  const gradient = ctx.createLinearGradient(0, 0, 0, height);

  if (this.variant === 'gradient') {
    gradient.addColorStop(0, 'rgba(255, 0, 128, 0.2)');
    gradient.addColorStop(1, 'rgba(255, 0, 128, 0.05)');
    ctx.fillStyle = gradient;
    ctx.fill();
  }

  if (this.variant === 'balance') {
    gradient.addColorStop(0, 'rgba(0, 255, 255, 0.2)');
    gradient.addColorStop(1, 'rgba(0, 255, 255, 0.05)');
    ctx.fillStyle = gradient;
    ctx.fill();
  }

  // === 🟣 COURBE
  ctx.beginPath();
  ctx.moveTo(getX(0), getY(chartData[0]));

  for (let i = 0; i < chartData.length - 1; i++) {
    const x0 = getX(i);
    const y0 = getY(chartData[i]);
    const x1 = getX(i + 1);
    const y1 = getY(chartData[i + 1]);
    const cx = (x0 + x1) / 2;
    const cy = (y0 + y1) / 2;
    ctx.quadraticCurveTo(x0, y0, cx, cy);
  }

  ctx.lineTo(getX(chartData.length - 1), getY(chartData[chartData.length - 1]));

  ctx.strokeStyle = '#5b21b6';
  ctx.lineWidth = 2.5;
  ctx.stroke();
}
 


}
