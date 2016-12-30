Desmat Git tool

This project enables one to access a git repository and extract valuable information regarding Commits, dates, tickets and versioning.

By default GitVersionTool.exe is located in GitVersionTool\bin\Debug.

Application writes three files (A .txt, .csv, .htm)
All these files contain information about Author, Committer, Message, Date and Time
The .htm file has an added link to the assembla ticket

For different subdivisions of repository the following can be used:
Choices for OPTION are:

	+	"-All"										++ All commits of given repository
	+	"-BetweenTwoDates"							++ All commits of given repository that are between two DateTimes
	+	"-MessagePhraseSearch"						++ All commits of given repository whos commit messages contain given phrase
	+	"-GroupByTickets"							++ All commits of given repository that correspond to tickets (Grouped by tickets) 
	+	"-BetweenLastTwoBuilds":					++ All changes between two most recent build

To run in each commandline the following is required:

In general: GitVersionTool.exe <Path to directory> <Name of file to be written to> <Path to where file will be written> <OPTION> [Parameters]

"-All":							GitVersionTool.exe <Path to directory> <Name of file to be written to> <Path to where file will be written> -All
"-BetweenTwoDates"				GitVersionTool.exe <Path to directory> <Name of file to be written to> <Path to where file will be written> -BetweenTwoDates "dd-mm-yyyy" "dd-mm-yyyy"
"-MessagePhraseSearch"			GitVersionTool.exe <Path to directory> <Name of file to be written to> <Path to where file will be written> -MessagePhraseSearch "Any phrase to be searched"
"-GroupByTickets"				GitVersionTool.exe <Path to directory> <Name of file to be written to> <Path to where file will be written> -GroupByTickets
"-BetweenLastTwoBuilds"			GitVersionTool.exe <Path to directory> <Name of file to be written to> <Path to where file will be written> -BetweenLastTwoBuilds "version number" 

Example:
GitVersionTool.exe "C:\Users\john.odetokun\Desktop\Project_Folder\CoreProject\deloitte-desmat" testInfoAll "C:\Users\john.odetokun\Documents\Visual Studio 2015\Projects\GitVersionTool\Output" -All
GitVersionTool.exe "C:\Users\john.odetokun\Desktop\Project_Folder\CoreProject\deloitte-desmat" testInfoDates "C:\Users\john.odetokun\Documents\Visual Studio 2015\Projects\GitVersionTool\Output" -BetweenTwoDates 8-12-2016 21-12-2016
GitVersionTool.exe "C:\Users\john.odetokun\Desktop\Project_Folder\CoreProject\deloitte-desmat" testInfoPhrase "C:\Users\john.odetokun\Documents\Visual Studio 2015\Projects\GitVersionTool\Output" -MessagePhraseSearch #
GitVersionTool.exe "C:\Users\john.odetokun\Desktop\Project_Folder\CoreProject\deloitte-desmat" testInfoGrouped "C:\Users\john.odetokun\Documents\Visual Studio 2015\Projects\GitVersionTool\Output" -GroupByTickets
GitVersionTool.exe "C:\Users\john.odetokun\Desktop\Project_Folder\CoreProject\deloitte-desmat" testInfoBuild "C:\Users\john.odetokun\Documents\Visual Studio 2015\Projects\GitVersionTool\Output" -BetweenLastTwoBuilds 1.772

=========================================================================================================================================================================================================================================

This project Project can be expanded to Extract different Commit info or write files of different formats
