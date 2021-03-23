# Investment Hub Server

This is a side project to fetch for data in different investment brokers/banks and aggregate them into a single source of data for personal analysis. This project is the server side that will be used by the [InvestmentHubPortal](https://github.com/VictorBalbo/InvestmentHubPortal)

## Integrations 
Right now the system is successfully integrated to:
1. Rico Provider, being able to log in and fetch for the assests on the broker. *Rico provider uses a 2 factor authentication, so to fetch data from the provider, is necessary to send a token on the request.*
2. Nubank, being able to get the balance from Nuconta and the value of the credit card debt (Integration was inspired on this [repo](https://github.com/lira92/nubank-dotnet)). *Nubank provider uses a 2 factor authentication, so to fetch data from the provider, is necessary to scan a QrCode inside Nubank App before sending the request.*

## Roadmap
* Find a way to update providers assets automatically for the providers that use 2 factor authentication.
* Change and forgot my password endpoits.
