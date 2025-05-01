# SauceDemo Automation Solution

## 📋 Project Description

This is a full automation solution for UI E2E tests and API tests using:

- Selenium WebDriver
- NUnit
- RestSharp (API testing)
- Allure Reports
- GitHub Actions CI/CD Pipeline

The project is separated into two parts:
- **SauceDemoAutomationUI** — Web UI E2E tests for https://www.saucedemo.com/
- **SauceDemoAutomationAPI** — API tests (example purposes)

Allure reporting is integrated both locally and in CI.
Tests are executed in parallel across Chrome, Firefox, and Edge browsers with multiple users.

---

## ⚙ Technologies

| Technology           | Version         |
|-----------------------|-----------------|
| .NET                  | 8.0             |
| Selenium WebDriver    | 4.19.0          |
| NUnit                 | 3.14.0          |
| RestSharp             | 110.2.0         |
| WebDriverManager      | 2.11.3          |
| Allure NUnit Adapter  | 2.13.9          |

---

## 🧪 How To Run Locally

1. Install .NET 8 SDK
2. Install Allure Commandline:
    ```bash
    npm install -g allure-commandline --save-dev
    ```
3. Restore NuGet packages:
    ```bash
    dotnet restore
    ```
4. Run UI tests:
    ```bash
    dotnet test SauceDemoAutomationUI/SauceDemoAutomationUI.csproj
    ```
5. Run API tests:
    ```bash
    dotnet test SauceDemoAutomationAPI/SauceDemoAutomationAPI.csproj
    ```
6. Generate local Allure report:
    ```bash
    allure generate TestResults --clean -o AllureReport
    allure open AllureReport
    ```

---

## 🛠 GitHub Actions

CI/CD pipeline will automatically:
- Trigger on every push/commit to `main` branch
- Build and Test the project
- Upload and generate Allure Reports

---

## 🚀 Parallel Execution

- Chrome, Firefox, and Edge browsers are supported.
- Tests are run for multiple SauceDemo users: `standard_user`, `problem_user`, `performance_glitch_user`.
- Pass browser as parameter using NUnit command-line:

```bash
dotnet test SauceDemoAutomationUI/SauceDemoAutomationUI.csproj -- TestRunParameters.Parameter(name=\"browser\", value=\"firefox\")
