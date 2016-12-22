Desmat Git tool

This project enables one to access a git repository and extract valuable information regarding Commits, dates, tickets and versioning.

By default GitVersionTool.exe is located in GitVersionTool\bin\Debug.

Application writes three files (A .txt, .csv, .htm)
All these files contain information about Author, Committer, Message, Date and Time
The .htm file has an added link to the assembla ticket

For different subdivisions of repository the following can be used:
Choices for OPTION are:

	+	"-displayAll"
	+	"-displayBetweenTwoDates"
	+	"-searchPhrase"
	+	"-displayGroupedTickets"
	+	"-displayChangesBetweenLastTwoBuilds":

To run in each commandline the following is required:

In general: GitVersionTool.exe <Path to directory> <Name of file to be written to> <Path to where file will be written> <OPTION> [Parameters]

"-displayAll":							GitVersionTool.exe <Path to directory> <Name of file to be written to> <Path to where file will be written> -displayAll
"-displayBetweenTwoDates"				GitVersionTool.exe <Path to directory> <Name of file to be written to> <Path to where file will be written> -displayBetweenTwoDates "dd-mm-yyyy" "dd-mm-yyyy"
"-searchPhrase"							GitVersionTool.exe <Path to directory> <Name of file to be written to> <Path to where file will be written> -searchPhrase "Any phrase to be searched"
"-displayGroupedTickets"				GitVersionTool.exe <Path to directory> <Name of file to be written to> <Path to where file will be written> -displayGroupedTickets
"-displayChangesBetweenLastTwoBuilds"	GitVersionTool.exe <Path to directory> <Name of file to be written to> <Path to where file will be written> -displayChangesBetweenLastTwoBuilds "version number"

NOTE**		If 1 date provided for displayBetweenTwoDates then all commits on that day will be displayed. 

Example:
GitVersionTool.exe "C:\Users\john.odetokun\Desktop\Project_Folder\CoreProject\deloitte-desmat" testInfoAll "C:\Users\john.odetokun\Documents\Visual Studio 2015\Projects\GitVersionTool\Output" -displayAll
GitVersionTool.exe "C:\Users\john.odetokun\Desktop\Project_Folder\CoreProject\deloitte-desmat" testInfoDates "C:\Users\john.odetokun\Documents\Visual Studio 2015\Projects\GitVersionTool\Output" -displayBetweenTwoDates 8-12-2016 21-12-2016
GitVersionTool.exe "C:\Users\john.odetokun\Desktop\Project_Folder\CoreProject\deloitte-desmat" testInfoPhrase "C:\Users\john.odetokun\Documents\Visual Studio 2015\Projects\GitVersionTool\Output" -searchPhrase #
GitVersionTool.exe "C:\Users\john.odetokun\Desktop\Project_Folder\CoreProject\deloitte-desmat" testInfoGrouped "C:\Users\john.odetokun\Documents\Visual Studio 2015\Projects\GitVersionTool\Output" -displayGroupedTickets
GitVersionTool.exe "C:\Users\john.odetokun\Desktop\Project_Folder\CoreProject\deloitte-desmat" testInfoBuild "C:\Users\john.odetokun\Documents\Visual Studio 2015\Projects\GitVersionTool\Output" -displayChangesBetweenLastTwoBuilds 1.694
 