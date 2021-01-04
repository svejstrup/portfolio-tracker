import { CircularProgress } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import { DataImport } from './components/data-import';
import { PortfolioTable } from './components/portfolio-table';
import { StatsSummary } from './components/stats-summary';
import { Portfolio } from './models/portfolio';
import { getTableData } from './services/api-service';

function App() {
  const [portfolio, setPortfolio] = useState<Portfolio>();

  useEffect(() => {
    getTableData().then(res => setPortfolio(res));
  }, []);

  return (
    <>
      {!portfolio ? <CircularProgress /> : (
        <>
          <StatsSummary portfolio={portfolio} />
          <PortfolioTable portfolio={portfolio} />
        </>
      )}

      <DataImport/>
    </>
  );
}

export default App;