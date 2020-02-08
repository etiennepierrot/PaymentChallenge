FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /build
COPY . .
RUN dotnet restore
COPY . .
RUN dotnet test ./test/PaymentChallenge.Tests/PaymentChallenge.Tests.csproj

RUN dotnet publish ./src/PaymentChallenge.WebApi/PaymentChallenge.WebApi.csproj -o /publish

WORKDIR /publish
 
ENV ASPNETCORE_URLS="http://0.0.0.0:5000"
 
ENTRYPOINT ["dotnet", "PaymentChallenge.WebApi.dll"]