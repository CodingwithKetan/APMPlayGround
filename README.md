## Prerequisites

Ensure MySQL is running before you publish or run the app:

### How to download MySQl
https://dev.mysql.com/doc/mysql-installation-excerpt/5.7/en/


## 1. Check MySQL Service Status

### Linux (systemd)

```bash
# Check if MySQL is active
sudo systemctl status mysql

# If it shows inactive or stopped, start the service
sudo systemctl start mysql
```

### Windows (PowerShell)

```powershell
# Check MySQL-related services
Get-Service -Name "MySQL*"

# If stopped, start the service
Start-Service -Name "MySQL"
```

> **Tip:** On Windows, the service name might vary (e.g., `MySQL80`). Use `Get-Service -Name "MySQL*"` to list all.

---


## 2. Connect to MySQL Shell

Before connecting, ensure you have a root password:

- **Ubuntu/Debian**: By default, MySQL may use socket authentication. You can connect without a password by running:
  ```bash
  sudo mysql
  ```
  Once inside, set a root password:
  ```sql
  ALTER USER 'root'@'localhost' IDENTIFIED WITH mysql_native_password BY 'admin';
  FLUSH PRIVILEGES;
  EXIT;
  ```

- **Windows**: During installation, you set the `root` password via the installer. If you forgot it, follow the [official MySQL guide](https://dev.mysql.com/doc/refman/8.0/en/resetting-permissions.html) to reset the root password.

Now connect to the MySQL shell:
```bash
mysql -u root -p
```

- When prompted, enter the **root** user password.

---

## 3. Create User and Database

Once inside the MySQL prompt (`mysql>`), run:

```sql
-- 1) Create a new user 'sa' with password 'admin'
CREATE USER 'sa'@'%' IDENTIFIED BY 'admin';

-- 2) Grant all privileges to 'sa'
GRANT ALL PRIVILEGES ON *.* TO 'sa'@'%' WITH GRANT OPTION;

-- 3) Create the TestDB database
CREATE DATABASE TestDB;

-- 4) Apply privilege changes
FLUSH PRIVILEGES;
```

> **Note:** The `@'%'` host wildcard allows connections from any host. For stricter security, replace `%` with `localhost` if only local connections are needed.

---

## 4. Verify Creation

```sql
-- List databases
SHOW DATABASES;

-- Confirm user exists
SELECT user, host FROM mysql.user WHERE user = 'sa';
```

---

## 5. Exit MySQL

```sql
EXIT;
```

---


### Which shold we consider 
Currently OTEL is testing with Version 2.0.0 and so we should consider that.

## Generate Self‑Contained App

Publish your app as a single, self‑contained Linux binary:

```bash
dotnet publish -c Release -r linux-x64 -f net8.0 --self-contained true /p:PublishSingleFile=true -o ./publish/linux64 -p:LibraryVersion=2.0.0
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

- **`-p:LibraryVersion=9.0.0`**  
  here we are specifying that version of MySQL Connector if we want 2.0.0

---

### if the .Net SDK present in System then use below command for to run application 

```bash
dotnet run -f net8.0 /p:LibraryVersion=2.0.0
```

- Here you can update dotnet framework and library version as you want.
```		
