1. **Install Docker**  
   - Visit the Docker download page: https://docs.docker.com/get-docker/  
   - Follow the instructions for your OS to install Docker Engine and CLI.

2. **Pull Oracle XE Docker Image**  
   ```bash
   docker pull gvenzl/oracle-xe:latest
```
```

3. **Run the Oracle XE Container**
```bash
   docker run -d  --name oracle-xe -e ORACLE_PASSWORD=Oracle123 -p 1521:1521 gvenzl/oracle-xe:latest
```


4. **Verify Container is Running**
   ```bash
   docker ps
   docker logs oracle-xe
