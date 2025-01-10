# Project setup

## Backend

1. Open appsetting.json file and change the connection string to the connection string of your local MSSQL database (If the database specified in connection string doesn't exist, starting the app will automatically create it and migrate tables):

```json
"ConnectionStrings": {
  "InforceDb": "Your connection string"
}
```

After that, you can start the API.

## Frontend

1. Open terminal in the project folder and execute the command:

```
npm install
```

2. After you launched API, you can start the client:

```
ng serve -o
```
