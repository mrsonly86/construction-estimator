export type QuantityItem = {
  code: string;
  name: string;
  unit: string;
  quantity: number;
  unitPrice: number;
  wasteFactor?: number; // e.g. 0.03 for 3%
};

export type CostBreakdown = {
  directCost: number;
  overhead: number;
  profit: number;
  tax: number;
  total: number;
};

export type CostingParams = {
  overheadRate: number; // 0.065
  profitRate: number; // 0.05
  taxRate: number; // 0.10
};

export function calculateItemAmount(item: QuantityItem): number {
  const wasteMultiplier = 1 + (item.wasteFactor ?? 0);
  return item.quantity * item.unitPrice * wasteMultiplier;
}

export function calculateCost(items: QuantityItem[], params: CostingParams): CostBreakdown {
  const directCost = items.reduce((sum, it) => sum + calculateItemAmount(it), 0);
  const overhead = directCost * params.overheadRate;
  const profit = (directCost + overhead) * params.profitRate;
  const taxable = directCost + overhead + profit;
  const tax = taxable * params.taxRate;
  return {
    directCost,
    overhead,
    profit,
    tax,
    total: taxable + tax
  };
}

