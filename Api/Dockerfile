# build sdk image
FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build-env

WORKDIR /source

COPY Sample.Tris.sln ./Sample.Tris.sln
COPY Sample.Tris.Lib/Sample.Tris.Lib.csproj ./Sample.Tris.Lib/
COPY Sample.Tris.Lib.Tests/Sample.Tris.Lib.Tests.csproj ./Sample.Tris.Lib.Tests/
COPY Sample.Tris.WebApi/Sample.Tris.WebApi.csproj ./Sample.Tris.WebApi/
COPY Sample.Tris.WebApi.Tests/Sample.Tris.WebApi.Tests.csproj ./Sample.Tris.WebApi.Tests/

RUN dotnet restore

COPY Sample.Tris.Lib ./Sample.Tris.Lib
COPY Sample.Tris.Lib.Tests ./Sample.Tris.Lib.Tests
COPY Sample.Tris.WebApi ./Sample.Tris.WebApi
COPY Sample.Tris.WebApi.Tests ./Sample.Tris.WebApi.Tests

RUN dotnet test ./Sample.Tris.sln --no-restore --configuration Release

RUN dotnet publish "./Sample.Tris.WebApi/Sample.Tris.WebApi.csproj" --output "../dist" --no-restore --configuration Release

#######################

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim
WORKDIR /app

ENV TrisApiGridSettings__MaxColumns=UNSET
ENV TrisApiGridSettings__Rows unset=UNSET
ENV TrisApiGridSettings__CellSizeInPixels=UNSET

COPY --from=build-env /dist .
EXPOSE 80 443
ENTRYPOINT ["dotnet", "Sample.Tris.WebApi.dll"]
