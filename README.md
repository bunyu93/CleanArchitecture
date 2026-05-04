# CleanArchitecture Template

Templates to use when creating an application

https://www.nuget.org/packages/Bunyamin.Sakar.CleanArchitecture

## Local development database setup

The template intentionally leaves `Database:Connection` blank in both
`appsettings.json` and `appsettings.Development.json`. Do not add database
passwords or other secrets to those files. For local development, provide the
connection string with .NET user secrets or an exported environment variable.

### Prerequisites

- .NET SDK compatible with the projects in `src/`
- Docker with Docker Compose

### 1. Configure Docker Compose environment values

Copy the example environment file and replace every placeholder value with a
local non-secret value that is valid for your machine:

```bash
cp src/.env.example src/.env
```

Edit `src/.env` and set values such as:

```dotenv
POSTGRES_USER=postgres
POSTGRES_PASSWORD=your-local-password
POSTGRES_DB=weatherdb
POSTGRES_PORT=5432
PGADMIN_IMAGE=dpage/pgadmin4:latest
PGADMIN_DEFAULT_EMAIL=admin@cleanarchitecture.dev
PGADMIN_DEFAULT_PASSWORD=your-local-pgadmin-password
PGADMIN_PORT=5050
```

`src/.env` is read by Docker Compose. The application does **not** load `.env`
files automatically.

### 2. Start PostgreSQL and pgAdmin

Run Docker Compose from the `src/` directory so it can read `src/.env`:

```bash
cd src
docker compose up -d postgres pgadmin
```

pgAdmin is bound to localhost only. Open `http://localhost:5050` (or your
configured `PGADMIN_PORT`) and sign in with `PGADMIN_DEFAULT_EMAIL` and
`PGADMIN_DEFAULT_PASSWORD` from `src/.env`.

The Compose PostgreSQL server is automatically registered in pgAdmin from
`src/pgadmin/servers.json` as **CleanArchitecture Postgres**. The registration
uses the scaffold defaults:

- **Host name/address:** `postgres`
- **Port:** `5432`
- **Maintenance database:** `weatherdb`
- **Username:** `postgres`
- **Password:** value of `POSTGRES_PASSWORD`

If you change `POSTGRES_DB` or `POSTGRES_USER`, update
`src/pgadmin/servers.json` to match, or edit the server registration in pgAdmin.

pgAdmin data is persisted in the `pgadmin_data` Docker volume. This keeps users,
sessions, and server registrations across restarts. Changing
`PGADMIN_DEFAULT_PASSWORD` after the first startup may not update an existing
pgAdmin account. To reset only pgAdmin state, stop Compose and remove the
project's `pgadmin_data` volume, for example `docker volume rm src_pgadmin_data`
when running Compose from the `src/` directory. Avoid saving database passwords
in pgAdmin unless you are comfortable storing them in your local Docker volume.

### 3. Configure the API connection string

Choose one of the following options.

#### Recommended: .NET user secrets

```bash
dotnet user-secrets set \
  --project src/CleanArchitectureTemplate.Api/CleanArchitectureTemplate.Api.csproj \
  "Database:Connection" \
  "Host=localhost;Port=5432;Database=weatherdb;Username=postgres;Password=your-local-password"
```

#### Alternative: exported environment variable

```bash
export Database__Connection="Host=localhost;Port=5432;Database=weatherdb;Username=postgres;Password=your-local-password"
```

Use double underscores (`__`) in environment variable names to represent nested
.NET configuration keys such as `Database:Connection`.

### 4. Optional startup migrations and seeding

By default, startup migrations and seed data are disabled. Enable them only when
you want the API to apply migrations or seed data at startup:

```bash
export Database__RunMigrationsOnStartup=true
export Database__RunSeedOnStartup=true
```

You can also store these values with user secrets by setting
`Database:RunMigrationsOnStartup` and `Database:RunSeedOnStartup`.

### Troubleshooting

- **The API cannot connect to PostgreSQL:** confirm `docker compose ps` shows the
  `postgres` service is running and that the port, database, username, and
  password match your user secret or `Database__Connection` value.
- **Changing `src/.env` does not change the API connection:** this is expected.
  `src/.env` is for Docker Compose values; configure the API separately with
  user secrets or exported environment variables.
- **Configuration files have blank database connection values:** this is
  intentional to avoid committing secrets.
