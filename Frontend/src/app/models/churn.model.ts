export interface ChurnDashboard {
  totalClients: number;
  riskClients: number;
  riskRate: number;
}

export interface ChurnPrediction {
  willChurn: boolean;
  probability: number;
  score: number;
}

export interface RiskClient {
  id: number;
  name: string;
  probability: number;
}

export interface MonthlyChurn {
  month: string;
  rate: number;
}