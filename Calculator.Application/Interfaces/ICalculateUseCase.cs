namespace Calculator.Application.UseCases;

public interface ICalculateUseCase
{
    double Execute(double a, double b, string operation);
}
