import React from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import Login from '../features/auth/Login';
import ChangePassword from '../features/auth/ChangePassword';
import Examples from '../pages/Examples';
import PromoCodes from '../pages/PromoCodes';
import Products from '../pages/Products';
import { useAuth } from '../store/auth';

function AppRoutes(): JSX.Element {
  const { loggedIn } = useAuth();

  return (
    <Routes>
      <Route path="/login" element={<Login />} />
      <Route path="/change-password" element={loggedIn ? <ChangePassword /> : <Navigate to="/login" />} />
      <Route path="/examples" element={loggedIn ? <Examples /> : <Navigate to="/login" />} />
      <Route path="/promocodes" element={loggedIn ? <PromoCodes /> : <Navigate to="/login" />} />
      <Route path="/products" element={loggedIn ? <Products /> : <Navigate to="/login" />} />
      <Route path="/" element={<Navigate to="/login" />} />
    </Routes>
  );
}

export default AppRoutes;
