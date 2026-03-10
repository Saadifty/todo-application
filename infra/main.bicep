targetScope = 'resourceGroup'

@description('Project prefix used for resource naming (must be globally unique for some resources).')
param prefix string = 'todo-saad'

@description('Azure location for all resources. Keep it the same as the Resource Group location.')
param location string = resourceGroup().location

@description('PostgreSQL admin username (cannot be "postgres").')
param pgAdminUser string = 'pgadmin'

@secure()
@description('PostgreSQL admin password.')
param pgAdminPassword string

@description('Database name.')
param pgDatabaseName string = 'todosdb'

@description('SKU for App Service plan. B1 is cheap-ish; you can change later.')
param appServiceSku string = 'B1'

var appServicePlanName = 'asp-${prefix}'
var webAppName = '${prefix}-api'
var pgServerName = '${prefix}-pg'
var pgVersion = '16'
var pgSkuName = 'Standard_B1ms'
var pgStorageGb = 32

resource plan 'Microsoft.Web/serverfarms@2023-12-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: appServiceSku
    tier: 'Basic'
    size: appServiceSku
    capacity: 1
  }
  kind: 'linux'
  properties: {
    reserved: true
  }
}

resource webApp 'Microsoft.Web/sites@2023-12-01' = {
  name: webAppName
  location: location
  kind: 'app,linux'
  properties: {
    serverFarmId: plan.id
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|8.0'
     appSettings: [
  {
    name: 'ASPNETCORE_ENVIRONMENT'
    value: 'Production'
  }
  {
    name: 'WEBSITES_PORT'
    value: '8080'
  }
]
    httpsOnly: true
  }
}

resource pg 'Microsoft.DBforPostgreSQL/flexibleServers@2023-06-01-preview' = {
  name: pgServerName
  location: location
  sku: {
    name: pgSkuName
    tier: 'Burstable'
  }
  properties: {
    version: pgVersion
    administratorLogin: pgAdminUser
    administratorLoginPassword: pgAdminPassword
    storage: {
      storageSizeGB: pgStorageGb
    }
    backup: {
      backupRetentionDays: 7
      geoRedundantBackup: 'Disabled'
    }
    highAvailability: {
      mode: 'Disabled'
    }
    network: {
      // For a newbie-friendly first deploy, we keep it public.
      // Later we can harden with Private Endpoint.
      publicNetworkAccess: 'Enabled'
    }
  }
}

resource pgDb 'Microsoft.DBforPostgreSQL/flexibleServers/databases@2023-06-01-preview' = {
  name: '${pg.name}/${pgDatabaseName}'
  properties: {
    charset: 'UTF8'
    collation: 'en_US.utf8'
  }
}

output webAppName string = webApp.name
output webAppHostName string = webApp.properties.defaultHostName
output pgServerName string = pg.name
output pgDatabaseName string = pgDatabaseName
