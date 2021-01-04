import { Button, createStyles, makeStyles, Paper, Theme } from "@material-ui/core";
import React, { useState } from "react";
import { uploadCsvFile } from "../services/api-service";

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    container: {
        padding: theme.spacing(1)
    }
  })
);

export function DataImport() {
    const classes = useStyles();
    const [file, setFile] = useState<File | null>()

    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setFile(event.target.files?.item(0));
    }

    const handleUpload = async () => {
        let formData = new FormData()
        formData.append(file!.name, file!)
        
        await uploadCsvFile(formData);
    }

    return (
        <Paper className={classes.container}>
            <input type="file" onChange={handleFileChange}></input>
            <Button 
                color="primary" 
                variant="contained" 
                disabled={file === null || file === undefined}
                onClick={handleUpload}
            >
                IMPORT
            </Button>
        </Paper>
    )
}