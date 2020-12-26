import { useState, useEffect } from "react";
import { getTableData } from "../services/api-service";
import Button from '@material-ui/core/Button';

export function PortfolioTable() {
    const [count, setCount] = useState(0);
    const [response, setResponse] = useState("");

  // Similar to componentDidMount and componentDidUpdate:
  useEffect(() => {
    // Update the document title using the browser API
    document.title = `You clicked ${count} times`;
  });

  const getData = async () => {
    let res = await getTableData();

    console.log(res);

    setResponse(res);
  }

  return (
    <div>
      <p>You clicked {count} times</p>
      <Button variant="contained" color="primary" onClick={() => setCount(count + 1)}>
        Click me
      </Button>
      <Button variant="contained" color="primary" onClick={getData}>Api call</Button>
      <p>{response}</p>
    </div>
  );
}