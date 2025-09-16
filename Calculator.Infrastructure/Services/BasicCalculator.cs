using Calculator.Domain.Interfaces;

namespace Calculator.Infrastructure.Services;

public sealed class BasicCalculator : ICalculator
{
    public double Calculate(double a, double b, string operation)
    {
        return operation?.Trim().ToLowerInvariant() switch
        {
            "add" => a + b,
            "subtract" => a - b,
            "multiply" => a * b,
            "divide" => b == 0 ? throw new ArgumentException("Division by zero") : a / b,
            _ => throw new ArgumentException($"Invalid operation: {operation}"),
        };
    }
}
