using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextStop.Common.Database;

public class QueryParameter(string name, object? value)
{
    public string Name { get; } = name;
    public object? Value { get; } = value;
}
