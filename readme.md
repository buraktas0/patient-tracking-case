# Patient Tracking

Simple clinical monitoring system demo.

## Instructions

To start web application, run:

```bash
ng serve
```

`http://localhost:4200/`


To start backend application, run:


```bash
dotnet run
```

API documentation:

`http://localhost:5000/swagger/index.html`


## Technologies

- Angular
- Tailwind
- .NET
- PostgreSQL

## Details

- Responsive Design
- JWT Authentication (register, login, logout, refresh-token)
- Access log middleware implementation
- PostgreSQL and Entity Framework
- Patient list page, patient detail page, create, update, delete operations with auth guard and token-based api access.

## Possible Improvements

- Seperation of prediction service
- SignalR implementation for prediction results
- Better ui/ux practices. Especially with error handling at web app for displaying api responses/errors.

