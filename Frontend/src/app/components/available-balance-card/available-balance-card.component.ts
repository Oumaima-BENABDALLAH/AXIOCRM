import { Component, AfterViewInit, ViewChild, ElementRef } from '@angular/core';

@Component({
  selector: 'app-available-balance-card',
  templateUrl: './available-balance-card.component.html',
  styleUrl: './available-balance-card.component.css'
})
export class AvailableBalanceCardComponent implements AfterViewInit {
  @ViewChild('pieCanvas') pieCanvasRef!: ElementRef<HTMLCanvasElement>;

  ngAfterViewInit(): void {
    this.drawPieChart();
  }

  drawPieChart(): void {
    const canvas = this.pieCanvasRef.nativeElement;
    const ctx = canvas.getContext('2d');
    if (!ctx) return;

    const total = 100;
    const part1 = 85;
    const part2 = total - part1;

    const centerX = canvas.width / 2;
    const centerY = canvas.height / 2;
    const radius = 45;
    let startAngle = 0;

    let angle1 = (part1 / total) * 2 * Math.PI;
    ctx.beginPath();
    ctx.moveTo(centerX, centerY);
    ctx.arc(centerX, centerY, radius, startAngle, startAngle + angle1);
    ctx.closePath();
    ctx.fillStyle = '#7c3aed'; 
    ctx.fill();
    startAngle += angle1;
    let angle2 = (part2 / total) * 2 * Math.PI;
    ctx.beginPath();
    ctx.moveTo(centerX, centerY);
    ctx.arc(centerX, centerY, radius, startAngle, startAngle + angle2);
    ctx.closePath();
    ctx.fillStyle = '#fbb6ce'; 
    ctx.fill();
  }
}