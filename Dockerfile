# Utilizar la imagen base de .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Establecer el directorio de trabajo
WORKDIR /app

# Copiar el proyecto .csproj y restaurar las dependencias
COPY Animalia/*.csproj ./ 
RUN dotnet restore

# Copiar el resto de los archivos y compilar la aplicación
COPY Animalia/. ./ 
RUN dotnet publish -c Release -o out

# Utilizar la imagen base de .NET Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
LABEL org.opencontainers.image.source="https://github.com/edwardapaza/shorten02"

# Establecer el directorio de trabajo
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
RUN apk add icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Copiar los archivos compilados desde la etapa de construcción
COPY --from=build /app/out .

# Definir el comando de entrada para ejecutar la aplicación
ENTRYPOINT ["dotnet", "Animalia.dll"]
