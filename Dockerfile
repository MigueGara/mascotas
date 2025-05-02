# Usar la imagen oficial de .NET como base
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

# Usar la imagen oficial de .NET SDK para compilar la aplicación
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY SistemaAdopcionMascotas/SistemaAdopcionMascotas.csproj ./
RUN dotnet restore "SistemaAdopcionMascotas.csproj"
COPY . .
WORKDIR "/src/SistemaAdopcionMascotas"
RUN dotnet build "SistemaAdopcionMascotas.csproj" -c Release -o /app/build

# Publicar la aplicación
FROM build AS publish
RUN dotnet publish "SistemaAdopcionMascotas.csproj" -c Release -o /app/publish

# Configurar el entorno de producción
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SistemaAdopcionMascotas.dll"]