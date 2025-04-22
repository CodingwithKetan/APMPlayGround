## Prerequisites

Ensure RabbitMQ is running before you publish or run the app:

### Windows (PowerShell)
```powershell
Get-Service rabbitmq | Select-Object -ExpandProperty Status
```
_Should return_ `Running`

### Linux (systemd)
```bash
systemctl is-active rabbitmq-server
```
_Should print_ `active`

---

### How to download RabbitMQ server 
https://www.rabbitmq.com/docs/download

### Which shold we consider 
Currently OTEL is testing with Version 6.0.0 to 7.0.0 so we should consider that.

## Generate Self‑Contained App

Publish your app as a single, self‑contained Linux binary:

```bash
dotnet publish -c Release -r linux-x64 --self-contained true /p:PublishSingleFile=true -o ./publish/linux64
```

---

## Parameter Breakdown

- **`-c Release`**  
  Builds using the **Release** configuration (optimizations enabled, debug info disabled).

- **`-r linux-x64`**  
  Targets the **Runtime Identifier (RID)** for Linux on x64 architecture.  
  (See the full list of RIDs: https://docs.microsoft.com/dotnet/core/rid-catalog)

- **`--self-contained true`**  
  Bundles the **.NET runtime** with your app so the target machine doesn’t need .NET installed.

- **`/p:PublishSingleFile=true`**  
  Packs **all** assemblies and dependencies into a **single executable**.

- **`-o ./publish/linux64`**  
  Outputs the published files into the `./publish/linux64` directory.
```
