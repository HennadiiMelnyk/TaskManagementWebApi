FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["TaskManagementWebApi/TaskManagementWebApi.csproj", "TaskManagementWebApi/"]
RUN dotnet restore "TaskManagementWebApi/TaskManagementWebApi.csproj"
COPY . .
WORKDIR "/src/TaskManagementWebApi"
RUN dotnet build "TaskManagementWebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskManagementWebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskManagementWebApi.dll"]
