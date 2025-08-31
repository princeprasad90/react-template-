import React from 'react';

interface Props {
  open: boolean;
  message: string;
  onConfirm: () => void;
  onCancel: () => void;
}

function ConfirmModal({ open, message, onConfirm, onCancel }: Props): JSX.Element | null {
  if (!open) return null;
  return (
    <div className="modal-backdrop">
      <div className="modal">
        <p>{message}</p>
        <button onClick={onConfirm}>Yes</button>
        <button onClick={onCancel}>No</button>
      </div>
    </div>
  );
}

export default ConfirmModal;
