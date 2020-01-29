<Query Kind="Expression">
  <Connection>
    <ID>455c9eac-3497-46b6-9888-54fba6b62429</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>Chinook</Database>
  </Connection>
</Query>

//query syntax
//from placeholder in datasource
//[where....]
//[orderby ...]
//[group...]
//select....

//select all albums
from x in Albums
select x


//method syntax is written as a composite object expression
//datasource.[Where(placeholder => expression)]
//				.[OrderBy(placeholder => expression)]
//				.[OrderByDescending(placeholder => expression)]
//				.[ThenBy(placeholder => expression)]
//				.[ThenByDescending(placeholder => expression)]
//				.[Group(...)]
//				select(placeholder => expression

//select all albums
Albums.Select(x => x)



//Navigational Properties an be used in both query and method.
//List all records from Albums ordered by descending Year
//	ascending Title release between 2007 and 2010 inclusive

//Query
from x in Albums
where x.ReleaseYear >= 2007 && x.ReleaseYear <= 2010
orderby x.ReleaseYear descending, x.Title 
select x.Title

//Method
Albums.Where(x => x.ReleaseYear >= 2007 && x.ReleaseYear <= 2010)
	.OrderByDescending(x => x.ReleaseYear)
	.ThenBy(x => x.Title)
	.Select(x => x)
		
//The orderby and where clauses can be entered in any order.

//List all USA customers with an @yahoo email address
//Order by Last Name, then first name
//Show the Customer Full name, email address, and phone number


from x in Customers
where x.Country.Equals("USA") && x.Email.Contains("@yahoo")
orderby x.LastName, x.FirstName
select new
{
	FullName = x.LastName + ", " + x.FirstName,
	Email = x.Email,
	Phone = x.Phone
}

Customers.Where(x => x.Country.Equals("USA") && x.Email.Contains("@yahoo"))
	.OrderBy(x => x.LastName)
	.ThenBy(x => x.FirstName)
	.Select(x=>new
{
	FullName = x.LastName + ", " + x.FirstName,
	Email = x.Email,
	Phone = x.Phone
})


//Create an alphabetic list of Albums by decades
//Early (pre 1970), 70s, 80s, 90s, modern( in and above 2000)
//List title, artist name, decade, year, and decade

from x in Albums
orderby x.Title
select new
{
	Title = x.Title,
	Name = x.Artist.Name,
	Year = x.ReleaseYear,
	Decade = (x.ReleaseYear <=1969 ? "Early" : 
	(x.ReleaseYear < 1980 && x.ReleaseYear >= 1970 ? "70s" : 
	(x.ReleaseYear < 1990 && x.ReleaseYear >= 1980 ? "80s" : 
	(x.ReleaseYear < 2000 && x.ReleaseYear >= 1990 ? "90s" : "Modern"))))
}

Albums.OrderBy(x => x.Title)
	.Select(x=> new
{
	Title = x.Title,
	Name = x.Artist.Name,
	Year = x.ReleaseYear,
	Decade = (x.ReleaseYear <=1969 ? "Early" : 
	(x.ReleaseYear < 1980 && x.ReleaseYear >= 1970 ? "70s" : 
	(x.ReleaseYear < 1990 && x.ReleaseYear >= 1980 ? "80s" : 
	(x.ReleaseYear < 2000 && x.ReleaseYear >= 1990 ? "90s" : "Modern"))))
})