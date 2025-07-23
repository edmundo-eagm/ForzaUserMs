# forza-user-ms
# Crear migración
dotnet ef migrations add UserUsernameUnique -o ./Src/Infrastructure/Migrations

# Aplicar Migración
dotnet ef database update


# Compilar
dotnet build

# Ejecutar en debug
dotnet run