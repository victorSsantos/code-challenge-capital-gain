using CapitalGains.Domain.Entities.Enums;

namespace CapitalGains.Domain.Entities;

public class Operation
{
    public OperationType OperationType { get; }
    public decimal UnitCost { get; }
    public int Quantity { get; }
    public decimal Tax { get; set; }

    public Operation(OperationType operationType, decimal unitCost, int quantity, decimal tax = 0.0M)
    {
        if (unitCost <= 0)
            throw new ArgumentException("Unit Cost must be a number greater than 0.");

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be a number greater than 0.");

        if (tax < 0)
            throw new ArgumentException("Tax must be a number greater or equal to 0.");

        OperationType = operationType;
        UnitCost = unitCost;
        Quantity = quantity;
        Tax = tax;
    }
}
