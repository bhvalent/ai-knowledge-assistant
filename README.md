## Console App
The console app has 3 commands: `save`, `search`, `ask`, `exit`

- **Save**
    - This will prompt you for a phrase, then the app will embed and save into the vector database
- **Search**
    - This will prompt you for a query, then will embed the query and search the vector database for the closest item.
- **Ask**
    - This will prompt you for a question, then will embed the question, search the vector database for the closest relevent context, then call the LLM with the context for a response to the question.
- **Exit**
    - Exits the program

## Setup
- Since this uses OpenAi API, you will need to create an account and create an api key. You will probably need to add a credit card to add $5. This doesn't auto charge

### Add User Secret
You will need to add the user secret to the Console App

Navigate to the AiKnowledgeAssistant.ConsoleApp folder

```
> dotnet user-secret init
> dotnet user-secret set "OpenAi:Key" <api-key-here>
> dotnet user-secret set "OpenAi:Url" "https://api.openai.com/"
> dotnet user-secret set "ConnectionStrings:AiDb" "Host=localhost;Database=ai_db;Username=postgres;Password=postgres"
```

### Database
```
docker run -d --name pgvector-db -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=ai_db -p 5432:5432 ankane/pgvector

```