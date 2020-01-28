<Query Kind="Expression">
  <Connection>
    <ID>bdad34d4-0f70-4045-9b6c-4286dee51bb4</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>Chinook</Database>
  </Connection>
</Query>

from x in Albums
select x

Albums
   .Select (x => x)
   
from x in Albums
where x.AlbumId == 5
select x

from x in Albums
where x.Title.Contains("de")
select x

from x in Employees
where !x.ReportsTo.HasValue
select x

from x in Albums
where x.Title.Contains("de")
orderby x.ReleaseYear descending, x.Title
select x

from x in Albums
select new 
{
	AlbumTitle = x.Title,
	Year = x.ReleaseYear,
	ArtistName = x.Artist.Name
}
