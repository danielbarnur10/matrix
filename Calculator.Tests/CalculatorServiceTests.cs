using Calculator.Domain.Interfaces;
using Calculator.Infrastructure.Services;
using FluentAssertions;

public class CalculatorServiceTests
{
    private readonly ICalculator _svc = new BasicCalculator();

    [Theory]
    [InlineData(2, 3, "add", 5)]
    [InlineData(5, 2, "subtract", 3)]
    [InlineData(2, 4, "multiply", 8)]
    [InlineData(9, 3, "divide", 3)]
    public void Calculate_Should_Work(double a, double b, string op, double expected)
    {
        _svc.Calculate(a, b, op).Should().Be(expected);
    }

    [Fact]
    public void Divide_By_Zero_Should_Throw()
    {
        var act = () => _svc.Calculate(1, 0, "divide");
        act.Should().Throw<ArgumentException>().WithMessage("*zero*");
    }

    [Fact]
    public void Invalid_Operation_Should_Throw()
    {
        var act = () => _svc.Calculate(1, 1, "pow");
        act.Should().Throw<ArgumentException>().WithMessage("*Invalid operation*");
    }
}
