export async function getTableData() {
    const response = await fetch('/api/portfolio/GetTableData');
    let res = await response.text();
    return res;
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