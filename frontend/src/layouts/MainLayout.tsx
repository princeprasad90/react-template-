import React, { ReactNode } from 'react';

interface Props {
  children: ReactNode;
}

function MainLayout({ children }: Props): JSX.Element {
  return (
    <div>
      <header>Sample App</header>
      <main>{children}</main>
    </div>
  );
}

export default MainLayout;
