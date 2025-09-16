import React, { useEffect } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import { useAuth } from '../../store/auth';

const AuthRedirect: React.FC = () => {
  const [params] = useSearchParams();
  const navigate = useNavigate();
  const { login } = useAuth();

  useEffect(() => {
    const token = params.get('token');
    const refresh = params.get('refreshToken');
    if (token && refresh) {
      void (async () => {
        try {
          await login(token, refresh);
          navigate('/examples');
        } catch {
          navigate('/login');
        }
      })();
    } else {
      navigate('/login');
    }
  }, [params, login, navigate]);

  return <div>Authenticating...</div>;
};

export default AuthRedirect;
