import React from 'react';
import PaginatedSelect from '../components/PaginatedSelect';
import ServerGrid from '../components/ServerGrid';

function Examples(): JSX.Element {
  return (
    <div>
      <h2>Async Select</h2>
      <PaginatedSelect />
      <h2>Data Grid</h2>
      <ServerGrid />
    </div>
  );
}

export default Examples;
