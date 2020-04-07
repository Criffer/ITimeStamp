set mypath=%cd%

dotnet script %mypath%\PocosGenerator.csx -- output:APFamilyModel.cs namespace:com.apthai.APTimeStamp.Model.APFamily config:..\appsettings.json connectionstring:ConnectionStrings:DefaultConnection dapper:true
