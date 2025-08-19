import React from 'react';
import PaginatedSelect from './PaginatedSelect';
import ServerGrid from './ServerGrid';

const Examples: React.FC = () => (
  <div>
    <h2>Async Select</h2>
    <PaginatedSelect />
    <h2>Data Grid</h2>
    <ServerGrid />
  </div>
);

export default Examples;
