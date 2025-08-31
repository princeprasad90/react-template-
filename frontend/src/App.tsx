import React from 'react';
import { useNavigate } from 'react-router-dom';
import AppRoutes from './routes/AppRoutes';
import { AuthProvider, useAuth } from './store/auth';
import MainLayout from './layouts/MainLayout';
import { ToastProvider } from './components/Toast';

const AppContent: React.FC = () => {
  const navigate = useNavigate();
  const { loggedIn, logout } = useAuth();

  const handleLogout = () => {
    fetch('/api/auth/logout', { method: 'POST' }).then(() => {
      logout();
      navigate('/login');
    });
  };

  return (
    <MainLayout>
      {loggedIn && <button onClick={handleLogout}>Logout</button>}
      <AppRoutes />
    </MainLayout>
  );
};

const App: React.FC = () => (
  <AuthProvider>
    <ToastProvider>
      <AppContent />
    </ToastProvider>
  </AuthProvider>
);

export default App;
