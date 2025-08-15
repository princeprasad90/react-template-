import React from 'react';
import { apiPost } from '../lib/apiClient';
import { useForm } from '../lib/form';

const ChangePassword: React.FC = () => {
  const [success, setSuccess] = React.useState(false);

  const { values, errors, handleChange, handleSubmit } = useForm(
    { currentPassword: '', newPassword: '' },
    v => {
      const errs: any = {};
      if (!v.currentPassword) errs.currentPassword = 'Required';
      if (!v.newPassword) errs.newPassword = 'Required';
      return errs;
    }
  );

  const onSubmit = async (vals: typeof values) => {
    try {
      await apiPost('/api/auth/change-password', vals);
      setSuccess(true);
    } catch {
      alert('Could not change password');
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <div>
        <label>Current Password</label>
        <input
          name="currentPassword"
          type="password"
          value={values.currentPassword}
          onChange={handleChange}
        />
        {errors.currentPassword && <div style={{ color: 'red' }}>{errors.currentPassword}</div>}
      </div>
      <div>
        <label>New Password</label>
        <input
          name="newPassword"
          type="password"
          value={values.newPassword}
          onChange={handleChange}
        />
        {errors.newPassword && <div style={{ color: 'red' }}>{errors.newPassword}</div>}
      </div>
      {success && <div>Password changed</div>}
      <button type="submit">Change Password</button>
    </form>
  );
};

export default ChangePassword;
