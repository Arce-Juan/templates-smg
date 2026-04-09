# Plantilla: publica Template.Api y sus dependencias (Application, Domain, Infrastructure).
# Uso: docker compose build api
#
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Template.Api/Template.Api.csproj", "Template.Api/"]
COPY ["Template.Application/Template.Application.csproj", "Template.Application/"]
COPY ["Template.Domain/Template.Domain.csproj", "Template.Domain/"]
COPY ["Template.Infrastructure/Template.Infrastructure.csproj", "Template.Infrastructure/"]

RUN dotnet restore "Template.Api/Template.Api.csproj"
COPY . .
RUN dotnet publish "Template.Api/Template.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Run stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS run
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Template.Api.dll"]
