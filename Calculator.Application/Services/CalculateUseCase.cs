using Calculator.Domain.Interfaces;

namespace Calculator.Application.UseCases;

public sealed class CalculateUseCase(ICalculator calculator) : ICalculateUseCase
{
    private readonly ICalculator _calculator = calculator;

    public double Execute(double a, double b, string operation) =>
        _calculator.Calculate(a, b, operation);
}
