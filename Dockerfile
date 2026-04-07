# ---------- Build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore QuantityMeasurementApi/QuantityMeasurementApi.csproj
RUN dotnet publish QuantityMeasurementApi/QuantityMeasurementApi.csproj -c Release -o /app/publish

# ---------- Runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["sh", "-c", "dotnet QuantityMeasurementApi.dll --urls http://0.0.0.0:$PORT"]