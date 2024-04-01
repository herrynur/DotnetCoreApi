FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
RUN --mount=type=bind,source=BackendService/BackendService.csproj,target=BackendService/BackendService.csproj \
	--mount=type=cache,id=nuget,target=/root/.nuget/packages,sharing=locked \
	dotnet restore "./BackendService/BackendService.csproj"
COPY . .
WORKDIR "/src/BackendService"
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages,sharing=locked \
	dotnet build "./BackendService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages,sharing=locked \
	dotnet publish "./BackendService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BackendService.dll"]