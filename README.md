# Self‑Contained .NET Application with Dynamic Library Version

This README explains how to publish a .NET application as a self‑contained, single‑file executable, target multiple frameworks, and override library versions at build/runtime. It also breaks down each CLI flag and MSBuild parameter.

---

### What is a Self‑Contained Application?
A **self‑contained deployment** bundles your app **and** the entire .NET runtime into the output folder. This means the target machine does **not** need to have .NET installed. You get:
- The application executable (or single file if you bundle).
- All framework libraries and runtime components.

---

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) installed on your machine  
- Your project uses the SDK‑style `.csproj` format  
- (Optional) A multi‑targeted project if you need to support multiple frameworks  

---

## Project Configuration

In your `.csproj`, you can set up multi‑targeting, default library version, and runtime identifiers. Example:

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <!-- Multi-target frameworks -->
    <TargetFrameworks>net9.0;net8.0</TargetFrameworks>

    <!-- Default library version; can be overridden -->
    <LibraryVersion>6.0.16</LibraryVersion>

    <!-- Nullable and implicit usings -->
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>

    <!-- Supported runtime environments -->
    <RuntimeIdentifiers>win-x64;linux-x64</RuntimeIdentifiers>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite"
                      Version="$(LibraryVersion)" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design"
                      Version="$(LibraryVersion)"
                      PrivateAssets="all" />
    <PackageReference Include="Swashbuckle.AspNetCore" Version="6.2.3" />
  </ItemGroup>
</Project>
```

- **TargetFrameworks**: list of frameworks you want to compile for.  
- **LibraryVersion**: a custom MSBuild property for your NuGet package version.  
- **RuntimeIdentifiers**: RIDs for each platform you want to deploy to.  

---

## Publishing the Application

### Basic Publish Command

From your project directory (next to `MyApp.csproj`), run:

```bash
dotnet publish MyApp.csproj \
  -c Release \
  -r linux-x64 \
  --self-contained true \
  -o ./publish/linux-x64
```

This produces a self‑contained folder under `./publish/linux-x64` containing:

- Your app executable (`MyApp`)  
- All framework DLLs  
- The .NET runtime  

### Targeting Specific Frameworks

If you multi‑target, specify `-f` / `--framework`:

```bash
dotnet publish MyApp.csproj \
  -c Release \
  -r linux-x64 \
  -f net8.0 \
  --self-contained true \
  -o ./publish/linux-x64-net8
```

That picks the `net8.0` target and builds only that.

### Overriding LibraryVersion at Runtime

Override MSBuild properties on the CLI with `-p:` (short for `--property:`):

```bash
# Override the LibraryVersion property
dotnet publish MyApp.csproj \
  -c Release -r linux-x64 -f net8.0 \
  -p:LibraryVersion=7.0.0-preview \
  --self-contained true \
  -o ./publish/linux-x64-net8-preview
```

Alternatively, set the environment variable (MSBuild imports all env‑vars):

```bash
export LibraryVersion=7.1.2
dotnet publish MyApp.csproj -c Release -r linux-x64 -f net8.0
```

---

## Explanation of Flags & Parameters

| Flag / Parameter                 | Description                                                                                                      |
| -------------------------------- | ---------------------------------------------------------------------------------------------------------------- |
| `MyApp.csproj`                   | Path to your project file; you can omit if you’re in the same directory and there’s only one `.csproj`.          |
| `-c Release` / `--configuration` | Build configuration; `Release` enables optimizations, `Debug` includes debug symbols.                            |
| `-r linux-x64` / `--runtime`     | Target Runtime Identifier (RID) for the platform you’re publishing to (e.g., `win-x64`, `linux-arm64`).          |
| `-f net8.0` / `--framework`      | Target framework when multi‑targeting (e.g., `net7.0`, `net8.0`, `netstandard2.0`).                              |
| `--self-contained true`          | Packages the .NET runtime and framework libraries so the app can run on machines without .NET installed.         |
| `/p:PublishSingleFile=true`      | Bundle app and dependencies into a single executable file (on Windows `.exe`, Linux a native ELF binary).        |
| `/p:PublishTrimmed=true`         | Trims unused IL to reduce final binary size; may require explicit trimmer roots for reflection-heavy libraries.  |
| `/p:PublishReadyToRun=true`      | Ahead‑of‑time (AOT) compile native code to improve startup performance.                                          |
| `-p:LibraryVersion=<version>`    | Overrides your custom `LibraryVersion` MSBuild property to pull a different NuGet package version at build time. |
| `-o ./publish/...` / `--output`  | Overrides the default output directory for published files.                                                      |

---

## Conditional Package References

To pull different package versions per framework, use MSBuild conditions:

```xml
<PropertyGroup Condition="'$(TargetFramework)'=='net8.0'">
  <LibraryVersion>6.0.16</LibraryVersion>
</PropertyGroup>
<PropertyGroup Condition="'$(TargetFramework)'=='net9.0'">
  <LibraryVersion>7.0.0-preview</LibraryVersion>
</PropertyGroup>

<ItemGroup>
  <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite"
                    Version="$(LibraryVersion)" />
</ItemGroup>
```

When you run `dotnet publish -f net8.0` it uses EF 6.0.16; with `-f net9.0` it uses EF 7.0.0-preview.

---

## Examples

### Publish for Linux x64 on .NET 8 (default version)

```bash
dotnet publish MyApp.csproj -c Release -r linux-x64 -f net8.0 --self-contained true -o ./publish/linux-x64
```

### Publish for Windows x64 on .NET 9 with preview EF

```bash
dotnet publish MyApp.csproj -c Release -r win-x64 -f net9.0 \
  -p:LibraryVersion=7.0.0-preview \
  --self-contained true \
  /p:PublishSingleFile=true \
  /p:PublishTrimmed=true \
  /p:PublishReadyToRun=true \
  -o ./publish/win-x64-net9-preview
```

---


