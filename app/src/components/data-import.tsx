import { Button } from "@material-ui/core";
import { useState } from "react";
import { uploadCsvFile } from "../services/api-service";

export function DataImport() {

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
        <div>
            <input type="file" onChange={handleFileChange}></input>
            <Button 
                color="primary" 
                variant="contained" 
                disabled={file === null || file === undefined}
                onClick={handleUpload}
            >
                IMPORT
            </Button>
        </div>
    )
}