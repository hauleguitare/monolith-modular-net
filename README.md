﻿# monolith-modular-net
# Migration Auth: dotnet ef migrations add {{Migration Name}} --startup-project "WebApi" --project "Modules/MonolithModularNET.Auth" --context "AuthContext"
# Update Database Auth: dotnet ef database update {{Migration Name}} --startup-project "WebApi" --project "Modules/MonolithModularNET.Auth"
