import { createStyles, Grid, makeStyles, Paper, Theme } from "@material-ui/core";
import React from "react";
import { Portfolio } from "../models/portfolio";
import { ColoredNumber } from "./colored-number";

interface Props {
    portfolio: Portfolio
}

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        root: {
            flexGrow: 1,
            marginTop: "10px"
        },
        paper: {
            backgroundColor: "#efefef",
            padding: theme.spacing(2),
            textAlign: 'center',
        },
        content: {
            // opacity: 1,
            // height: "30px"
        },
        item: {
            height: "70px"
        },
        paragraph: {
            fontVariant: "small-caps",
            fontSize: "0.6rem",
            opacity: 0.7
        },
        totalValue: {
            fontSize: "1.4rem"
        }
    }),
);

export function StatsSummary(props: Props) {

    const classes = useStyles();

    return (
        <div className={classes.root}>
            <Grid container spacing={1}>
                <Grid item xs>
                    <Paper className={classes.paper}>
                        <div className={classes.totalValue}>
                            <ColoredNumber
                                value={props.portfolio.todaysReturnPercentage!}
                                suffix={"%"}
                                formatted={false}
                            />
                        </div>
                        <div className={classes.paragraph}>
                            <ColoredNumber
                                value={props.portfolio.todaysReturn!}
                                formatted={true}
                                suffix={"DKK)"}
                                prefix={"("}
                            />
                        </div>
                    </Paper>
                </Grid>
            </Grid>
            <Grid container spacing={1}>
                <Grid item xs>
                    <Paper className={classes.paper}>
                        <div className={classes.content}>
                            <div className={classes.totalValue}>
                                <ColoredNumber
                                    value={props.portfolio.totalReturnPercentage!}
                                    suffix={"%"}
                                    formatted={false}
                                />
                            </div>
                            <div className={classes.paragraph}>
                                <ColoredNumber
                                    value={props.portfolio.totalReturn!}
                                    formatted={true}
                                    suffix={"DKK)"}
                                    prefix={"("}
                                    precision={0}
                                />
                            </div>
                        </div>
                    </Paper>
                </Grid>
                <Grid item xs>
                    <Paper className={classes.paper}>
                        <div className={classes.content}>
                            <div className={classes.totalValue}>
                                {props.portfolio.currentValue?.toFixed(0)} DKK
                            </div>
                            <div className={classes.paragraph}>
                                (Current portfolio value)
                            </div>
                        </div>
                    </Paper>
                </Grid>
            </Grid>
        </div>
    )
}