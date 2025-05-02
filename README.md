# enV Vehicles Solution

This solution is designed to manage vehicle data, including importing vehicle information from CSV files, retrieving vehicle details, and augmenting vehicle data using external sources like the NHTSA vPIC API.

## Prerequisites

To run this application, ensure you have the following installed:

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- A code editor or IDE such as [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)

## How to Run the Application

Follow these steps to build and run the application:

### 1. Clone the Repository
Clone the repository to your local machine:
git clone <repository-url> cd <repository-folder>


### 2. Build the Application
Restore the dependencies and build the application:
dotnet build


### 3. Run the Application
Run the application using the following command:
dotnet run --project enV.Vehicles

The application will start and listen on the ports specified in launch profile (e.g., `https://localhost:7115`).

### 4. Access the API
You can access the API endpoints using tools like [Postman](https://www.postman.com/) or [curl](https://curl.se/).
If the application is running in development, you can access the swagger page at `https://localhost:7115/swagger` to view the API documentation and test the endpoints.


#### Import a CSV File
- **Endpoint**: `POST /Vehicle/import-csv`
- **Description**: Upload a CSV file to import vehicle data.
- **Example**:
curl -X POST -F "file=@path/to/vehicles.csv" https://localhost:7115/Vehicle/import-csv


#### Get Vehicle by VIN
- **Endpoint**: `GET /Vehicle/get-by-vin/{vin}`
- **Description**: Retrieve vehicle details by VIN.
- **Example**:
curl https://localhost:7115/Vehicle/get-by-vin/1HGCM82633A123456


#### List Vehicles with Pagination and Filtering
- **Endpoint**: `GET /Vehicle/list`
- **Description**: List vehicles with pagination and optional filtering by dealer ID or modification date.
- **Example**:
curl "https://localhost:7115/Vehicle/list?pageNumber=1&pageSize=10&dealerId=1"


#### Retrieve Imported CSV
- **Endpoint**: `GET /Vehicle/get-imported-csv/{csvIdentifier}`
- **Description**: Retrieve the imported CSV file by its identifier.
- **Example**:
curl -O https://localhost:7115/Vehicle/get-imported-csv/{csvIdentifier}


#### Augment Vehicles with External Data
- **Endpoint**: `POST /VehicleAugmenting/augment`
- **Description**: Augment vehicle data using the NHTSA vPIC API.
- **Example**:
curl -X POST https://localhost:7115/VehicleAugmenting/augment


## Configuration

### App Settings
The application uses default configurations. If you need to customize settings (e.g., database connection, API keys), update the `appsettings.json` file in the `enV.Vehicles` project.

### Dependencies
Ensure the following dependencies are installed:
- LiteDB for data storage
- CsvHelper for CSV parsing
- IHttpClientFactory for making HTTP requests to external APIs

## Testing

To run the unit tests for the application, use the following command:
dotnet test


## Additional Notes

- Ensure that the NHTSA vPIC API is accessible for augmenting vehicle data.
- The application uses LiteDB for local data storage. The database file (`vehicles.db`) will be created in the application's working directory.

For any issues or questions, please contact Tim Ross at timross.software@gmail.com.