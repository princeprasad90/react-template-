import React from 'react';
import { useNavigate } from 'react-router-dom';
import AppRoutes from './routes/AppRoutes';
import { AuthProvider, useAuth } from './store/auth';
import MainLayout from './layouts/MainLayout';
import { ToastProvider } from './components/Toast';

function AppContent(): JSX.Element {
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
}

function App(): JSX.Element {
  return (
    <AuthProvider>
      <ToastProvider>
        <AppContent />
      </ToastProvider>
    </AuthProvider>
  );
}

export default App;
