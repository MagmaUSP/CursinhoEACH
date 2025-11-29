# CursinhoEACH

Sistema em **.NET Core MVC** para auxiliar o Cursinho Popular EACH – USP no controle de assiduidade e desempenho dos alunos.

---
## Organização
|_ doc
|_ src
    |_ 


---

## Pré-requisitos

1. **Instalar o .NET SDK 9.0**  
   Baixe e instale o SDK compatível com seu sistema operacional:  
   [Download .NET 9.0.304](https://dotnet.microsoft.com/pt-br/download/dotnet/9.0)

   - Windows: escolha **x64 Installer** (a maioria dos PCs).  
   - macOS: escolha **x64** ou **Arm64**, dependendo do processador.  
   - Linux: siga as instruções do gerenciador de pacotes.
        - Atualizar pacotes e instalar .NET SDK
        ```bash
        sudo apt-get update
        sudo apt-get install -y dotnet-sdk-9.0
        dotnet --version
        ```

2. **Clonar o repositório**:
   ```bash
        git clone https://github.com/MagmaUSP/CursinhoEACH.git
        cd CursinhoEACH
   ```
3. **Configurar postgreSQL (Linux)**
    - Instalar pacote:
    ```bash
    sudo apt-get install postgresql
    ```
    
    - Trocar para o usuário postgres:
    ```bash
    sudo -i -u postgres
    ```
    
    - Acessar o PostgreSQL:
    ```bash
    psql
    ```
    
    - Alterar a senha do usuário postgres (se necessário):
    ```sql
    ALTER USER postgres WITH PASSWORD '1234';
    ```

    - Checar os bancos existentes:
    ```sql
    SELECT datname FROM pg_database;
    ```
    
    - Sair do psql:
    ```sql
    \q
    ```
    
    - Sair do usuário postgres:
    ```bash
    exit
    ```
    
    - Testar conexão:
    ```bash
    psql -h localhost -p 5432 -U postgres -d postgres -c "SHOW search_path;"
    ```


4. **Executar projeto**:
    - Confiar no certificado HTTPS de desenvolvimento:
    ```bash
        dotnet dev-certs https --trust
    ```

    - Alterar a string de conexão com o banco de dados, no arquivo appsettings.json, para as informações do seu PostgreSQL (senha, host, etc)
    ```
        Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=1234;SearchPath=cursinho_each
    ```
    - Dar build na aplicação:
    ```bash
        cd src
        dotnet build
    ```
    - Rodar a aplicação:
    ```bash
        dotnet run
    ```
    - A aplicação ficará disponível em (verifique a saída do comando `dotnet run` para confirmar as portas):
    ```
    http://localhost:5000
    ou
    https://localhost:5001
    ```
    - Em caso de necessidade de reinicialização do banco com os dados default do script de inicialização:
    ```bash
    sudo -i -u postgres
    psql
    ```
    ```sql
    DROP DATABASE cursinho_each;
    SELECT datname FROM pg_database;
    ```

    # Bugs encontrados

    # Bugs corrigidos
    1. DropDown Matéria na pagina Turma está estático, não bate com novas inclusões. Ajustados os valores.
    1. Ocupação de turma mostrando a metade do valor real, capacidade e matriculados puxados do banca parecem corretos.
    1. Presenca em simulados e aulas esta zuada, nao bate com os dados do banco.

    # Implementações de interesse
    1. Filtro de turma e ano no painel de Alunos para visualização de desempenho e presença
    1. Aba de cadastro de grade horária (professor por turma/horário/ano)
        - Fixa para o ano todo, sem opção de mudanças pontuais.
        - Já cria todos os eventos_turma
    1. Aba de controle de presença
        - Filtro de data turma, cpf, nome e presentes
            - Click para registra presença no evento
            