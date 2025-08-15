import React from 'react';
import {
  useTable,
  usePagination,
  useSortBy,
  useFilters,
  Column,
} from 'react-table';

interface DataTableProps<T extends object> {
  columns: Column<T>[];
  data: T[];
}

export function DataTable<T extends object>({ columns, data }: DataTableProps<T>) {
  const defaultColumn = React.useMemo(
    () => ({
      Filter: ({ column: { filterValue, setFilter } }: any) => (
        <input
          value={filterValue || ''}
          onChange={e => setFilter(e.target.value || undefined)}
          placeholder="Search..."
        />
      ),
    }),
    []
  );

  const {
    getTableProps,
    getTableBodyProps,
    headerGroups,
    prepareRow,
    page,
    nextPage,
    previousPage,
    canNextPage,
    canPreviousPage,
    state: { pageIndex },
  } = useTable(
    { columns, data, defaultColumn, initialState: { pageIndex: 0 } },
    useFilters,
    useSortBy,
    usePagination
  );

  return (
    <div>
      <table {...getTableProps()}>
        <thead>
          {headerGroups.map(headerGroup => (
            <tr {...headerGroup.getHeaderGroupProps()}>
              {headerGroup.headers.map(column => (
                <th {...column.getHeaderProps(column.getSortByToggleProps())}>
                  {column.render('Header')}
                  <div>{column.canFilter ? column.render('Filter') : null}</div>
                </th>
              ))}
            </tr>
          ))}
        </thead>
        <tbody {...getTableBodyProps()}>
          {page.map(row => {
            prepareRow(row);
            return (
              <tr {...row.getRowProps()}>
                {row.cells.map(cell => (
                  <td {...cell.getCellProps()}>{cell.render('Cell')}</td>
                ))}
              </tr>
            );
          })}
        </tbody>
      </table>
      <div>
        <button onClick={() => previousPage()} disabled={!canPreviousPage}>
          Previous
        </button>
        <span> Page {pageIndex + 1} </span>
        <button onClick={() => nextPage()} disabled={!canNextPage}>
          Next
        </button>
      </div>
    </div>
  );
}
