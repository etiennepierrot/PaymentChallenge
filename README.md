# README

## Processus of design

* I started to implement a scenario of shopper registration. But the document pictogramme suggest that the Payment Gateway has no knowledge of the "shopper". The requirement didn't mention either the need of card registration for later use. So i go direct to payment. 
* I choose to use the FluentValidation package to simplify the process of validate the command who goes inside the model
* I choose to use the Either pattern for handling error. This is a questionable choice. I found this functional pattern elegant and enforcing the error workflow (see Railway Oriented Programming : https://fsharpforfunandprofit.com/rop/)
