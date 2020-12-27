import { useState, useEffect } from "react";
import { getTableData } from "../services/api-service";
import Button from '@material-ui/core/Button';
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