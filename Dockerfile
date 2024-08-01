FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["InsecureEncryptionDemo.csproj", "./"]
RUN dotnet restore "InsecureEncryptionDemo.csproj"
COPY . .
RUN dotnet build "InsecureEncryptionDemo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InsecureEncryptionDemo.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8880
ENV ASPNETCORE_URLS=http://+:8880
ENTRYPOINT ["dotnet", "InsecureEncryptionDemo.dll"]