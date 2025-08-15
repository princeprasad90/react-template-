import React, { createContext, useContext, useReducer } from 'react';

interface State {
  user?: string;
}

type Action =
  | { type: 'login'; user: string }
  | { type: 'logout' };

const StateContext = createContext<{
  state: State;
  dispatch: React.Dispatch<Action>;
} | undefined>(undefined);

function reducer(state: State, action: Action): State {
  switch (action.type) {
    case 'login':
      return { ...state, user: action.user };
    case 'logout':
      return { ...state, user: undefined };
    default:
      return state;
  }
}

export const AppStateProvider: React.FC<React.PropsWithChildren> = ({ children }) => {
  const [state, dispatch] = useReducer(reducer, {});
  return (
    <StateContext.Provider value={{ state, dispatch }}>
      {children}
    </StateContext.Provider>
  );
};

export function useAppState() {
  const ctx = useContext(StateContext);
  if (!ctx) {
    throw new Error('useAppState must be used within AppStateProvider');
  }
  return ctx;
}
