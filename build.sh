#!/bin/bash
set -e

# entra na pasta do projeto Presentation
cd CapitalGains.Presentation

# publica em binário único para Linux
dotnet publish CapitalGains.Presentation.csproj -c Release -r linux-x64 \
  --self-contained true -p:PublishSingleFile=true -o ./publish-linux

# entra na pasta de saída
cd publish-linux

# renomeia e dá permissão
mv CapitalGains.Presentation capital-gains
chmod +x capital-gains

echo "Binário gerado em: $(pwd)/capital-gains"
echo "Rode com: ./capital-gains < ../input.txt > output.json"