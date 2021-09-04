import { Button, createStyles, Grid, makeStyles, Paper, Theme, CircularProgress } from "@material-ui/core";
import React, { useState } from "react";
import { uploadCsvFile } from "../services/api-service";

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    container: {
        padding: theme.spacing(1),
    },
    marginLeft: {
        marginLeft: theme.spacing(1)
    }
  })
);

export function DataImport() {
    const classes = useStyles();
    const [file, setFile] = useState<File | null>();
    const [isLoading, setIsLoading] = useState<boolean>(false);

    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setFile(event.target.files?.item(0));
    }

    const handleUpload = async () => {
        let formData = new FormData()
        formData.append(file!.name, file!)

        setIsLoading(true);

        await uploadCsvFile(formData);

        setIsLoading(false);
    }

    const navigateToNewTransaction = () => {
        console.log("New transaction");
        setIsLoading(!isLoading);
    }

    return (
        <>
            {isLoading ? <CircularProgress /> : (
                <Paper className={classes.container}>
                    <Grid container spacing={1}>
                        <Grid item xs>
                            <div>
                                <input type="file" onChange={handleFileChange}></input>
                                <Button 
                                    className={classes.marginLeft}
                                    color="primary" 
                                    variant="contained" 
                                    disabled={file === null || file === undefined}
                                    onClick={handleUpload}
                                >
                                    IMPORT
                                </Button>
                            </div>
                        </Grid>
                    </Grid>
                    <Grid container spacing={1}>
                        <Grid item xs>
                            <div>
                                <Button
                                    color="primary" 
                                    variant="contained"
                                    onClick={navigateToNewTransaction}
                                >
                                    Add Transaction 
                                </Button>
                            </div>
                        </Grid>
                    </Grid>
                </Paper>
            )}
        </>
    )
}