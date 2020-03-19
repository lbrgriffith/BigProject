# BigProject
Example for logging to database

## Add Connection String
An ordinary connection string is used to access your desired database:
```xml
<connectionStrings>
  <add name="NLog" connectionString="Data Source=.\;Initial Catalog=MyDatabase;Integrated Security=true;" providerName="System.Data.SqlClient" />
</connectionStrings>
```

## Create Table And Stored Procedure to Log Exceptions
Create the table to hold the log data and a stored procedure that will insert data into the table.
```sql
/*
 Logs table will hold log messages
*/

CREATE TABLE [dbo].[Logs](
[LogId] [int] IDENTITY(1,1) not null,
[Level] [varchar](20) not null,
[CallSite] [varchar](50) not null,
[Type] [varchar](128) not null,
[Message] [varchar](max) not null,
[StackTrace] [varchar](max) not null,
[InnerException] [varchar](max) not null,
[AdditionalInfo] [varchar](max) not null,
[LoggedOnDate] [datetime] not null constraint [df_logs_loggedondate] default (getutcdate()),

constraint [pk_logs] primary key clustered
(
[LogId]
))

GO

/*
Create stored procedure for adding entries to the database.
*/

CREATE procedure [dbo].[InsertLog]
(
  @level varchar(20),
  @callSite varchar(50),
  @type varchar(128),
  @message varchar(max),
  @stackTrace varchar(max),
  @innerException varchar(max),
  @additionalInfo varchar(max)
)
as
BEGIN
  SET NOCOUNT ON;

  INSERT INTO dbo.Logs
  (
  [Level],
  CallSite,
  [Type],
  [Message],
  StackTrace,
  InnerException,
  AdditionalInfo)
  VALUES
  (
  @level,
  @callSite,
  @type,
  @message,
  @stackTrace,
  @innerException,
  @additionalInfo);
END
GO
```

## Configure Target and Logger
You will need to add configuration settings for NLog to call the dbo.InsertLog stored procedure:

_NOTE: Full [config file](https://github.com/lbrgriffith/BigProject/blob/master/NLog.config) is included in this repository._
```xml
<targets>
<!-- database target -->
<target name="database"
xsi:type="Database"
connectionStringName="NLog"
commandText="exec dbo.InsertLog
@level,
@callSite,
@type,
@message,
@stackTrace,
@innerException,
@additionalInfo">
<parameter name="@level" layout="${level}" />
<parameter name="@callSite" layout="${callsite}" />
<parameter name="@type" layout="${exception:format=type}" />
<parameter name="@message" layout="${exception:format=message}" />
<parameter name="@stackTrace" layout="${exception:format=stackTrace}" />
<parameter name="@innerException"
layout="${exception:format=:innerFormat=ShortType,Message,Method:MaxInnerExceptionLevel=1:InnerExceptionSeparator=}" />
<parameter name="@additionalInfo" layout="${message}" />
</target></targets>
<rules>
<!-- database logger -->
<logger levels="Error,Warn,Fatal" name="databaseLogger" writeTo="database"/></rules>
```

## Useful Links
1. [NLog Documentation](https://github.com/NLog/NLog/wiki/Tutorial) for .NET Framework(Config options for NLog's configuration) 
2. [Database target](https://github.com/NLog/NLog/wiki/Database-target) Writes log messages to the database using an ADO.NET provider.
