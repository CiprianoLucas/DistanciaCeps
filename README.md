# Cálculo de Distância Entre CEPS
Uma aplicação completa com diversas tecnologias mas com própósito simples, calcular a distância linha reta entre dois CEPs.

## Propósito
O objetivo deste projeto é verificar como está meu conhecimento dentro da linguagem C#, utilizando o framework .NET, e ainda fazer a integração com outras tecnologias.

## Funcionalidades aplicadas
- Cadastro e Login de Usuários: Os usuários podem se registrar e fazer login para acessar o sistema.

- Efetuar um novo cálculo de distância entre dois CEPs.

- Listar e filtrar por todas as consultas já efetuadas.

- Manter salvo Cash das requisições feitas para a API CepAberto(utilizada para pegar as coordenadas)

- Funcionar através de um sistema Docker

- Salvar dados e consultas em um banco de dados.

- Importar arquivo CSV com ceps e coloca-los em uma fila para calcular.

## Tecnologias Utilizadas
- Backend:
    - **C# .NET 9**
    - Estrutura hexagonal
    - JWT
    - Cryptography
    - WebApplicationBuilder
    - Logger
    - Migrations

- Frontend:
    - **Vue 2**
    - Bootstrap
    - Axios
    - Typescript
    
- Infraestrutura:
    - **Docker**
    - **PostgreSQL**
    - Redis
    - RabbitMq

## Instalação
### Pré-requisitos obrigatórios
- Docker ^25.0.3

### Pré-requisitos para desenvolvimento
- Docker ^25.0.3
- Node ^18
- .NET ^9

### Clonando o Repositório

```bash
git clone https://github.com/CiprianoLucas/DistanciaCeps.git
cd DistanciaCeps
```

## Configuração obrigatória
- Configure as variáveis de ambiente necessárias, como credenciais do banco de dados e chaves secretas, utilize o arquivo .env.example para criar um .env

- Certifique-se de que o backend e o frontend estejam apontando para os respectivos servidores e endpoints.

### Configurando o ambiente
Com docker em funcionamento, Acesse a pasta de build e execute o comando para criar o container.

```bash
cd build
docker compose up
```

### Como demonstrar
Após realizar a instalação e configuração. Acesse a aplicação em http://localhost:7001 para utilizar o frontend e http://localhost:7000 para visualizar a documentação.

## Configuração desenvolvimento
**Efetue a configuração obrigatória**

O projeto é separado em duas aplicações, back (backend) e front (frontend)

### Desenvolvimento Frontend
Recomendo utilizar o endpoint do docker para fazer as requisições para o backend em vez do repositório principal.

A partir do diretório raiz do projeto, acesse o diretório de frontend:
```bash
cd front
```

Faça a instalação das dependências:
```bash
npm i
```

Faça rodar com visualização de modificações ativa:
```bash
npm run dev
```
Ao fazer esse comando ele retornará o endereço para visualizar a página. A tela será atualizada sempre que modificar o diretório

### Desenvolvimento Backend
As técnologias Postres, Redis e RabbitMq precisam estar funcionando, para isso, a partir do diretório principal, execute.
```bash
docker compose -f build/docker-compose.yml up -d db rabbitmq redis
```

Agora precisamos instalar as dependências, a partir do diretório principal execute:
```bash
cd back
dotnet restore && dotnet build && dotnet tool restore && dotnet tool install --global dotnet-ef
```

Na primeira vez será necessário efetuar as migrações do banco de dados, então executamos:
```bash
dotnet ef database update
```

Para subir a aplicação
Crie as dependencias, build
```bash
dotnet run
```

Ao fazer esse comando ele retornará o endereço para visualizar a página.