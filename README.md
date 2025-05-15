# UsageTranslator

A C# console application that reads usage data from a CSV file and maps it to product codes using a JSON typemap. It generates SQL `INSERT` statements for two normalized database tables: `chargeable` and `domains`.

This application is designed to be modular and maintainable, with a focus on clean code practices. It includes unit tests to ensure the correctness of the functionality.

---

## Features

- Reads and parses `Sample_Report.csv`
- Maps `PartNumber` to product using `typemap.json`
- Cleans `accountGuid` to alphanumeric string
- Skips invalid or excluded records (e.g. `PartnerID == 26392`)
- Applies unit reduction rules for specific part numbers
- Tracks and logs total usage per product
- Outputs two SQL insert blocks
- Designed with clean separation of concerns (Parser, Mapper, SQL Builder)
- Includes unit tests using xUnit

---

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- Visual Studio 2022+ or any text editor

---

## Project Structure

<pre> ``` UsageTranslator/ ├── Data/ │ ├── Sample_Report.csv │ └── typemap.json ├── Models/ ├── Services/ ├── Output/ │ └── output.sql ├── Program.cs └── README.md ``` </pre>

## Run the Application
You can run this application using the command line or from within Visual Studio.

### Command Line
1. Open a terminal and navigate to the project directory.
2. Run the following command:
   ```bash
   dotnet run
   ```
1. The output will be generated in the `Output` directory as `output.sql`.

### Visual Studio
1. Open the project in Visual Studio.
1. Set the project as the startup project.
1. Press `F5` or click on the "Start" button to run the application.
1. The output will be generated in the `Output` directory as `output.sql`.
1. You can also run the unit tests by right-clicking on the project in Solution Explorer and selecting "Run Tests" or using the Test Explorer window.
1. The application will read the `Sample_Report.csv` file, process the data, and generate SQL insert statements for the `chargeable` and `domains` tables. The output will be saved in the `Output` directory as `output.sql`.
1. You can open the `output.sql` file to view the generated SQL statements.
