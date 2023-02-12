# Game Rating Service

This project is a [stateless](https://en.wikipedia.org/wiki/Service_statelessness_principle) micro-service that serves a RESTful API made with [.NET 6](https://docs.microsoft.com/es-es/aspnet/core/?view=aspnetcore-6.0) that will allow players to rate a game session by giving it a numeric score between 1 and 5 (min and max are configurable) and also add a comment.

Using [Domain Driven Design](https://en.wikipedia.org/wiki/Domain-driven_design) architecture and following [SOLID](https://en.wikipedia.org/wiki/SOLID) principles.

Also using:
- Entity Framework
- Fluent Validation

The API can be served using [Docker](https://docs.docker.com/get-started/overview/) containers.

##  Clone, Build and Run 
* `git clone https://github.com/blacksmith94/Game.RatingService.git`
* `cd Game.RatingService`
* `docker-compose build`

Make sure you have an instance of SQL Server before running the [Integration](https://en.wikipedia.org/wiki/Integration_testing) and [Unit](https://en.wikipedia.org/wiki/Unit_testing) tests and the service.

To bring up the service, including the creation of the SQL Server container, run this command from the root folder:
* `docker-compose up`

To bring up only a configured instance of SQL Server run this command:
* `docker-compose up sql`


The API will be served at http://localhost:5001/api

##  Endpoints
`POST` Endpoint to send the rating of a game session as a user 
* `POST -> http://localhost:5001/api/Rating/{sessionId}`

    The `sessionId` in the path must be a Guid.

    The HTTP request headers must contain the key `userId` and a string as a value.

    The body contains the score (required) and the comment (optional)

    Body example:
        `{
            "score": 4,
            "comment": "Nice minigame!"
        }`


`GET` Endpoint to fetch the list of the latest ratings, it accepts a query param to filter ratings by the score that the users specified. 
* `GET -> http://localhost:5001/api/Rating?rating=3`


## OpenAPI Specification

The project includes API documentation with [Swagger](https://swagger.io/).

Served at http://localhost:5001/swagger


## Configuration
The following settings can be changed in the `appsettings.json` file to adapt the service to your needs:

`MaxLatestRating`: The max number of ratings that will be fetched when using the `GET` endpoint.
    
`MinAllowedScore`: The minimum score that a user is allowed to specify.
    
`MaxAllowedScore`: The maximum score that a user is allowed to specify.
