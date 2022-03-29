# Blue10CloudConnector
4 self-updating CloudConnector Templates

## Templates
- There are 2 templates made with WPF and self-update using ClickOnce.
  - .NET 6 Project
  - .NET Framework 4.8 Project
- The other 2 templates are Windows services built with Topshelf that have their own updating logic.
  - .NET 6 Project
  - .NET Framework 4.7.2 Project

## Technologies used
- ClickOnce
- WPF (Windows Presentation Foundation)
- Windows Services

## Installation 
### The templates can be installed from Nuget by using dotnet new :

- #### WPF dotnet core template
``` dotnet new --install Blue10.Templates.Cloud.Connector ```
- #### WPF dotnet framework template
``` dotnet new --install Blue10.Templates.Cloud.Connector.DNF ```
- #### Windows Service dotnet core template
``` dotnet new --install Blue10.Templates.Windows.Service ```
- #### Windows Service dotnet framework template
``` dotnet new --install Blue10.Templates.Windows.Service.DNF ```
