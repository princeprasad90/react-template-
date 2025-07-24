import React from 'react';

const ChangePassword: React.FC = () => {
  const [currentPassword, setCurrentPassword] = React.useState('');
  const [newPassword, setNewPassword] = React.useState('');
  const [error, setError] = React.useState('');
  const [success, setSuccess] = React.useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    const res = await fetch('/api/auth/change-password', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ currentPassword, newPassword })
    });
    if (res.ok) {
      setSuccess(true);
    } else {
      setError('Could not change password');
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <div>
        <label>Current Password</label>
        <input type="password" value={currentPassword} onChange={e => setCurrentPassword(e.target.value)} />
      </div>
      <div>
        <label>New Password</label>
        <input type="password" value={newPassword} onChange={e => setNewPassword(e.target.value)} />
      </div>
      {error && <div style={{color:'red'}}>{error}</div>}
      {success && <div>Password changed</div>}
      <button type="submit">Change Password</button>
    </form>
  );
};

export default ChangePassword;
