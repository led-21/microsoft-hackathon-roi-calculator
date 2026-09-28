# Stage 1: Build .NET 10 API
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["microsoft-hackathon-roi-calculator.Domain/microsoft-hackathon-roi-calculator.Domain.csproj", "microsoft-hackathon-roi-calculator.Domain/"]
COPY ["microsoft-hackathon-roi-calculator.Application/microsoft-hackathon-roi-calculator.Application.csproj", "microsoft-hackathon-roi-calculator.Application/"]
COPY ["microsoft-hackathon-roi-calculator.Persistence/microsoft-hackathon-roi-calculator.Persistence.csproj", "microsoft-hackathon-roi-calculator.Persistence/"]
COPY ["microsoft-hackathon-roi-calculator.ServiceDefaults/microsoft-hackathon-roi-calculator.ServiceDefaults.csproj", "microsoft-hackathon-roi-calculator.ServiceDefaults/"]
COPY ["microsoft-hackathon-roi-calculator.Api/microsoft-hackathon-roi-calculator.Api.csproj", "microsoft-hackathon-roi-calculator.Api/"]

RUN dotnet restore "microsoft-hackathon-roi-calculator.Api/microsoft-hackathon-roi-calculator.Api.csproj"

COPY microsoft-hackathon-roi-calculator.Domain/ microsoft-hackathon-roi-calculator.Domain/
COPY microsoft-hackathon-roi-calculator.Application/ microsoft-hackathon-roi-calculator.Application/
COPY microsoft-hackathon-roi-calculator.Persistence/ microsoft-hackathon-roi-calculator.Persistence/
COPY microsoft-hackathon-roi-calculator.ServiceDefaults/ microsoft-hackathon-roi-calculator.ServiceDefaults/
COPY microsoft-hackathon-roi-calculator.Api/ microsoft-hackathon-roi-calculator.Api/

WORKDIR "/src/microsoft-hackathon-roi-calculator.Api"
RUN dotnet publish "microsoft-hackathon-roi-calculator.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "microsoft-hackathon-roi-calculator.Api.dll"]
