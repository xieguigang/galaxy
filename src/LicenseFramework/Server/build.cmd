@echo off

set reflector="\graphQL\src\mysqli\App\Reflector.exe"
set R_src="./KeySigned/mysql"
set sql="./license_svr.sql"

CALL %reflector% --reflects /sql %sql% -o %R_src% /namespace license_svrModel --language visualbasic /split /auto_increment.disable

REM pause