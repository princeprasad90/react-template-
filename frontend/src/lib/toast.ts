import { toast } from 'react-toastify';

/** Show a success toast message. */
export function showSuccess(message: string): void {
  toast.success(message);
}

/** Show an error toast message. */
export function showError(message: string): void {
  toast.error(message);
}
