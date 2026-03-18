FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["CourseWebApi/CourseWebApi.csproj", "CourseWebApi/"]
RUN dotnet restore "CourseWebApi/CourseWebApi.csproj"
COPY . .

WORKDIR /src/CourseWebApi
RUN dotnet publish "CourseWebApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "CourseWebApi.dll"]