import React from 'react';
import Select from 'react-select';

export interface Option {
  value: string;
  label: string;
}

interface Props {
  options: Option[];
  onChange?: (value: Option | null) => void;
}

export function SelectDropdown(props: Props) {
  return <Select {...props} />;
}
