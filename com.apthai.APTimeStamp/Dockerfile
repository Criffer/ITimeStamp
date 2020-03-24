FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY com.apthai.APTimeStamp.csproj ./
RUN dotnet restore com.apthai.APTimeStamp.csproj
COPY . .
WORKDIR /src
RUN dotnet build com.apthai.APTimeStamp.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish com.apthai.APTimeStamp.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "com.apthai.APTimeStamp.dll"]

ENV TZ=Asia/Bangkok
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone