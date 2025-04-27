var builder = DistributedApplication.CreateBuilder(args);

// Integrations
var sqlServer = builder
  .AddSqlServer(ServiceName.SqlServer, port: 1433)
  .WithImage("mssql/server", "2022-CU17-ubuntu-22.04")
  .WithDataVolume(ServiceName.SqlServerVolume)
  .WithLifetime(ContainerLifetime.Persistent);

var azureStorage = builder
  .AddAzureStorage(ServiceName.AzureStorage)
  .RunAsEmulator(config => config
    .WithImage("azure-storage/azurite", "3.33.0")
    .WithDataVolume(ServiceName.AzureStorageVolume)
    .WithLifetime(ContainerLifetime.Persistent));

var azureServiceBus = builder
  .AddAzureServiceBus("messaging")
  .RunAsEmulator(emulator => emulator.WithConfiguration(AzureServiceBusConfig.Configure));

// Resources
var dataProtectionDb = sqlServer.AddDatabase(SharedServiceName.DataProtectionDb);
var azureStorageBlobs = azureStorage.AddBlobs(ServiceName.AzureStorageBlobs);
var cmsDb = sqlServer.AddDatabase("EPiServerDB", ServiceName.CommerceWebCmsDb);
var commerceDb = sqlServer.AddDatabase("EcfSqlConnection", ServiceName.CommerceWebCommerceDb);
var identityServerConfigurationDb = sqlServer.AddDatabase(SharedServiceName.IdentityServerConfigurationDb);
var identityServerIdentityDb = sqlServer.AddDatabase(SharedServiceName.IdentityServerIdentityDb);
var identityServerPersistedGrantDb = sqlServer.AddDatabase(SharedServiceName.IdentityServerPersistedGrantDb);

// Projects
var migrationApp = builder
  .AddProjectWithDefaults<Projects.Migration_App>(ServiceName.MigrationApp)
  .WaitFor(azureStorage)
  .WaitFor(sqlServer);

var identityServerWeb = builder
  .AddProjectWithDefaults<Projects.IdentityServer_Web>(SharedServiceName.IdentityServerWeb)
  .AddEndpointWithDefaults(out var identityServerWebEndpoint, true)
  .WaitForCompletion(migrationApp);

var commerceWeb = builder
  .AddProjectWithDefaults<Projects.Commerce_Web>(ServiceName.CommerceWeb)
  .AddEndpointWithDefaults(out var commerceWebEndpoint)
  .WaitForCompletion(migrationApp);

var basketApi = builder
  .AddProjectWithDefaults<Projects.Basket_Api>(SharedServiceName.BasketApi)
  .AddEndpointWithDefaults(out var basketApiEndpoint)
  .WaitForCompletion(migrationApp);

var profileApi = builder
  .AddProjectWithDefaults<Projects.Profile_Api>(SharedServiceName.ProfileApi)
  .AddEndpointWithDefaults(out var profileApiEndpoint)
  .WaitForCompletion(migrationApp);

var shop1Web = builder
  .AddProjectWithDefaults<Projects.Shop1_Web>(ServiceName.Shop1Web)
  .AddEndpointWithDefaults(out var shop1WebEndpoint)
  .WaitForCompletion(migrationApp);

// References
migrationApp
  .WithReference(azureStorageBlobs)
  .WithReference(sqlServer)
  .WithReference(commerceWebEndpoint)
  .WithReference(basketApiEndpoint)
  .WithReference(profileApiEndpoint)
  .WithReference(shop1WebEndpoint);

identityServerWeb
  .WithReference(identityServerConfigurationDb)
  .WithReference(identityServerIdentityDb)
  .WithReference(identityServerPersistedGrantDb)
  .WithReference(dataProtectionDb)
  .WithReference(commerceWebEndpoint)
  .WithReference(basketApiEndpoint)
  .WithReference(profileApiEndpoint)
  .WithReference(shop1WebEndpoint);

commerceWeb
  .WithReference(azureStorageBlobs)
  .WithReference(azureServiceBus)
  .WithReference(cmsDb)
  .WithReference(commerceDb)
  .WithReference(dataProtectionDb)
  .WithReference(identityServerWebEndpoint)
  .WithReference(basketApiEndpoint)
  .WithReference(profileApiEndpoint);

basketApi
  .WithReference(identityServerWebEndpoint);

profileApi
  .WithReference(identityServerWebEndpoint);

shop1Web
  .WithReference(dataProtectionDb)
  .WithReference(identityServerWebEndpoint)
  .WithReference(basketApiEndpoint)
  .WithReference(profileApiEndpoint);

builder.Build().Run();
