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

2. **Clonar o repositório**:
   ```bash
        git clone https://github.com/MagmaUSP/CursinhoEACH.git
        cd CursinhoEACH
   ```

3. **Executar projeto**:
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
        dotnet build
    ```
    - Rodar a aplicação:
    ```bash
        dotnet run
    ```
    - Aplicação ficará disponível em:
     ```
        https://localhost:<port#>
    ```
    