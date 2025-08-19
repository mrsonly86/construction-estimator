import React from 'react';
import { createRoot } from 'react-dom/client';
import { calculateCost, type QuantityItem, type CostingParams } from './lib/calc/costing';

function App() {
  const [items, setItems] = React.useState<QuantityItem[]>([
    { code: 'A.1', name: 'Đào đất', unit: 'm3', quantity: 10, unitPrice: 120000 },
    { code: 'A.2', name: 'Bê tông', unit: 'm3', quantity: 5, unitPrice: 1500000, wasteFactor: 0.02 }
  ]);
  const [params, setParams] = React.useState<CostingParams>({ overheadRate: 0.065, profitRate: 0.05, taxRate: 0.1 });
  const result = React.useMemo(() => calculateCost(items, params), [items, params]);

  const updateItem = (index: number, field: keyof QuantityItem, value: string) => {
    setItems(prev => prev.map((it, i) => i === index ? { ...it, [field]: field === 'name' || field === 'code' || field === 'unit' ? value : Number(value) } as QuantityItem : it));
  };

  const addRow = () => setItems(prev => [...prev, { code: '', name: '', unit: '', quantity: 0, unitPrice: 0 }]);
  const removeRow = (index: number) => setItems(prev => prev.filter((_, i) => i !== index));

  // @ts-ignore
  const [pong, setPong] = React.useState<string>('');
  React.useEffect(() => {
    // @ts-ignore
    window.api?.ping().then((res: string) => setPong(res));
  }, []);

  return (
    <div style={{ fontFamily: 'Inter, system-ui, Arial', padding: 16, maxWidth: 1100, margin: '0 auto' }}>
      <h1>DuToan Pro</h1>
      <p>IPC: {pong}</p>

      <h3>Danh mục công việc (BOQ)</h3>
      <button onClick={addRow}>Thêm dòng</button>
      <div style={{ overflowX: 'auto', marginTop: 8 }}>
        <table style={{ width: '100%', borderCollapse: 'collapse' }}>
          <thead>
            <tr>
              <th style={{ borderBottom: '1px solid #ddd', textAlign: 'left' }}>Mã</th>
              <th style={{ borderBottom: '1px solid #ddd', textAlign: 'left' }}>Tên</th>
              <th style={{ borderBottom: '1px solid #ddd', textAlign: 'left' }}>Đvt</th>
              <th style={{ borderBottom: '1px solid #ddd', textAlign: 'right' }}>Khối lượng</th>
              <th style={{ borderBottom: '1px solid #ddd', textAlign: 'right' }}>Đơn giá</th>
              <th style={{ borderBottom: '1px solid #ddd', textAlign: 'right' }}>Hao hụt (%)</th>
              <th style={{ borderBottom: '1px solid #ddd', textAlign: 'right' }}>Thành tiền</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {items.map((it, idx) => (
              <tr key={idx}>
                <td><input value={it.code} onChange={e => updateItem(idx, 'code', e.target.value)} style={{ width: 80 }} /></td>
                <td><input value={it.name} onChange={e => updateItem(idx, 'name', e.target.value)} style={{ width: 300 }} /></td>
                <td><input value={it.unit} onChange={e => updateItem(idx, 'unit', e.target.value)} style={{ width: 60 }} /></td>
                <td style={{ textAlign: 'right' }}><input type="number" value={it.quantity} onChange={e => updateItem(idx, 'quantity', e.target.value)} style={{ width: 100, textAlign: 'right' }} /></td>
                <td style={{ textAlign: 'right' }}><input type="number" value={it.unitPrice} onChange={e => updateItem(idx, 'unitPrice', e.target.value)} style={{ width: 120, textAlign: 'right' }} /></td>
                <td style={{ textAlign: 'right' }}><input type="number" value={(it.wasteFactor ?? 0) * 100} onChange={e => updateItem(idx, 'wasteFactor', String(Number(e.target.value) / 100))} style={{ width: 100, textAlign: 'right' }} /></td>
                <td style={{ textAlign: 'right' }}>{(it.quantity * it.unitPrice * (1 + (it.wasteFactor ?? 0))).toLocaleString('vi-VN')}</td>
                <td><button onClick={() => removeRow(idx)}>Xóa</button></td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      <h3 style={{ marginTop: 16 }}>Thông số chi phí</h3>
      <div style={{ display: 'flex', gap: 16 }}>
        <label>Chi phí chung (%) <input type="number" value={params.overheadRate * 100} onChange={e => setParams(p => ({ ...p, overheadRate: Number(e.target.value) / 100 }))} /></label>
        <label>Lợi nhuận (%) <input type="number" value={params.profitRate * 100} onChange={e => setParams(p => ({ ...p, profitRate: Number(e.target.value) / 100 }))} /></label>
        <label>Thuế VAT (%) <input type="number" value={params.taxRate * 100} onChange={e => setParams(p => ({ ...p, taxRate: Number(e.target.value) / 100 }))} /></label>
      </div>

      <h3 style={{ marginTop: 16 }}>Kết quả</h3>
      <ul>
        <li>Chi phí trực tiếp: {result.directCost.toLocaleString('vi-VN')}</li>
        <li>Chi phí chung: {result.overhead.toLocaleString('vi-VN')}</li>
        <li>Lợi nhuận: {result.profit.toLocaleString('vi-VN')}</li>
        <li>Thuế: {result.tax.toLocaleString('vi-VN')}</li>
        <li><strong>Tổng cộng: {result.total.toLocaleString('vi-VN')}</strong></li>
      </ul>
    </div>
  );
}

const container = document.getElementById('root');
if (container) {
  const root = createRoot(container);
  root.render(<App />);
}