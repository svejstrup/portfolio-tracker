import { useState, useEffect } from "react";
import { getTableData } from "../services/api-service";
import Button from '@material-ui/core/Button';
import { Portfolio } from "../models/portfolio";

export function PortfolioTable() {
    const [count, setCount] = useState(0);
    const [portfolio, setPortfolio] = useState<Portfolio>();

  // Similar to componentDidMount and componentDidUpdate:
  useEffect(() => {
    // Update the document title using the browser API
    document.title = `You clicked ${count} times`;
  });

  const getData = async () => {
    let res = await getTableData();

    console.log(res);
    setPortfolio(res);
  }

  return (
    <div>
      <p>You clicked {count} times</p>
      <Button variant="contained" color="primary" onClick={() => setCount(count + 1)}>
        Click me
      </Button>
      <Button variant="contained" color="primary" onClick={getData}>Api call</Button>
      <p>{portfolio?.totalReturn}</p>
    </div>
  );
}