import React, { useEffect, useState } from 'react';
import ProductForm, { Product } from '../features/products/ProductForm';
import ConfirmModal from '../components/ConfirmModal';
import { api } from '../api';
import { useToast } from '../components/Toast';

const Products: React.FC = () => {
  const [products, setProducts] = useState<Product[]>([]);
  const [editing, setEditing] = useState<Product | null>(null);
  const [showForm, setShowForm] = useState(false);
  const [deleteCode, setDeleteCode] = useState<string | null>(null);
  const { addToast } = useToast();

  const load = async () => {
    try {
      const data = await api<Product[]>('/api/products');
      setProducts(data);
    } catch {
      addToast('Failed to load products', 'error');
    }
  };

  useEffect(() => { load(); }, []);

  const handleSave = async (p: Product) => {
    try {
      if (editing) {
        await api(`/api/products/${editing.code}`, {
          method: 'PUT',
          body: JSON.stringify(p)
        });
        addToast('Product updated');
      } else {
        await api('/api/products', { method: 'POST', body: JSON.stringify(p) });
        addToast('Product created');
      }
      setShowForm(false);
      setEditing(null);
      load();
    } catch (e: any) {
      addToast(e.message || 'Operation failed', 'error');
    }
  };

  const confirmDelete = (code: string) => setDeleteCode(code);

  const handleDelete = async () => {
    if (!deleteCode) return;
    try {
      await api(`/api/products/${deleteCode}`, { method: 'DELETE' });
      addToast('Product deleted');
      setDeleteCode(null);
      load();
    } catch {
      addToast('Delete failed', 'error');
    }
  };

  return (
    <div>
      <h2>Products</h2>
      <button onClick={() => { setShowForm(true); setEditing(null); }}>Add Product</button>
      {showForm && (
        <ProductForm
          initial={editing || undefined}
          onSave={handleSave}
          onCancel={() => { setShowForm(false); setEditing(null); }}
        />
      )}
      <ul>
        {products.map(p => (
          <li key={p.code}>
            {p.code} - {p.name} ({p.quantity})
            <button onClick={() => { setEditing(p); setShowForm(true); }}>Edit</button>
            <button onClick={() => confirmDelete(p.code)}>Delete</button>
          </li>
        ))}
      </ul>
      <ConfirmModal
        open={deleteCode !== null}
        message="Delete this product?"
        onConfirm={handleDelete}
        onCancel={() => setDeleteCode(null)}
      />
    </div>
  );
};

export default Products;
