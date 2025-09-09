// src/app/services/country.service.ts
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

export interface Country {
  name: string;
  code: string;
  dial_code: string;
}

@Injectable({
  providedIn: 'root'
})
export class CountryService {
  private countries: Country[] = [
    { name: 'United States', code: 'US', dial_code: '+1' },
    { name: 'United Kingdom', code: 'GB', dial_code: '+44' },
    { name: 'France', code: 'FR', dial_code: '+33' },
    { name: 'Germany', code: 'DE', dial_code: '+49' },
    { name: 'Japan', code: 'JP', dial_code: '+81' },
    { name: 'China', code: 'CN', dial_code: '+86' },
    { name: 'Brazil', code: 'BR', dial_code: '+55' },
    { name: 'Australia', code: 'AU', dial_code: '+61' },
    { name: 'India', code: 'IN', dial_code: '+91' },
    { name: 'Canada', code: 'CA', dial_code: '+1' },
    { name: 'Mexico', code: 'MX', dial_code: '+52' },
    { name: 'Russia', code: 'RU', dial_code: '+7' },
    { name: 'South Africa', code: 'ZA', dial_code: '+27' },
    { name: 'Egypt', code: 'EG', dial_code: '+20' },
    { name: 'Tunisia', code: 'TN', dial_code: '+216' },
    { name: 'Algeria', code: 'DZ', dial_code: '+213' },
    { name: 'Morocco', code: 'MA', dial_code: '+212' },
    { name: 'Spain', code: 'ES', dial_code: '+34' },
    { name: 'Italy', code: 'IT', dial_code: '+39' },
    { name: 'Portugal', code: 'PT', dial_code: '+351' },
    { name: 'Belgium', code: 'BE', dial_code: '+32' },
    { name: 'Switzerland', code: 'CH', dial_code: '+41' },
    { name: 'United Arab Emirates', code: 'AE', dial_code: '+971' },
    { name: 'Saudi Arabia', code: 'SA', dial_code: '+966' },
    { name: 'Argentina', code: 'AR', dial_code: '+54' },
    { name: 'Nigeria', code: 'NG', dial_code: '+234' },
    { name: 'Pakistan', code: 'PK', dial_code: '+92' },
    { name: 'Indonesia', code: 'ID', dial_code: '+62' },
    { name: 'South Korea', code: 'KR', dial_code: '+82' },
    { name: 'Turkey', code: 'TR', dial_code: '+90' }
  ];

  getCountries(): Observable<Country[]> {
    return of(this.countries);
  }
}