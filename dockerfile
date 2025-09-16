FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY matrix.sln ./
COPY Calculator.Api/Calculator.Api.csproj Calculator.Api/
COPY Calculator.Application/Calculator.Application.csproj Calculator.Application/
COPY Calculator.Domain/Calculator.Domain.csproj Calculator.Domain/
COPY Calculator.Infrastructure/Calculator.Infrastructure.csproj Calculator.Infrastructure/
COPY Calculator.Tests/Calculator.Tests.csproj Calculator.Tests/

RUN dotnet restore

COPY . .
RUN dotnet publish Calculator.Api/Calculator.Api.csproj -c Release -o /app /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

EXPOSE 5029
EXPOSE 7008

ENV ASPNETCORE_URLS=http://+:5029

ENV Jwt__Issuer=CalculatorApi
ENV Jwt__Audience=CalculatorApiUsers
ENV Jwt__Key=91cd14eab4046f9229e04df55cb2d29b565af53aec277e203e59d261cccd995f

COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Calculator.Api.dll"]