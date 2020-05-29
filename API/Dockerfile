#Build mvc-api
FROM mcr.microsoft.com/dotnet/core/sdk:2.1 as build-env
WORKDIR /api
COPY ./API/*.csproj ./
RUN dotnet restore

COPY ./API/. ./
RUN dotnet publish -c Release -o out

#Run api on port 5000
FROM mcr.microsoft.com/dotnet/core/sdk:2.1
WORKDIR /api
EXPOSE 5000
COPY --from=build-env /api/out .
ENTRYPOINT [ "dotnet", "LabProgramowanie_II.dll" ]