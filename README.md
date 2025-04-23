# Observações

### Banco de Dados

1. Criar Database **ChallengeMVCStore**
2. Alterar no ConnectionStrings/DefaultConnection em:

    > \Challenge-MVC-Store.API\appsettings.json

    > \Challenge-MVC-Store\appsettings.json

3. Executar comando para atualizar o banco de dados via Migrations, criando as tabelas
4. Executar projeto **ASP.NET Core MVC** para adicionar dados inicias no Banco de Dados (SeedData)

### Pipeline

Arquivo **azure-pipelines.yml** configurado para ser executado em pool local, pois o Azure DevOps não permitiu execução usando **vmImage: 'windows-latest'**, pelo erro citado neste link: https://stackoverflow.com/questions/68405027/how-to-resolve-no-hosted-parallelism-has-been-purchased-or-granted-in-free-tie

### Arquivos avulsos

* O **gabarito de respostas teóricas** encontra-se na pasta **Perguntas Objetivas**
* As 4 queries requisitadas no **Desafio prático de SQL Server** encontra-se na pasta **Queries**
