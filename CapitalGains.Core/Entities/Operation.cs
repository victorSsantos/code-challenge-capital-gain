using CapitalGains.Core.Entities.Enums;

namespace CapitalGains.Core.Entities;

public class Operation
{
    public OperationType OperationType { get; }
    public double UnitCost { get; }
    public int Quantity { get; }
    public double Tax { get; set; }

    public Operation(OperationType operationType, double unitCost, int quantity, double tax = 0.0)
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
