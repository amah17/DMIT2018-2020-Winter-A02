<Query Kind="Statements">
  <Connection>
    <ID>bdad34d4-0f70-4045-9b6c-4286dee51bb4</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>Chinook</Database>
  </Connection>
</Query>

var results = from x in Albums
				select new 
				{
					AlbumTitle = x.Title,
					Year = x.ReleaseYear,
					ArtistName = x.Artist.Name
				};

//.Dump() is a LINQ pad method ONLY. DOES NOT work in my application

results.Dump();