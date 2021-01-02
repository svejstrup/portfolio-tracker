export interface Portfolio {
    name?: string
    currentValue?: number
    totalReturn?: number
    totalReturnPercentage?: number
    todaysReturn?: number
    todaysReturnPercentage?: number
    currentHoldings: Holding[]
    previousHoldings: Holding[]
}

export interface Holding {
    symbol: string 
    name: string 
    currency: string 
    price: number
    changeToday: number
    buyPrice: number
    amountOwned: number 
    buyDate: Date
    totalChange: number
    totalValue: number
    buyValue: number
    returnValue: number
    returnPercentage: number
}