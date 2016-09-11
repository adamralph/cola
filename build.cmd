:: the windows shell, so amazing

:: options
@echo Off
cd %~dp0
setlocal

:: determine cache dir
set NUGET_CACHE_DIR=%LocalAppData%\NuGet





:: download nuget to cache dir
set NUGET_URL="https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
if not exist %NUGET_CACHE_DIR%\NuGet.exe (
  if not exist %NUGET_CACHE_DIR% md %NUGET_CACHE_DIR%
  echo Downloading latest version of NuGet.exe...
  @powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest '%NUGET_URL%' -OutFile '%NUGET_CACHE_DIR%\NuGet.exe'"
  if %errorlevel% neq 0 exit /b %errorlevel%
)

:: copy nuget locally
if not exist .nuget\NuGet.exe (
  if not exist .nuget md .nuget
  copy %NUGET_CACHE_DIR%\NuGet.exe .nuget\NuGet.exe > nul
  if %errorlevel% neq 0 exit /b %errorlevel%
)

:: restore packages
.nuget\NuGet.exe restore Cola.sln
if %errorlevel% neq 0 exit /b %errorlevel%

:: build solution
if not exist artifacts mkdir artifacts
"%ProgramFiles(x86)%\MSBuild\14.0\Bin\msbuild.exe" Cola.sln /property:Configuration=Release /nologo /maxcpucount /verbosity:minimal /fileLogger /fileloggerparameters:LogFile=artifacts\msbuild.log;Verbosity=normal;Summary /nodeReuse:false %*
if %errorlevel% neq 0 exit /b %errorlevel%

:: run acceptance tests
packages\xunit.runner.console.2.1.0\tools\xunit.console.exe tests\ColaTests.Acceptance\bin\Release\ColaTests.Acceptance.dll -xml artifacts\ColaTests.Acceptance.xml -html artifacts\ColaTests.Acceptance.html
if %errorlevel% neq 0 exit /b %errorlevel%

:: run smoke tests
src\Cola.Console\bin\Release\cola.exe
src\Cola.Console\bin\Release\cola.exe tests\ColaTests.Smoke\hello-world-csharp-program.linq
if %errorlevel% neq 0 exit /b %errorlevel%
