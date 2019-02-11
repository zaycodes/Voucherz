FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["VoucherService/VoucherService.csproj", "VoucherService/"]
RUN dotnet restore "VoucherService/VoucherService.csproj"
COPY . .
WORKDIR "/src/VoucherService"
RUN dotnet build "VoucherService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "VoucherService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VoucherService.dll"]