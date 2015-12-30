# CoreCLR.OdooRpc.Client

Simple [Odoo JSON-RPC](https://www.odoo.com/documentation/9.0/api_integration.html) Client for [CoreCLR 5.0](https://github.com/dotnet/coreclr).

### Installation

*TODO*

### Build From Source

To build the library from source, please do:

```Shells
# Clone repository
git clone https://github.com/vmlf01/CoreCLR.OdooRpc.Client.git
cd CoreCLR.OdooRpc.Client

# Restore NuGet packages
dnu restore

# Build NuGet package
dnu pack src/CoreCLR.OdooRpc.Client --out build
```

You can also run the tests by doing:

```Shell
# Run tests from repository root
dnx -p tests/CoreCLR.OdooRpc.Client.Tests test
```

### Publish NuGet package to repository

You will need nuget.exe on your path and set the NuGet API Key before you can push a package to the NuGet repository.

```Shell
nuget.exe setApiKey 76d7xxxx-xxxx-xxxx-xxxx-eabb8b0cxxxx
nuget.exe push .\build\Debug\CoreCLR.OdooRpc.Client.0.0.1.nupkg
```

### Samples

*TODO*
