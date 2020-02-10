# README

## CI

[![.NET Core](https://github.com/etiennepierrot/PaymentChallenge/workflows/.NET%20Core/badge.svg?branch=master)](https://github.com/etiennepierrot/PaymentChallenge/actions?query=workflow%3A%22.NET+Core%22)

## Requirements

dotnet core 3.0 installed

## Run
    
    Build : dotnet build
    Test : dotnet test
    Run : dotnet run --project src/PaymentChallenge.WebApi/

### Authentication (not secure at all)

Basic Auth with the your merchantId and any password (no check implemented yet)
Example :
FancyShop:anypassword =>  Authorization: Basic RmFuY3lTaG9wOmFueXBhc3N3b3Jk

### Examples

Make a payment :

``` curl
curl --location --request POST 'http://localhost:5000/api/payments' \
--header 'Content-Type: application/json' \
--header 'Authorization: Basic RmFuY3lTaG9wOmFueXBhc3N3b3Jk' \
--data-raw '{
    "Card": {
        "CardNumber": "4242424242424242",
        "Cvv": "100",
        "ExpirationDate": "1212"
    },
    "AmountToCharge": {
        "Amount": 1000,
        "Currency": "EUR"
    },
    "MerchantReference": "a_unique_reference"
}'
```

Note : the MerchantReference is used as an idempotency key here (maybe it's better to use a non business value, like a http header)

Response :

``` json
{
  "paymentStatus": "Approved",
  "paymentId": "b528c5f6-1256-4b26-911a-1766fc06dd0f"
}
```

Retrieve payment :

``` curl
curl --location --request GET 'http://localhost:5000/api/payments/b528c5f6-1256-4b26-911a-1766fc06dd0f' \
--header 'Content-Type: application/json' \
--header 'Authorization: Basic RmFuY3lTaG9wOmFueXBhc3N3b3Jk' \
--data-raw ''
```

Response :

``` json
{
    "id": "b528c5f6-1256-4b26-911a-1766fc06dd0f",
    "card": {
        "cardNumber": "4242 XXXX XXXX 4242",
        "cvv": "100",
        "expirationDate": "1212"
    },
    "amount": {
        "amount": 1000,
        "currency": "EUR"
    },
    "status": "Approved",
    "merchantReference": "a_unique_reference"
}
```

List all payments :

``` curl
curl --location --request GET 'http://localhost:5000/api/payments/' \
--header 'Content-Type: application/json' \
--header 'Authorization: Basic RmFuY3lTaG9wOmFueXBhc3N3b3Jk' \
--data-raw ''
```

Response :

``` json
[
    {
        "id": "b528c5f6-1256-4b26-911a-1766fc06dd0f",
        "card": {
            "cardNumber": "4242 XXXX XXXX 4242",
            "cvv": "100",
            "expirationDate": "1212"
        },
        "amount": {
            "amount": 1000,
            "currency": "EUR"
        },
        "status": "Approved",
        "merchantReference": "a_unique_reference"
    }
]
```

### Run with docker

``` bash
docker build -t paymentchallenge:build .
docker run --rm -it -p 8080:5000 paymentchallenge:build
```

Swagger documentation : [Link](http://localhost:8080/index.html)

## Process of design

* I started to implement a scenario of shopper registration. But the document pictogram suggest that the Payment Gateway has no knowledge of the "shopper". The requirement didn't mention either the need of card registration for later use. So i go direct to the implemntation of payment's endpoints.
* I choose to use the FluentValidation package to simplify the process of validate the command who goes inside the model
* I choose an Hexagonal Architecture for testing and for being able to replace easily the mock of the bank by a real implementation. Hexagonal architecture allow me to fullfill the requirement without using a real Database. 
* I didn't log in filesystem but in console, so logs are treated as stream of events (12 factors)
  

## Questionable choices

* I use a lot of strong typed value (MerchandId, PaymentId ..). The reason : recently i was interested about Type Driven Development (https://blog.ploeh.dk/2015/08/10/type-driven-development/), it work like a charm in F# because F# has immutability and equality for Free. In C#, we need a lot a ceremonial stuff ... 
But i think it's still useful, because we avoid "primitive obsession" and confusion between the merchant, the payment gateway and bank reference.
* I choose to use some monads for handling error. This is a questionable choice. I found this functional pattern elegant and enforcing the error workflow (see Railway Oriented Programming : https://fsharpforfunandprofit.com/rop/) in F#. I have tried the library LanguageExt who tried to implement Functional Pattern in C#. And like Type Driven Development, it's not as good as C# (essentialy because C# didn't support Union and Pattern Matching like F#). So, i'm not totally satisfied by this choices.
* I didn't use the IConvention for IInterface, because i found this convention useless (IMHO i found that the client of an interface didn't need to known if if he call an real implementation of an interface accordingly to Liskov principle). Of course, in a team where people want really use this convention, and if i can't convinced them that this convention isn't useful, i will use this convention.
* Of course, a lot of over engineering here, in real situation, i'll go for simpler solution

## TODO

* Implement a real Basic Auth with hashed passphrases and OAuth protocol ( https://www.oauth.com/oauth2-servers/server-side-apps/)
* Logging with PCI DSS compliance ()
* add unit of work pattern
* add pagination on list endpoints
* End to end integration tests, Property Based Testing
* True persistence with database (postgres would be an interesting candidate). Even with Hexagonal Architecture, i think my domain should need some adaptation (maybe implement a proxy pattern for loading the payment from database)
* Better retry policy with Circuit Breaker
* Add more constraints (size, regex,) on API input validations and improve error messages
* Improve mapping and validation Dto => Model  (maybe with automapper)
* Continuous Deployment 
* And so many things ...
