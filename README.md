#BadBroker

A BadBroker determines what will be the best income when selling / buying currency for a certain historical period

To get rates used service https://exchangeratesapi.io/

Please note that you need to fill the HTDC_API_KEY env variable with a private key.
Also you need to fill the HTDC_EXCHANGE_RATES_ENDPOINT env variable with a service endpoint.

Example Request and Response

curl -X GET
"http://localhost:5000/rates/best?startDate=2014-12-15&endDate=2014-12-23&moneyUsd=100" -H
"accept: application/json"


{
	"rates": [
		{
		"date": "2014-12-15T00:00:00",
		"rub": 60.1735876388,
		"eur": 0.8047642041,
		"gbp": 0.6386608724,
		"jpy": 118.8395300177
		},
		// ...
	],
	"buyDate": "2014-12-16T00:00:00",
	"sellDate": "2014-12-22T00:00:00",
	"tool": "RUB",
	"revenue": 27.258783297622983
}
