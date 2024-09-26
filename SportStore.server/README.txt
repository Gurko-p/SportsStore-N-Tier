
#Команда для накатывания МИГРАЦИИ в контексте IdentityApplicationDbContext

	dotnet ef migrations add InitialIdentity --project DataLayer --context IdentityApplicationDbContext --output-dir Migrations\IdentityMigrations --startup-project SportStore.server

-------------------------------------------------------------------------
#Команда для накатывания МИГРАЦИИ в контексте ApplicationDbContext

	dotnet ef migrations add InitialApplicationData --project DataLayer --context ApplicationDbContext --output-dir Migrations\ApplicationMigrations --startup-project SportStore.server

-------------------------------------------------------------------------

#Команда для ОБНОВЛЕНИЯ базы данных после миграции в контексте IdentityApplicationDbContext

	dotnet ef database update --project DataLayer --context IdentityApplicationDbContext --startup-project SportStore.server

-------------------------------------------------------------------------
#Команда для ОБНОВЛЕНИЯ базы данных после миграции в контексте ApplicationDbContext

	dotnet ef database update --project DataLayer --context ApplicationDbContext --startup-project SportStore.server

-------------------------------------------------------------------------