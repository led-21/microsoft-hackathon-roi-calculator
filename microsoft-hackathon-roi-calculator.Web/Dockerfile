# Stage 1: Build Blazor WebAssembly static assets
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["microsoft-hackathon-roi-calculator.Domain/microsoft-hackathon-roi-calculator.Domain.csproj", "microsoft-hackathon-roi-calculator.Domain/"]
COPY ["microsoft-hackathon-roi-calculator.Web/microsoft-hackathon-roi-calculator.Web.csproj", "microsoft-hackathon-roi-calculator.Web/"]

RUN dotnet restore "microsoft-hackathon-roi-calculator.Web/microsoft-hackathon-roi-calculator.Web.csproj"

COPY microsoft-hackathon-roi-calculator.Domain/ microsoft-hackathon-roi-calculator.Domain/
COPY microsoft-hackathon-roi-calculator.Web/ microsoft-hackathon-roi-calculator.Web/

WORKDIR "/src/microsoft-hackathon-roi-calculator.Web"
RUN dotnet publish "microsoft-hackathon-roi-calculator.Web.csproj" -c Release -o /app/publish

# Stage 2: Serve via Nginx
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

COPY --from=build /app/publish/wwwroot .
COPY microsoft-hackathon-roi-calculator.Web/nginx.conf /etc/nginx/nginx.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
