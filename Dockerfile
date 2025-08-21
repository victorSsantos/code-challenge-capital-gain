# ===== build =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia tudo (ajuste .dockerignore para não levar bin/obj/publish)
COPY . .

# Publica a app de console como binário único, self-contained, para Linux
RUN dotnet publish CapitalGains.Presentation/CapitalGains.Presentation.csproj \
    -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true \
    -o /app

# ===== runtime =====
# Para self-contained use runtime-deps (não precisa do runtime .NET completo)
FROM mcr.microsoft.com/dotnet/runtime-deps:8.0 AS runtime
WORKDIR /app

# Copia apenas o binário publicado
COPY --from=build /app/CapitalGains.Presentation /app/capital-gains
RUN chmod +x /app/capital-gains

# A app lê do STDIN e escreve no STDOUT
ENTRYPOINT ["/app/capital-gains"]
