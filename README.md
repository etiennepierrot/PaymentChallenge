# README

## Process of design

* I started to implement a scenario of shopper registration. But the document pictogram suggest that the Payment Gateway has no knowledge of the "shopper". The requirement didn't mention either the need of card registration for later use. So i go direct to payment. 
* I choose to use the FluentValidation package to simplify the process of validate the command who goes inside the model
* I choose an Hexagonal Architecture for testing and for being able to replace easily the mock of the bank by a real implementation.

## Questionable choices 

* I use a lot of strong typed value (MerchandId, PaymentId ..). The reason : recently i was interested about Type Driven Development (https://blog.ploeh.dk/2015/08/10/type-driven-development/), it work like a charm in F# because F# has immutability and equality for Free. In C#, we need a lot a ceremonial stuff ... 
But i think it's still useful, because we avoid "primitive obsession" and confusion between the merchant, the payment gataway and bank reference.
* I choose to use some monads for handling error. This is a questionable choice. I found this functional pattern elegant and enforcing the error workflow (see Railway Oriented Programming : https://fsharpforfunandprofit.com/rop/).
* I didn't use the IConvention for IInterface, because i found this convention useless (IMHO i found that the client of an interface didn't need to known if if he call an real implementation of an interface accordingly to Liskov principle). Of course, in a team where people want use this convention, i will use this convention.

## TODO
* Implement a real Basic Auth with hashed passphrases and OAuth protocol (https://www.oauth.com/oauth2-servers/server-side-apps/)
* Logging with PCI DSS compliance
* add unit of work pattern
* add pagination of list payments endpoints