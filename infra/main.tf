terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 4.0.0"
    }
  }
  required_version = ">= 0.14.9"
}

variable "suscription_id" {
  type        = string
  description = "452059b4-1dbe-460c-9ef6-85738d616b22"
}

provider "azurerm" {
  features {}
  subscription_id = var.suscription_id
}

# Crear el grupo de recursos
resource "azurerm_resource_group" "rg" {
  name     = "upt-arg-app"
  location = "centralus"
}

# Crear el Plan de Servicio de Aplicación
resource "azurerm_service_plan" "appserviceplan" {
  name                = "upt-asp-app"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  os_type             = "Linux"
  sku_name            = "F1"
}

# Crear la Aplicación Web en Azure
resource "azurerm_linux_web_app" "webapp" {
  name                = "upt-awa-animalia"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  service_plan_id     = azurerm_service_plan.appserviceplan.id
  depends_on          = [azurerm_service_plan.appserviceplan]

  site_config {
    minimum_tls_version = "1.2"
    always_on           = false
    application_stack {
      dotnet_version = "8.0"
    }
  }
}

# Recurso para configurar el control de código fuente en Azure App Service
# resource "azurerm_app_service_source_control" "app_service_source_control" {
#   app_id = azurerm_linux_web_app.webapp.id  # ID de la App Service
#   repo_url = "https://github.com/UPT-FAING-EPIS/proyecto-si982-2024-rec-u2-proyectou2_web2_ccalli_apaza"  # URL del repositorio
#   branch = "main"  # Rama a desplegar
#   use_manual_integration = false  # Integración automática
#   use_mercurial = false  # No usar Mercurial (Git es el predeterminado)
# }
