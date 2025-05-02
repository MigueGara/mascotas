# Etapa base de runtime con ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

# Etapa de build con SDK
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar solo el archivo del proyecto primero (mejora la cache de Docker)
COPY SistemaAdopcionMascotas.csproj ./
RUN dotnet restore

# Luego copiar el resto del código
COPY . ./

# Compilar en modo Release
RUN dotnet build -c Release -o /app/build

# Etapa de publicación
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Imagen final de runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Comando de arranque
ENTRYPOINT ["dotnet", "SistemaAdopcionMascotas.dll"]
