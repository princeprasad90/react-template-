import React from 'react';
import { Routes, Route, Navigate, useNavigate } from 'react-router-dom';
import Login from './components/Login';
import ChangePassword from './components/ChangePassword';
import { apiPost } from './lib/apiClient';
import { useAppState } from './lib/state';

const App: React.FC = () => {
  const { state, dispatch } = useAppState();
  const navigate = useNavigate();

  const handleLogout = () => {
    apiPost('/api/auth/logout', {}).finally(() => {
      dispatch({ type: 'logout' });
      navigate('/login');
    });
  };

  const loggedIn = !!state.user;

  return (
    <div>
      {loggedIn && (
        <button onClick={handleLogout}>Logout</button>
      )}
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/change-password" element={loggedIn ? <ChangePassword /> : <Navigate to="/login" />} />
        <Route path="/" element={<Navigate to="/login" />} />
      </Routes>
    </div>
  );
};

export default App;
