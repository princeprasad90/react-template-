import React from 'react';
import { useNavigate } from 'react-router-dom';
import AppRoutes from './routes/AppRoutes';
import { AuthProvider, useAuth } from './store/auth';
import MainLayout from './layouts/MainLayout';
import { api } from './api';

const AppContent: React.FC = () => {
  const navigate = useNavigate();
  const { loggedIn, logout, user } = useAuth();

  const handleLogout = () => {
    api('/api/auth/logout', { method: 'POST' }).then(() => {
      logout();
      navigate('/login');
    });
  };

  return (
    <MainLayout>
      {loggedIn && (
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <span>{user ? `Welcome, ${user.name}` : 'Welcome'}</span>
          <button onClick={handleLogout}>Logout</button>
        </div>
      )}
      <AppRoutes />
    </MainLayout>
  );
};

const App: React.FC = () => (
  <AuthProvider>
    <AppContent />
  </AuthProvider>
);

export default App;
