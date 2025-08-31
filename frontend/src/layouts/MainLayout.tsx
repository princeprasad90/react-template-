import React, { ReactNode } from 'react';

interface Props {
  children: ReactNode;
}

const MainLayout: React.FC<Props> = ({ children }) => (
  <div>
    <header>Sample App</header>
    <main>{children}</main>
  </div>
);

export default MainLayout;
