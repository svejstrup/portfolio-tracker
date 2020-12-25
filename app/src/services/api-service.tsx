export async function getTableData() {
    const response = await fetch('/api/GetTableData');
    let res = await response.text();
    return res;
}