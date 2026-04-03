FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore "src/Gateways/AICalorieCounter.Bff.Api/AICalorieCounter.Bff.Api.csproj"
RUN dotnet publish "src/Gateways/AICalorieCounter.Bff.Api/AICalorieCounter.Bff.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "AICalorieCounter.Bff.Api.dll"]
