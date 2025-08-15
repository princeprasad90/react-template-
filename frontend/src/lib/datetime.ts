import { format, parseISO } from 'date-fns';

/**
 * Format a date into a string using date-fns.
 * @param date A Date object or ISO date string.
 * @param pattern Formatting pattern, defaults to 'yyyy-MM-dd'.
 */
export function formatDate(date: Date | string, pattern = 'yyyy-MM-dd'): string {
  const d = typeof date === 'string' ? parseISO(date) : date;
  return format(d, pattern);
}
