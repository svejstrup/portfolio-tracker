import { createStyles, makeStyles, Theme } from "@material-ui/core";

interface Props
{
    value: number,
    suffix?: string,
    prefix?: string,
    formatted?: boolean,
    precision?: number
}

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    red: {
        color: "rgb(217, 83, 79)"
    },
    green: {
        color: "green"
    },
  })
);

export function ColoredNumber(props: Props)
{
    const classes = useStyles(props);
    let formattedValue = props.formatted ? props.value : ((props.value - 1) * 100)
    const colorClass = formattedValue >= 0 ? classes.green : classes.red;

    formattedValue = isNaN(formattedValue) ? 0 : formattedValue;

    return (
        <div className={colorClass}>
            {props.prefix}{formattedValue.toFixed(props.precision ?? 2)} {props.suffix}
        </div>
    )
}