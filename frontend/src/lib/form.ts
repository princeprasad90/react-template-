import React from 'react';

export type Errors<T> = Partial<Record<keyof T, string>>;

export function useForm<T>(initialValues: T, validate: (values: T) => Errors<T>) {
  const [values, setValues] = React.useState<T>(initialValues);
  const [errors, setErrors] = React.useState<Errors<T>>({});

  function handleChange(e: React.ChangeEvent<HTMLInputElement>) {
    const { name, value } = e.target;
    setValues(v => ({ ...v, [name]: value }));
  }

  function handleSubmit(onSubmit: (values: T) => Promise<void>) {
    return async (e: React.FormEvent) => {
      e.preventDefault();
      const validation = validate(values);
      setErrors(validation);
      if (Object.keys(validation).length === 0) {
        await onSubmit(values);
      }
    };
  }

  return { values, errors, handleChange, handleSubmit };
}
