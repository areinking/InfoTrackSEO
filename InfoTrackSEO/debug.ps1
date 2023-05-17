invoke-expression 'cmd /c start powershell -Command {dotnet run --project InfoTrackSEO.API/InfoTrackSEO.API.csproj}'
invoke-expression 'cmd /c start powershell -Command {dotnet run --project InfoTrackSEO.Web/InfoTrackSEO.Web.csproj}'
start https://localhost:54321
