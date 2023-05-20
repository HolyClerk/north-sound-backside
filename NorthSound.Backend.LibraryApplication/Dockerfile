#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["NorthSound.Backend.LibraryApplication/NorthSound.Backend.LibraryApplication.csproj", "NorthSound.Backend.LibraryApplication/"]
COPY ["NorthSound.Backend.DAL/NorthSound.Backend.DAL.csproj", "NorthSound.Backend.DAL/"]
COPY ["NorthSound.Backend.Domain/NorthSound.Backend.Domain.csproj", "NorthSound.Backend.Domain/"]
COPY ["NorthSound.Backend.Infrastructure/NorthSound.Backend.Infrastructure.csproj", "NorthSound.Backend.Infrastructure/"]
COPY ["NorthSound.Backend.Services/NorthSound.Backend.Services.csproj", "NorthSound.Backend.Services/"]
RUN dotnet restore "NorthSound.Backend.LibraryApplication/NorthSound.Backend.LibraryApplication.csproj"
COPY . .
WORKDIR "/src/NorthSound.Backend.LibraryApplication"
RUN dotnet build "NorthSound.Backend.LibraryApplication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NorthSound.Backend.LibraryApplication.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NorthSound.Backend.LibraryApplication.dll"]