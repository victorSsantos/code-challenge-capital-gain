#!/bin/bash
set -e

# entra na pasta do projeto Presentation
cd CapitalGains.Presentation

# publica em bin�rio �nico para Linux
dotnet publish CapitalGains.Presentation.csproj -c Release -r linux-x64 \
  --self-contained true -p:PublishSingleFile=true -o ./publish-linux

# entra na pasta de sa�da
cd publish-linux

# renomeia e d� permiss�o
mv CapitalGains.Presentation capital-gains
chmod +x capital-gains

echo "Bin�rio gerado em: $(pwd)/capital-gains"
echo "Rode com: ./capital-gains < ../input.txt > output.json"