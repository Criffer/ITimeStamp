set mypath=%cd%

dotnet script %mypath%\PocosGenerator.csx -- output:APFamilyModel.cs namespace:com.apthai.APTimeStamp.Model.CRMWeb config:..\appsettings.json connectionstring:ConnectionStrings:DefaultConnection dapper:true
