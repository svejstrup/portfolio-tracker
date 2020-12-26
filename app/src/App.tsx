import React from 'react';
import './App.css';
import { DataImport } from './components/data-import';
import { PortfolioTable } from './components/portfolio-table';

function App() {
  return (
    <div className="App">
      <PortfolioTable/>
      <DataImport/>
    </div>
  );
}

export default App;