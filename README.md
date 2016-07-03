# OdooRpc.CoreCLR.Client

Simple [Odoo JSON-RPC](https://www.odoo.com/documentation/9.0/api_integration.html) Client for [.Net Core 1.0](https://www.microsoft.com/net/core).

Inspired by https://github.com/saidimu/odoo and https://github.com/osiell/odoorpc.

### Installation

*TODO*

### Build From Source

To build the library from source, please do:

```Shells
# Clone repository
git clone https://github.com/vmlf01/OdooRpc.CoreCLR.Client.git
cd OdooRpc.CoreCLR.Client

# Restore NuGet packages
dotnet restore

# Build NuGet package
dotnet pack src/OdooRpc.CoreCLR.Client --output build
```

You can also run the tests by doing:

```Shell
# Run tests from repository root
dotnet test tests/OdooRpc.CoreCLR.Client.Tests
```

### Publish NuGet package to repository

You will need nuget.exe on your path and set the NuGet API Key before you can push a package to the NuGet repository.

```Shell
nuget.exe setApiKey 76d7xxxx-xxxx-xxxx-xxxx-eabb8b0cxxxx
nuget.exe push .\build\OdooRpc.CoreCLR.Client.0.0.1.nupkg
```

### Samples

*TODO*
