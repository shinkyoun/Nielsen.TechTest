@ECHO OFF
@ECHO.

SET CUR_PATH=%~dp0

@ECHO Current drive, %CD:~0,2%
%CD:~0,2%

@ECHO Current full path, %CUR_PATH%
cd %CUR_PATH%

dotnet watch run --project ./Nielsen.TechTest.Q1.RobotHosting/Nielsen.TechTest.Q1.RobotHosting.csproj
