# run-tests.ps1

# Clean up previous results
Remove-Item -Recurse -Force TestResults, AllureReport -ErrorAction SilentlyContinue

# Create merged results directory
$mergedResultsDir = "TestResults"
New-Item -ItemType Directory -Force -Path $mergedResultsDir | Out-Null

# Run UI tests
dotnet test SauceDemoAutomationUI/SauceDemoAutomationUI.csproj --logger:"console;verbosity=detailed"

# Run API tests
dotnet test SauceDemoAutomationAPI/SauceDemoAutomationAPI.csproj --logger:"console;verbosity=detailed"

# Copy results from UI project
$uiAllureDir = "SauceDemoAutomationUI/bin/Debug/net8.0/TestResults"
if (Test-Path $uiAllureDir) {
    Copy-Item "$uiAllureDir/*" -Destination $mergedResultsDir -Recurse -Force
}

# Copy results from API project
$apiAllureDir = "SauceDemoAutomationAPI/bin/Debug/net8.0/TestResults"
if (Test-Path $apiAllureDir) {
    Copy-Item "$apiAllureDir/*" -Destination $mergedResultsDir -Recurse -Force
}

# Generate Allure report
allure generate $mergedResultsDir --clean -o AllureReport

# Open Allure report
allure open AllureReport
