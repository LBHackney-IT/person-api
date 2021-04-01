# Running a local coverage report

It is easily possible to gerenate a full interactive coverage report to see where the test coverage may be lacking.

* Install the dotnet report generator (this only need be done once)
    ```bash
    dotnet tool install -g dotnet-reportgenerator-globaltool
    ```
* Ensure that the test project has a NuGet reference to the latest *coverlet.msbuild* package.
* Open a command window in the root of the test project and run the handy `RunCoverage.bat` file located there.
    ```bash
    .\RunCoverage.bat
    ```
* This will generate the coverage data, convert it into a report and then open it for you to view.
