import { Portfolio } from "../models/portfolio";

interface Props {
  portfolio?: Portfolio
}

export function PortfolioTable(props: Props) {
  return (
    <div>
      <p>{props.portfolio?.totalReturn}</p>
    </div>
  );
}