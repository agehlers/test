@echo off
echo Running Sonar Tests
SonarQube.Scanner.MSBuild.exe begin /k:"HETSAPI" /n:"HETSAPI" /v:1.0
"D:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\amd64\MSBuild.exe" /t:Rebuild
SonarQube.Scanner.MSBuild.exe end
