# flash-card-identity
# flash-card
Flash Card Identity API Technology: 
  Back-end: .net 8 , JWT token, SMSglobal, Entity Framework core
  Database: MS Sql Server

	I build this identity API for purpose to request and verify otp code sent by SMSGlobal
	also create pair of access token and refresh token
	
	In case access token expired we can use refresh token to regrant new access token .

Note : for mobile number. please use Australian mobile to get otp code

Set up instructions:

	Under project FlashCard.API, create folder App_Data/key if not exist

	Add PrivateSigning.key to App_Data/key
	Add SmsGlobal-Key.json
	Rebuild solution

Completed feature : 

	-Calling API of SMSglobal to request and verify otp

	-API Create access token and refresh token

	-API refresh access token using refresh token

	-Middleware to hanlde Exception globally

	-Apply clean architechture

	-API caching following configuration

Known issue and limitations :
	-Custom Authorize Attribute not working
	-Validate Signature of JWT token got issue due to using Openssl to generate security key not correct
	-so It affect to API CRUD of card when we call API without authentication
	-Create an UI app to integrate
Future improvement
  -implement rsaSecurityKey and signingCredentials for jwt token
  - Move key to azure key vault
API is publish on azure https://identityapi20241010055828.azurewebsites.net/index.html