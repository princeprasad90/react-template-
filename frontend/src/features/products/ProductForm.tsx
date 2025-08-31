import React, { useState } from 'react';

export interface Product {
  code: string;
  name: string;
  description: string;
  quantity: number;
}

interface Props {
  initial?: Product;
  onSave: (product: Product) => void;
  onCancel: () => void;
}

const ProductForm: React.FC<Props> = ({ initial, onSave, onCancel }) => {
  const [form, setForm] = useState<Product>(
    initial || { code: '', name: '', description: '', quantity: 0 }
  );

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setForm(f => ({ ...f, [name]: name === 'quantity' ? Number(value) : value }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSave(form);
  };

  return (
    <form onSubmit={handleSubmit}>
      <div>
        <label>Code</label>
        <input name="code" value={form.code} onChange={handleChange} required />
      </div>
      <div>
        <label>Name</label>
        <input name="name" value={form.name} onChange={handleChange} required />
      </div>
      <div>
        <label>Description</label>
        <textarea name="description" value={form.description} onChange={handleChange} />
      </div>
      <div>
        <label>Quantity</label>
        <input type="number" name="quantity" value={form.quantity} onChange={handleChange} />
      </div>
      <button type="submit">Save</button>
      <button type="button" onClick={onCancel}>Cancel</button>
    </form>
  );
};

export default ProductForm;
