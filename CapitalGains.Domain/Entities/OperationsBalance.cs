namespace CapitalGains.Domain.Entities;

public  class OperationsBalance
{
    public int TotalQuantity { get; set; } = 0;
    public decimal AverageBuyingCost { get; set; } = 0.0M;
    public decimal Loss { get; set; } = 0.0M;
}
