import { Button } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import './App.css';
import { DataImport } from './components/data-import';
import { PortfolioTable } from './components/portfolio-table';
import { Portfolio } from './models/portfolio';
import { getTableData } from './services/api-service';

function App() {
  const [portfolio, setPortfolio] = useState<Portfolio>();

  useEffect(() => {
    // Update the document title using the browser API
    // document.title = `You clicked ${count} times`;
  });

  const getData = async () => {
    let res = await getTableData();

    console.log(res);
    setPortfolio(res);
  }
  
  return (
    <div className="App">
      <Button variant="contained" color="primary" onClick={getData}>
        Fetch portfolio
      </Button>

      {portfolio && (
        <PortfolioTable
          key={portfolio.totalReturn}
          portfolio={portfolio}
        />
      )}
      <DataImport/>
    </div>
  );
}

export default App;