## _Summary_
A high performance API is desired to support queries against data stored in a sqlite file. 

The database file contains area/geography data and the API is intended to support a lookup UI with autocomplete functionality. 

## _Requirements_
<li> Docker: To facilitate usability across multiple platforms the code has been packaged into a docker image available at ddnev/emsi_api. Therefore, by the magic of containers Docker is the only requirement. 

## _Setup_
The following instructions assume the Docker container will be run from a linux environment. Nonetheless, these steps should be easy to translate to Windows. 
  
(1) Confirm that Docker is installed.
> $ sudo docker --version
  
(2) Initialize local container from image
> $ sudo docker run -d -p 5000:80
  
(3) Confirm that the container is running
> $ sudo docker ps

## Data Structure
The database file contains a single table with the following fields:
  <li> name (TEXT)
  <li> abbr (TEXT)
  <li> display_id (TEXT)
  <li> child (INT)
  <li> parent (INT)
  <li> aggregation_path (INT)
  <li> level (INT)
  
## Supported Query Types
The API supports queries on: 
    <li> name 
    <li> abbreviation
    <li> display_id
    
Queries return the name, abbreviation and display_id for any matching results.
      
## Examples
To get all rows where name starts with "Pull" and the display_id starts with "ZIP9916":
> http://localhost:5000/areas?name=Pull&display_id=ZIP9916
  
Yields:
>[{"name":"Pullman","abbr":"WA","display_id":"ZIP99164"},{"name":"Pullman","abbr":"WA","display_id":"ZIP99163"}]

## Solution Design
The API is implemented using the System.Data.SQLite library. This approach was selected over Entity Framework (EF) in an effort to increase performance. While EF has many good qualities, the extra overhead that comes with supporting ORM seemed undesireable for a read-only API focused performance.
      
Query results were limited to the name, abbreviation and display_id fields based on the autocomplete use case. The other data relating the areas to eachother does not appear to be of interest to the users.
      
Queries are parameterized by the same fields - name, abbreviation and display_id. Given an input like
      > http://localhost:5000/areas?name=Pull&abbr=W&display_id=ZIP9916 
The query executed against the database is of the form:
      > SELECT name, abbr, display_id FROM areas WHERE name like 'Pull%' and abbr like 'W%' and display_id like 'ZIP9916%'
Structuring the query this way should support autocomplete and may not have as poor of performance as a contains (e.g., "like '%searchterm%'") predicate. Additionally, this format supports query parameterization to prevent SQL injection. Note: In it's current form the SQL query is still generated in a vulnerable, naive manner. 
      
Basic logging is implemented, including:
      <li> The expected database file location
      <li> Query parameters
      <li> Number of results returned
      <li> Database connection failure
      <li> Query errors
        
## To Do
SQL query generation should be improved with regard to injection protection.
This solution doesn't implement any certification. 
        

    
