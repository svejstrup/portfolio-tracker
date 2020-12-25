import { useState, useEffect } from "react";
import { getTableData } from "../services/api-service";

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
      <button onClick={() => setCount(count + 1)}>
        Click me
      </button>
      <button onClick={getData}>Api call</button>
      <p>{response}</p>
    </div>
  );
}