INSTALLATION NOTES

Software Requirements
1.	Visual Studio 2012 
2.	Visual Studio 2012 Update 4, this can be downloaded from the link http://www.microsoft.com/en-in/download/details.aspx?id=39305 . Offline downloaded software is available with the support team.

Optional software’s

1.	SQL Sever 2012, Its not necessary to have SQL Server 2012.Application can use the free LocalDB that comes with Visual Studio 2012 Update 4
2.	Modeling SDK for Visual Studio 2012. This is require din case UML modelling features of Visual Studio is utilized.
	
DATABASE INSTALLATION - MS SQL SERVER 2012 Version

Database can be installed using 2 different options as mentioned below
1. Using SQL Server Local DB. This does not require any software installation and its bundled with Visual Studio 2012. Refer to document "Configuring Portal database in  free local db version.docx" for configuring database in the local db.
2.In SQL Server 2012 Express/Developer/Enterprise editions. Refer to the following steps for installing the database in SQL Server 2012.

	1. Create the folder "D:\Data" in the database server. This path is hardcoded for sql data and log files in DatabaseSchemaScript.sql file
		If there is any change in path then it has to be updated in DatabaseSchemaScript.sql
	2. Open SQL Server Enterprise Manager and execute the SQL file DatabaseSchemaScript.sql file and execute it. 
	3. Verify that database by name CSGPortal is created successfully.
	3. Run the LookupDataScript.sql file against CSGPortal database
	4. Run the SampleDataScipt.sql file against CSGPortal database.

APPLICATION SERVER INSTALLATION

1. Update the Server instance name in the Connection string key in Config file for the following projects.
   1. MT.CSGPortal.UI
   2. MT.CSGPortal.Services
   3. MT.CSGPortal.UI.Tests
   4. MT.CSGPortal.Services.Tests
   5. MT.CSGPortal.Utility.Tests
2. Update the value of 'IsADMocked' in the appsettings section of .config files to '1' to mock the ActiveDirectory service.
   <add key="IsADMocked" value="1"/> - Mock AD
   <add key="IsADMocked" value="0"/> - Search in actual AD (should be connected to mindtree n/w).
   
   Note:When AD is mocked results obtained will not be according to search criteria. (for AD search only).



