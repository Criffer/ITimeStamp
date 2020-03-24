set mypath=%cd%

dotnet script %mypath%\PocosGenerator.csx -- output:CRMMobile.cs namespace:com.apthai.APTimeStamp.Model.CRMMobile config:..\appsettings.json connectionstring:ConnectionStrings:DefaultMobileConnection dapper:true
