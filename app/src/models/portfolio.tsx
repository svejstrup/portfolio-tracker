export interface Portfolio {
    name?: string
    currentValue?: number
    totalReturn?: number
    totalReturnPercentage?: number
    currentHoldings: Holding[]
    previousHoldings: Holding[]
}

export interface Holding {
    Symbol: string 
    Name: string 
    Currency: string 
    Price: number
    ChangeToday?: number
    BuyPrice: number
    AmountOwned: number 
    BuyDate: Date
    TotalChange?: number
    TotalValue?: number
    BuyValue?: number
    ReturnValue?: number
    ReturnPercentage?: number
}