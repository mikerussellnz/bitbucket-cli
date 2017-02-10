# bitbucket-cli
Command line client to connect to Atlassian Bitbucket and perform actions.  Allows scripting of Bitbucket operations by shell scripts or for people who just perfer to use the command line. 

## Todo

* add short command line options where appropriate.
* finish commit actions.
* add inbox list action. 
* mask password input when configuring credentials.
* error handling - exit code on failure, pretty error messages. 
* add verbose mode.
* add more actions and modules.

Pull requests welcome.  

## Building

This project depends on the Atlassian.Stash.Api Nuget package.  Ensure the package is restored before building.

## Usage Examples

#### Create pull requset
BitbucketCLI pullrequest create --repository=PROJ/repo --from=feature/make_it_cool --to=develop --title="Added cool new features." --description="Added feature X and feature Y." --reviewer=james@tester.com  

#### List pull requsets
BitbucketCLI pullrequest list --repository=PROJ/repo --repository=PROJ/repo2  
BitbucketCLI pullrequest list --repository=PROJ/repo --user=mike@tester.com --state=merged  

#### Create branch
BitbucketCLI branch create --repository=PROJ/repo --from=ddb3629bfe5f7f9ae76c314a3da1b255a8f45dd3 --name=feature/make_it_cool 

#### List branches
BitbucketCLI branch list --repository=PROJ/repo  
BitbucketCLI branch list --repository=PROJ/repo --filter=feature/  

#### List projects
BitbucketCLI project list  

#### List repositories
BitbucketCLI repository list --project=PROJ  

#### List files in repository
BitbucketCLI repository listfiles --repository=PROJ/repo  

#### Get file contents
BitbucketCLI repository getfile --repository=PROJ/repo --file=path/to/file
