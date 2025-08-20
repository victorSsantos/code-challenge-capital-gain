using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapitalGains.Domain.Entities;

public class OrderOperarion
{
    public Guid Id { get; }
    public List<Operation> Operations { get; } = [];
}
