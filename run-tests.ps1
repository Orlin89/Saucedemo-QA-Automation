# Задаване на променлива за среда
$allureEnv = "Local"
$env:ALLURE_ENV = $allureEnv
$resultsDir = "TestResults\$allureEnv"

# Почистване на стари резултати
Remove-Item -Recurse -Force $resultsDir -ErrorAction SilentlyContinue
Remove-Item -Recurse -Force AllureReport -ErrorAction SilentlyContinue

# Създаване на нова папка за обединени резултати
New-Item -ItemType Directory -Force -Path $resultsDir | Out-Null

# Стартиране на UI тестовете
dotnet test SauceDemoAutomationUI/SauceDemoAutomationUI.csproj `
  --logger:"console;verbosity=detailed"

# Стартиране на API тестовете
dotnet test SauceDemoAutomationAPI/SauceDemoAutomationAPI.csproj `
  --logger:"console;verbosity=detailed"

# Копиране на Allure резултати от UI проекта
$uiResults = "SauceDemoAutomationUI\bin\Debug\net8.0\allure-results"
if (Test-Path $uiResults) {
    Copy-Item "$uiResults\*" -Destination $resultsDir -Recurse -Force
}

# Копиране на Allure резултати от API проекта
$apiResults = "SauceDemoAutomationAPI\bin\Debug\net8.0\allure-results"
if (Test-Path $apiResults) {
    Copy-Item "$apiResults\*" -Destination $resultsDir -Recurse -Force
}

# Запазване на история от предишни отчети (ако има)
if (Test-Path AllureReport\history) {
    Copy-Item AllureReport\history\* "$resultsDir\history" -Recurse -Force
}

# Генериране на Allure отчета
allure generate $resultsDir --clean -o AllureReport

# Отваряне на Allure отчета
allure open AllureReport
