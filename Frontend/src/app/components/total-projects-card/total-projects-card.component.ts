import { Component, AfterViewInit, ViewChild, ElementRef } from '@angular/core';

@Component({
  selector: 'app-total-projects-card',
  standalone: true,
  templateUrl: './total-projects-card.component.html',
  styleUrls: ['./total-projects-card.component.css']
})
export class TotalProjectsCardComponent implements AfterViewInit {
  @ViewChild('barCanvas') barCanvasRef!: ElementRef<HTMLCanvasElement>;

  ngAfterViewInit(): void {
    this.drawBarChart();
  }

  drawBarChart(): void {
    const canvas = this.barCanvasRef.nativeElement;
    const ctx = canvas.getContext('2d');
    if (!ctx) return;

    const data = [20, 30, 25, 35, 28, 40, 33,70,15,31,55,20, 30, 25, 35, 28,];
    const width = 13;
    const gap = 10;
    const maxHeight = 130;

    data.forEach((value, i) => {
      const barHeight = (value / Math.max(...data)) * maxHeight;
      const x = i * (width + gap) + 20;
      const y = canvas.height - barHeight;

      ctx.fillStyle = i % 2 === 0 ? '#7c3aed' : '#fbb6ce';
      ctx.fillRect(x, y, width, barHeight);
    });
  }
}
