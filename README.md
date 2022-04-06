# Pokedex API

This solution includes a seris of API endpoints designed to retrieve information on a user specified Pokemon.

This appication is built in ASP.NET Core 6.0 and built using Docker.

## Installation

These instructions assume that you have cloned this repository to your machine.

### Add an appsettings file

From the root directory of the repository, navigate to PokedexAPI/PokedexAPI.  
Add a file called appsettings.json with the following contents  
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "PokeApiUrl": "https://pokeapi.co/api/v2",
  "TranslatorApiUrl": {
    "Shakespeare": "https://api.funtranslations.com/translate/shakespeare.json",
    "Yoda": "https://api.funtranslations.com/translate/yoda.json"
  }
}
```

### Installing Docker

To run this application, you will need to have Docker installed. To install Docker on your machine, please navigate [here](https://www.docker.com/products/docker-desktop/) and follow the instructions provided.

## Running the application

From the root directory of the repository, navigate to PokedexAPI/PokedexAPI.  
Open a command prompt from this directory.
Run the following commands
```
docker build --tag pokedexbuild .
docker run -d -p 51234:80 --name testbuild pokedexbuild
```

The application is now running at 
```
localhost:51234
```

## What I would do differently in production

- Require an API key to access the endpoints
- Implement load testing
- Add rate limitng to the endpoints
- Add more exception handling as currently I am only handeling exceptions when a pokemon is not found using the PokeAPI
- Implement health checking tools
  - Sentry
    - Error capture
    - Performance tracing
  - Logging (I have included the loggers for each class but have not implemented them fully)

