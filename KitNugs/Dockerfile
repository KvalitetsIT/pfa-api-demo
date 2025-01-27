#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8080 8081

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["KitNugs/KitNugs.csproj", "KitNugs/"]
RUN dotnet restore "KitNugs/KitNugs.csproj"
COPY . .
WORKDIR "/src/KitNugs"
RUN dotnet build "KitNugs.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KitNugs.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

# Use port 8080
EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080

# non-root
RUN addgroup --group app --gid 11000 && adduser --uid 11001 --gid 11000 "app" 
RUN chown app:app  /app
USER app:app 

#enable read-only filesystem
ENV COMPlus_EnableDiagnostics=0 

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KitNugs.dll"]