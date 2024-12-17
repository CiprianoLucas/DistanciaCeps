FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY ./back ./
COPY ./build/.env ./
RUN dotnet restore && dotnet build && dotnet tool restore
RUN dotnet tool install --global dotnet-ef
RUN export PATH="$PATH:/root/.dotnet/tools"
RUN chmod +x entrypoint.sh
EXPOSE 7000

ENTRYPOINT ["/app/entrypoint.sh"]