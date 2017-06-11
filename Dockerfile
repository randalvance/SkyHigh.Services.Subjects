FROM microsoft/aspnetcore:1.1.2
WORKDIR /app
COPY ./publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "SkyHigh.Services.Subjects.dll"]