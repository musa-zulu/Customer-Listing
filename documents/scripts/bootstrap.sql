-- use master;
-- go
-- ALTER DATABASE  [CustomerListing]
-- SET SINGLE_USER
-- WITH ROLLBACK IMMEDIATE
-- drop database [CustomerListing]
-- go
------ normal creation after here
use master;
go
if not exists (select name from master..syslogins where name = 'CustomerListingWeb')
    begin
        create login CustomerListingWeb with password = 'P4$$w0rd';
    end;
go


if not exists (select name from master..sysdatabases where name = 'CustomerListing')
begin
create database CustomerListing
end;
GO

use CustomerListing
if not exists (select * from sysusers where name = 'CustomerListingWeb')
begin
create user CustomerListingWeb
	for login CustomerListingWeb
	with default_schema = dbo
end;
GO
grant connect to CustomerListingWeb
go
exec sp_addrolemember N'db_datareader', N'CustomerListingWeb';
go
exec sp_addrolemember N'db_datawriter', N'CustomerListingWeb';
go
exec sp_addrolemember N'db_owner', N'CustomerListingWeb';
GO
use master;
GO

