import * as yup from 'yup';

/**
 * Validate data using a Yup schema.
 * @param schema Yup schema to validate against.
 * @param data The data to validate.
 */
export function validate<T>(schema: yup.Schema<T>, data: unknown): Promise<T> {
  return schema.validate(data);
}
