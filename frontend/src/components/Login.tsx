import React from 'react';
import { useNavigate } from 'react-router-dom';
import { apiPost } from '../lib/apiClient';
import { useAppState } from '../lib/state';
import { useForm } from '../lib/form';

const Login: React.FC = () => {
  const { dispatch } = useAppState();
  const navigate = useNavigate();

  const { values, errors, handleChange, handleSubmit } = useForm(
    { username: '', password: '' },
    v => {
      const errs: any = {};
      if (!v.username) errs.username = 'Required';
      if (!v.password) errs.password = 'Required';
      return errs;
    }
  );

  const onSubmit = async (vals: typeof values) => {
    try {
      await apiPost('/api/auth/login', vals);
      dispatch({ type: 'login', user: vals.username });
      navigate('/change-password');
    } catch (e) {
      dispatch({ type: 'logout' });
      alert('Invalid credentials');
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <div>
        <label>Username</label>
        <input name="username" value={values.username} onChange={handleChange} />
        {errors.username && <div style={{color:'red'}}>{errors.username}</div>}
      </div>
      <div>
        <label>Password</label>
        <input name="password" type="password" value={values.password} onChange={handleChange} />
        {errors.password && <div style={{color:'red'}}>{errors.password}</div>}
      </div>
      <button type="submit">Login</button>
    </form>
  );
};

export default Login;
