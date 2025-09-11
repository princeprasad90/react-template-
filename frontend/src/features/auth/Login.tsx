import React from 'react';
import * as yup from 'yup';
import { useFormCommand } from '../../hooks/useFormCommand';
import { validateRegex } from '../../utils/validation';
import { useAuth } from '../../store/auth';

const Login: React.FC = () => {
  const { login } = useAuth();
  const [username, setUsername] = React.useState('');
  const [password, setPassword] = React.useState('');
  const [error, setError] = React.useState<string | null>(null);

  const submit = useFormCommand({
    schema: yup.object({
      username: yup
        .string()
        .required('Username is required')
        .test('alphanumeric', 'Username must be alphanumeric', (v) =>
          !validateRegex(v || '', /^[a-zA-Z0-9]+$/, '')
        ),
      password: yup.string().required('Password is required')
    }),
    api: async (values: { username: string; password: string }) => {
      const res = await fetch('/api/auth/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(values)
      });
      if (!res.ok) throw new Error('Invalid credentials');
      return res.json();
    },
    onSuccess: (tokens: { accessToken: string; refreshToken: string }) =>
      login(tokens.accessToken, tokens.refreshToken)
  });

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    const { errors } = await submit({ username, password });
    if (errors) {
      setError(Array.isArray(errors) ? errors.join(', ') : String(errors));
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <div>
        <label>Username</label>
        <input value={username} onChange={e => setUsername(e.target.value)} />
      </div>
      <div>
        <label>Password</label>
        <input type="password" value={password} onChange={e => setPassword(e.target.value)} />
      </div>
      {error && <div style={{color:'red'}}>{error}</div>}
      <button type="submit">Login</button>
    </form>
  );
};

export default Login;
