## Prerequisites

Ensure postgresql is running before you publish or run the app:

### How to download postgresql
https://www.postgresql.org/download

## Create a database and user

#### On Linux:
```bash
sudo -i -u postgres psql
```

#### On Windows:
```bash
psql -U postgres
```

Inside the psql prompt, execute:

-  Create a login role
```bash
CREATE ROLE sa WITH LOGIN PASSWORD 'admin';
```

- Create the database owned by that user
```bash
CREATE DATABASE testapm OWNER sa;
```

- Grant all privileges
```bash
GRANT ALL PRIVILEGES ON DATABASE testapm TO sa;
```

If you prefer a different username/password, just replace sa and 'admin' above consistently.


### Which shold we consider 
Currently OTEL is testing with Version 8.0.5 and so we should consider that.

## Generate Self‑Contained App

Publish your app as a single, self‑contained Linux binary:

```bash
dotnet publish -c Release -r linux-x64 -f net8.0 --self-contained true /p:PublishSingleFile=true -o ./publish/linux64
```

---

### Parameter Breakdown

- **`-c Release`**  
  Builds using the **Release** configuration (optimizations enabled, debug info disabled).
  
  - **`-f net8.0`**  
  Builds using the **framework** currently supported verisons are net8.0 and net9.0.

- **`-r linux-x64`**  
  Targets the **Runtime Identifier (RID)** for Linux on x64 architecture.  
  (See the full list of RIDs: https://docs.microsoft.com/dotnet/core/rid-catalog)

- **`--self-contained true`**  
  Bundles the **.NET runtime** with your app so the target machine doesn’t need .NET installed.

- **`/p:PublishSingleFile=true`**  
  Packs **all** assemblies and dependencies into a **single executable**.

- **`-o ./publish/linux64`**  
  Outputs the published files into the `./publish/linux64` directory.

---

### if the .Net SDK present in System then use below command for to run application 

```bash
dotnet run -f net8.0 /p:LibraryVersion=6.0.0
```

- Here you can update dotnet framework and library version as you want.
```
