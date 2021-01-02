import { createStyles, makeStyles, Theme } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import { DataImport } from './components/data-import';
import { PortfolioTable } from './components/portfolio-table';
import { StatsSummary } from './components/stats-summary';
import { Portfolio } from './models/portfolio';
import { getTableData } from './services/api-service';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    App: {
      backgroundColor: "#efefef",
    }
  })
);

function App() {
  const classes = useStyles();

  const [portfolio, setPortfolio] = useState<Portfolio>();

  useEffect(() => {
    console.log("us")
    getTableData().then(res => setPortfolio(res));
  }, []);

  return (
    <div className={classes.App}>

      {portfolio && (
        <StatsSummary
          portfolio={portfolio}
        />
      )}

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