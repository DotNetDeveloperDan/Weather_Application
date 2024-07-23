# Weather_Application
This is a quickly constructed weather application project. The requirements are listed below: 
Your task is to create a web application with the API endpoints listed below. This application will
keep track of cities provided by the API user, including attributes such as name, country,
latitude, and longitude. Additionally, you will integrate the weather.gov weather API to fetch and
display weather information for these cities.
Functional Requirements
City Management:
Track cities with the following attributes: name, country, latitude, and longitude.
Implement CRUD operations (Create, Read, Update, Delete) for city management via API
endpoints.
Weather Integration:
Use the weather.gov weather API service
(https://www.weather.gov/documentation/services-web-api) to get weather details.
Fetch weather data including temperature, humidity, and wind speed for each city.
Update weather data every 1 minute for all cities provided by the end user.
User Interface:
Create a basic UI to display the list of cities, city attributes, and the associated weather
information.
Technical Requirements
Logging:
Implement logging using Serilog.
Log all API requests and errors to a log file.
Configuration:
Create a configuration file to store the Weather API URL and other necessary configurations.
Authentication:
Implement a basic authentication system to secure your API.
API Endpoints:
POST /api/cities : Add a new city
GET /api/cities : Retrieve all cities
GET /api/cities/{id} : Retrieve a specific city by ID
PUT /api/cities/{id} : Update a specific city by ID
DELETE /api/cities/{id} : Delete a specific city by ID
GET /api/cities/{id}/weather : Retrieve current weather for a specific city by ID
GET /api/cities/{id}/weather/{date} : Retrieve weather for a specific city by ID and date
Bonus:
Use Postman or another API testing tool to demonstrate sending POST requests to your API.
