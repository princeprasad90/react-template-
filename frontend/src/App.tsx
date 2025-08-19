import React from 'react';
import { Routes, Route, Navigate, useNavigate } from 'react-router-dom';
import Login from './components/Login';
import ChangePassword from './components/ChangePassword';
import Examples from './components/Examples';
import PromoCodes from './components/PromoCodes';

const App: React.FC = () => {
  const [loggedIn, setLoggedIn] = React.useState(false);
  const navigate = useNavigate();

  const handleLogout = () => {
    fetch('/api/auth/logout', { method: 'POST' }).then(() => {
      setLoggedIn(false);
      navigate('/login');
    });
  };

  return (
    <div>
      {loggedIn && (
        <button onClick={handleLogout}>Logout</button>
      )}
      <Routes>
        <Route path="/login" element={<Login onLogin={() => setLoggedIn(true)} />} />
        <Route path="/change-password" element={loggedIn ? <ChangePassword /> : <Navigate to="/login" />} />
        <Route path="/examples" element={loggedIn ? <Examples /> : <Navigate to="/login" />} />
        <Route path="/promocodes" element={loggedIn ? <PromoCodes /> : <Navigate to="/login" />} />
        <Route path="/" element={<Navigate to="/login" />} />
      </Routes>
    </div>
  );
};

export default App;
