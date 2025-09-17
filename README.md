## Setup
- Since this uses OpenAi API, you will need to create an account and create an api key. You will probably need to add a credit card to add $5. This doesn't auto charge

## Console App
The console app adds the following sentences as "documents" so that you can see how the embedding works.
```
"The quick brown fox jumps over the lazy dog."
"C# is a modern, object-oriented programming language."
"SQL Server is a relational database management system by Microsoft."
"Angular is a TypeScript-based front-end framework."
```
1. All these sentences are embedded separately
2. You write out a query like "Angular"
3. Your query is then embedded in order to be compared to the sentences
4. The sentence with the closest score is returned.

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