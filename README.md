My attempt at learning NancyFx and Topshelf to create a RESTful API.
Assumes Redis is running on localhost.
Without Redis, change the bootloader to use stub for a persistance store.

Host defaults to running at localhost:5000, (for example, http://localhost:5000/v1/managers to return all managers)

Example usage:
POST /v1/managers/test/name=joe&company=acme
GET /v1/managers
GET /v1/managers/test
DELETE /v1/managers/test