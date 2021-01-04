import { createStyles, makeStyles, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, TableSortLabel, Theme, withStyles } from "@material-ui/core";
import React from "react";
import { Holding, Portfolio } from "../models/portfolio";
import { ColoredNumber } from "./colored-number";

interface Props {
  portfolio: Portfolio
}

const StyledTableCell = withStyles((theme: Theme) =>
  createStyles({
    head: {
      backgroundColor: theme.palette.info.dark,
      color: theme.palette.common.white,
      lineHeight: "normal"
    },
    body: {
      fontSize: 12,
      lineHeight: "normal"
    },
  }),
)(TableCell);

const StyledTableRow = withStyles((theme: Theme) =>
  createStyles({
    root: {
      '&:nth-of-type(odd)': {
        backgroundColor: theme.palette.action.hover,
      },
    },
  }),
)(TableRow);

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      width: '100%',
    },
    paper: {
      width: '100%',
      marginBottom: theme.spacing(1),
      marginTop: theme.spacing(1)
    },
    table: {
      minWidth: 250,
    },
    visuallyHidden: {
      border: 0,
      clip: 'rect(0 0 0 0)',
      height: 1,
      margin: -1,
      overflow: 'hidden',
      padding: 0,
      position: 'absolute',
      top: 20,
      width: 1,
    },
    th: {
      lineHeight: "normal",
      fontSize: "0.75rem"
    }
  }),
);

function descendingComparator<T>(a: T, b: T, orderBy: keyof T) {
  if (b[orderBy] < a[orderBy]) {
    return -1;
  }
  if (b[orderBy] > a[orderBy]) {
    return 1;
  }
  return 0;
}

type Order = 'asc' | 'desc';

function getComparator<Key extends keyof any>(
  order: Order,
  orderBy: Key,
): (a: { [key in Key]: number | string | Date }, b: { [key in Key]: number | string | Date }) => number {
  return order === 'desc'
    ? (a, b) => descendingComparator(a, b, orderBy)
    : (a, b) => -descendingComparator(a, b, orderBy);
}

function stableSort<T>(array: T[], comparator: (a: T, b: T) => number) {
  const stabilizedThis = array.map((el, index) => [el, index] as [T, number]);
  stabilizedThis.sort((a, b) => {
    const order = comparator(a[0], b[0]);
    if (order !== 0) return order;
    return a[1] - b[1];
  });
  return stabilizedThis.map((el) => el[0]);
}

interface HeadCell {
  disablePadding: boolean;
  id: keyof Holding;
  label: string;
  numeric: boolean;
}

interface EnhancedTableProps {
  classes: ReturnType<typeof useStyles>;
  onRequestSort: (event: React.MouseEvent<unknown>, property: keyof Holding) => void;
  order: Order;
  orderBy: string;
}

const headCells: HeadCell[] = [
  { id: 'name', numeric: false, disablePadding: true, label: 'Name' },
  { id: 'price', numeric: true, disablePadding: false, label: 'Price (DKK)' },
  { id: 'changeToday', numeric: true, disablePadding: false, label: 'Change' },
  { id: 'totalValue', numeric: true, disablePadding: false, label: 'Value (DKK)' },
  { id: 'returnPercentage', numeric: true, disablePadding: false, label: 'Return (DKK)' },
];

function SortingTableHead(props: EnhancedTableProps) {
  const { classes, order, orderBy, onRequestSort } = props;
  const createSortHandler = (property: keyof Holding) => (event: React.MouseEvent<unknown>) => {
    onRequestSort(event, property);
  };

  return (
    <TableHead>
      <TableRow>
        {headCells.map((headCell) => (
          <StyledTableCell
            key={headCell.id}
            align={headCell.numeric ? 'right' : 'left'}
            padding={headCell.disablePadding ? 'none' : 'default'}
            sortDirection={orderBy === headCell.id ? order : false}
          >
            <TableSortLabel
              active={orderBy === headCell.id}
              direction={orderBy === headCell.id ? order : 'asc'}
              onClick={createSortHandler(headCell.id)}
            >
              {headCell.label}
              {orderBy === headCell.id ? (
                <span className={classes.visuallyHidden}>
                  {order === 'desc' ? 'sorted descending' : 'sorted ascending'}
                </span>
              ) : null}
            </TableSortLabel>
          </StyledTableCell>
        ))}
      </TableRow>
    </TableHead>
  );
};

const getShortName = (holding: Holding) => {
  if (holding.name.includes("ISHARES"))
    return holding.symbol;

  if (holding.name.includes("Sparinvest"))
    return holding.symbol;

  return holding.name.slice(0, 11);
}

export function PortfolioTable(props: Props) {
  const classes = useStyles();
  const [order, setOrder] = React.useState<Order>('asc');
  const [orderBy, setOrderBy] = React.useState<keyof Holding>('name');

  const handleRequestSort = (event: React.MouseEvent<unknown>, property: keyof Holding) => {
    const isAsc = orderBy === property && order === 'asc';
    setOrder(isAsc ? 'desc' : 'asc');
    setOrderBy(property);
  };

  return (
    <Paper className={classes.paper}>
      <TableContainer>
        <Table
          className={classes.table}
          aria-labelledby="tableTitle"
          size={'small'}
          aria-label="enhanced table"
        >
          <SortingTableHead
            classes={classes}
            order={order}
            orderBy={orderBy}
            onRequestSort={handleRequestSort}
          />
          <TableBody>
            {stableSort(props.portfolio.currentHoldings, getComparator(order, orderBy))
              .map((row) => row.changeToday && (
            <StyledTableRow key={row.name}>
              <StyledTableCell component="th" scope="row">
                {getShortName(row)}
              </StyledTableCell>
              <StyledTableCell align="right">
                {row.price.toFixed(2)}
              </StyledTableCell>
              <StyledTableCell align="right">
                <ColoredNumber
                  value={row.changeToday}
                  formatted={false}
                  suffix={"%"}
                />
              </StyledTableCell>
              <StyledTableCell align="right">{row.totalValue?.toFixed(0)}</StyledTableCell>
              <StyledTableCell align="right">
                <ColoredNumber
                  value={row.returnPercentage}
                  formatted={false}
                  suffix={"%"}
                />
              </StyledTableCell>
            </StyledTableRow>
          ))}
          </TableBody>
      </Table>
    </TableContainer>

    </Paper>
  );
}