import { Portfolio } from "../models/portfolio";

export async function getTableData() {
    const response = await fetch('/api/portfolio/GetTableData');
    let portfolio = (await response.json()) as Portfolio;

    return portfolio;
}

export async function uploadCsvFile(file: FormData) {
    try {
        const response = await fetch('/api/CsvImport/ImportTransactions', {
            method: "POST",
            body: file
        });

        await response.json();

        return true; 
    } catch {
        return false;
    }
    
}