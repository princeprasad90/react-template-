import React from 'react';
import { AsyncPaginate } from 'react-select-async-paginate';

const loadOptions = async (inputValue: string, _loaded: any, additional: any) => {
  const page = additional.page || 1;
  const res = await fetch(`/api/items?page=${page}&pageSize=10&search=${encodeURIComponent(inputValue)}`);
  const data = await res.json();
  return {
    options: data.items.map((i: any) => ({ value: i.id, label: i.name })),
    hasMore: page * 10 < data.totalCount,
    additional: { page: page + 1 }
  };
};

function PaginatedSelect(): JSX.Element {
  return <AsyncPaginate loadOptions={loadOptions} additional={{ page: 1 }} />;
}

export default PaginatedSelect;
