FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80


FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["ServiceSystemNRDCL.csproj", "./"]
RUN dotnet restore "ServiceSystemNRDCL.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ServiceSystemNRDCL.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ServiceSystemNRDCL.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ServiceSystemNRDCL.dll"]
