<Query Kind="Statements">
  <Connection>
    <ID>455c9eac-3497-46b6-9888-54fba6b62429</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>Chinook</Database>
  </Connection>
</Query>

//Distinct

//List of countries in which we have customers
var results1 = (from x in Customers
				orderby x.Country
				select x.Country).Distinct();
//results1.Dump();

//boolean filters .Any) and .All()
//.Any() iterates through the entire collection to see if 
//	any of the items match the specificied condition.
//RETURNS NO DATA just true or false.
//an instance of the collection that receives a true on the condition
//	is selected for processing

Genres.OrderBy(x=>x.Name).Dump();

//Show genres that have tracks which are not any playlist.
var results2 = from x in Genres
				where x.Tracks.Any(trk=>trk.PlaylistTracks.Count() == 0)
				orderby x.Name
				select new
				{
					genre = x.Name,
					tracksingenre = x.Tracks.Count(),
					boringtracks = from y in x.Tracks
									where y.PlaylistTracks.Count() == 0
									orderby y.Name
									select y.Name					
				};				
results2.Dump();


//.All() iterates through the entire collection to see if 
//	all of the items match the specificied condition.
//RETURNS NO DATA just true or false.
//an instance of the collection that receives a true on the condition
//	is selected for processing

//Show genres that have tracks which have all their tracks appearing at 
//	least once on a playlist
var populargenres = from x in Genres
				where x.Tracks.All(trk=>trk.PlaylistTracks.Count() > 0)
				orderby x.Name
				select new
				{
					genre = x.Name,
					tracksingenre = x.Tracks.Count(),
					boringtracks = from y in x.Tracks
									where y.PlaylistTracks.Count() > 0
									orderby y.Name
									select new
									{
										song= y.Name,
										count = y.PlaylistTracks.Count()
									}
				};				
populargenres.Dump();

//Sometimes you have two collections that need to be compared.
//Usually you are looking for items that are the same 
//	OR you arelooking for items that are different.
//In either case, you are comparing one collection to a second collection.

//obtain a distinct list of all playlist tracks for Roberto Almeida
//	username is AlmeidaR
var almedia = (from x in PlaylistTracks
				where x.Playlist.UserName.Contains("Almeida")
				orderby x.Track.Name
				select new
				{
					genre = x.Track.Genre.Name,
					trackid = x.TrackId,
					song = x.Track.Name
				}).Distinct().Dump();
				

//obtain a distinct list of all playlist tracks for Michelle Brooks
//	username is BrooksM
var brooks= (from x in PlaylistTracks
				where x.Playlist.UserName.Contains("Brooks")
				orderby x.Track.Name
				select new
				{
					genre = x.Track.Genre.Name,
					trackid = x.TrackId,
					song = x.Track.Name
				}).Distinct().Dump();
				
//when you are comparing two collections, you need to determine
//	which collection will be the ListA and which will be ListB

//create a list of tracks that both Roberto and Michelle have on their playlist.
var likes = almedia
		.Where(a => brooks.Any(b => a.trackid == b.trackid))
		.OrderBy(a=> a.genre)
		.Select(a => a)
		.Dump();
		
//Create a list of tracks that Roberto has but michelle doesn't
var robertoonly = almedia
		.Where(a => !brooks.Any(b => a.trackid == b.trackid))
		.OrderBy(a=> a.genre)
		.Select(a => a)
		.Dump();
		
//Create a list of tracks that Michelle has but roberto doesn't
var michelleonlyAny = brooks
		.Where(a => !almedia.Any(b => a.trackid == b.trackid))
		.OrderBy(a=> a.genre)
		.Select(a => a)
		.Dump();

//using .All(0
//note where the ! is placed
var michelleonlyAll = brooks
		.Where(a => almedia.All(b => a.trackid != b.trackid))
		.OrderBy(a=> a.genre)
		.Select(a => a)
		.Dump();
		
		
		
//To concatonate two or more results from multiple queries
//	you can use the .Union()
//This operates in the same fashion as the sql union operator.
//The rules are quite similar between the two "union"
//	number of columns same
//	column datatype same
//	ordering done on the last query

//Create a list of albums showing their title, total track count,
//	total price of tracks, and the average length of the tracks in seconds.

//query1 will report albums that have tracks
//query2 will report albums without tracks(There are no tracks for Sum() or Average())

//syntax (query1).Union(query2).Union(queryn).OrderBy(first field)ThenBy(nth field)

//Count is an integer
//Sum() is a decimal (UnitPrice is a decimal)
//Average() is returned as a double. (even though Milliseconds is an integer)
var unionresults = (from x in Albums
					where x.Tracks.Count() > 0
					select new 
					{
						title = x.Title,
						trackcount = x.Tracks.Count(),
						trackcost = x.Tracks.Sum(y=>y.UnitPrice),
						avglengthA = x.Tracks.Average(y => y.Milliseconds)/1000.00,
						avglengthB = x.Tracks.Average(y => y.Milliseconds/1000.00)
					}).Union(from x in Albums
						where x.Tracks.Count() == 0
						select new 
						{
							title = x.Title,
							trackcount = 0,
							trackcost = 0.00m,
							avglengthA = 0.0,
							avglengthB = 0.0
						})
		.OrderBy(y=>y.trackcount)
		.ThenBy(y=>y.title)
		.Dump();