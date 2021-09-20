FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

# Copy everything and build
COPY . ./
RUN dotnet publish -c Release -o out

# build runtime
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .
COPY ./areas.sqlite3 .
EXPOSE 5000
EXPOSE 5001
EXPOSE 80
ENTRYPOINT ["dotnet", "emsiproject.dll"]