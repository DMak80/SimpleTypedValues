FROM mcr.microsoft.com/dotnet/sdk:6.0 AS builder

ARG Version=0.0.0.1-alpha
ARG NUGET_KEY
ARG NUGET_URL
WORKDIR .

COPY . .

RUN dotnet restore
RUN dotnet build /p:Version=$Version -c Release --no-restore  
RUN dotnet pack /p:Version=$Version -c Release --no-restore --no-build -o ./artifacts 
RUN dotnet nuget push ./artifacts/*.nupkg --source NUGET_URL --api-key $NUGET_KEY
