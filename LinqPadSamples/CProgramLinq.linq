<Query Kind="Program">
  <Connection>
    <ID>bdad34d4-0f70-4045-9b6c-4286dee51bb4</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>Chinook</Database>
  </Connection>
</Query>

void Main()
{
	var results = from x in Albums
					select new AlbumArtists
					{
						AlbumTitle = x.Title,
						Year = x.ReleaseYear,
						ArtistName = x.Artist.Name
					};
	
	//.Dump() is a LINQ pad method ONLY. DOES NOT work in my application
	
	results.Dump();
}

// Define other methods and classes here
public class AlbumArtists
{
	public string AlbumTitle{get;set;}
	public int Year {get;set;}
	public string ArtistName {get;set;}
}
