<Query Kind="Program">
  <Connection>
    <ID>ca15a51e-e65a-4589-a7b0-9452a4f29456</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>Chinook</Database>
    <NoPluralization>true</NoPluralization>
  </Connection>
</Query>

void Main()
{
	//Nested Queries
	
	//A query within a query
	
	//the query is returned as an IEnumerable<T> or IQueryable<T>
	//if you need to return your query as a List<T> then you must
	//	encapsulate your query and add a .ToList() on the query
	//	(from...).ToList()
	
	//.ToList is also useful if you require your data in memory
	//	for some execution
	
	//Create a list of albums showing their title and artist
	//Show only albums with 25 or more tracks
	//Show the songs on the album (Name, and length)
	//Use strongly datatype elements for all data
	
	//		Artist			Album			Track
	//		 .Name			.Title			List<T> T:(name, length)
	
	var results = from x in Albums
			where x.Tracks.Count() >= 25
			orderby x.Title
			select new MyPlaylist
			{
				AlbumTitle = x.Title,
				ArtistName = x.Artist.Name,
				Songs = (from trk in x.Tracks
						select new Song
						{
							SongName = trk.Name,
							SongLength = trk.Milliseconds
						}).ToList()//Need the name and length of each track
			};		//Take note, this is ONE c# line. Thus the semi colon is at the end.
			
	results.Dump();	
	
	//Create a list of Playlist with more than 15 tracks.
	//Show the playlist name, count of tracks, total play time for playlist
	//	and the list of tracks on the playlist.
	//For each track, show the song name and Genre
	//Order the track list by Genre
	//Use strong datatypes for all data
	
	var plresults = from pl in Playlists
						where pl.PlaylistTracks.Count() > 15
						select new ThePlaylist
						{
							PLName = pl.Name,
							PLTRKCount = pl.PlaylistTracks.Count(),
							PLPlayTime = pl.PlaylistTracks.Sum(pltrk => pltrk.Track.Milliseconds),
							PlaylistSongs = from strk in pl.PlaylistTracks
											orderby strk.Track.Genre.Name
											select new PlaylistSong 
											{
												SongName = strk.Track.Name,
												SongGenre = strk.Track.Genre.Name
											}									
						};
	plresults.Dump();
}

// Define other methods and classes here

//This is a POCO. Its a flat data collection. No lists, arrays, structs, ect. There is no structures.
public class Song
{
	public string SongName {get;set;}
	public int SongLength{get; set;}	
}

public class PlaylistSong 
{
	public string SongName {get;set;}
	public string SongGenre{get;set;}
}

//DTO: Everything of a POCO PLUS a structure. 
public class MyPlaylist
{
	public string AlbumTitle {get; set;}
	public string ArtistName {get; set;}
	public List<Song> Songs {get;set;}
	
}
public class ThePlaylist
{
	public string PLName{get; set;}
	public int PLTRKCount{get;set;}
	public int PLPlayTime{get;set;}
	public IEnumerable <PlaylistSong> PlaylistSongs{get;set;}
}