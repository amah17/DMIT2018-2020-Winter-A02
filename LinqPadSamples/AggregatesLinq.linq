<Query Kind="Expression">
  <Connection>
    <ID>455c9eac-3497-46b6-9888-54fba6b62429</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>Chinook</Database>
  </Connection>
</Query>

//Aggregates

//Aggregates work against collections (0, 1, or more records in a dataset)
//.Count(0) , .Sum() , .Min() , .Max() , .Average()
//Aggregates can be used in both Query syntax anx method syntax

//list all albus showing the ablum title, album artist name
//	and the number of tracks on the album
//	Artists -> Albums -> Tracks
//	   ICollection	   ICollection	(parent -> Child navigational Property)
//			Artist			Album	(child -> parent navigational Property)

//method syntax used for the aggregate 
//Query syntax for the overall query
from x in Albums
select new
{
	title = x.Title,
	artist = x.Artist.Name,
	trackcount = x.Tracks.Count()
}

//complete method syntax
Albums
	.Select (x => new 
				{
					title = x.Title,
					artist = x.Artist.Name,
					trackcount = x.Tracks.Count()
				})
				
				
//Query Syntax
from x in Albums
select new
{
	title = x.Title,
	artist = x.Artist.Name,
	trackcount = (from y in x.Tracks
					select y).Count()
}

from x in Albums
select new
{
	title = x.Title,
	artist = x.Artist.Name,
	trackcount = (from y in Tracks
					where y.AlbumId == x.AlbumId
					select y).Count()
}


//ToDo
//List the artists and their number of albums
//order the list from  most albums to least albums
//if the count is tied, order by artist Name

from x in Artists
orderby x.Albums.Count() descending, x.Name 
select new
{
	artist = x.Name,
	albumcount = x.Albums.Count()
}

//Find the Maximum number of albums for all artists

//Get a list of counts, find the max in that list
(from x in Artists
select x.Albums.Count()
).Max()

(Artists.Select(x=> x.Albums.Count())).Max()

//Produce a list of albums which have tracks showing their
//title, artist name, number of tracks on album and
//Total price of all tracks for album

//number of x : Count()
//Total of x : Sum()
from x in Albums
orderby x.Tracks.Sum(tr => tr.UnitPrice) descending
select new
{
	albumtitle = x.Title,
	artistname = x.Artist.Name,
	trackcount = x.Tracks.Count(),
	totalprice = x.Tracks.Sum(tr => tr.UnitPrice)
}

//method version
from x in Albums
select new
{
	albumtitle = x.Title,
	artistname = x.Artist.Name,
	trackcount = (from y in x.Tracks
					select y).Count(),
	totalprice = (from y in x.Tracks
					select y.UnitPrice).Sum()
}

//List all playlists which have a track 
//showing the playlist name, number of tracks for playlist, the cost of the playlist
//and total storage size for the playlist in megabytes


//Playlists				PlaylistTracks			Tracks
// .Name	    Based on Playlist.Count()		 .UnitPrice, .Bytes
//			ICollection					Track
//					Playlists		 Icollection
from x in Playlists
where x.PlaylistTracks.Count() > 0
select new
{
	plname = x.Name,
	pltrackcount = x.PlaylistTracks.Count(),
	plcost = x.PlaylistTracks.Sum(plt => plt.Track.UnitPrice),					//x.PlaylistTracks points to only the tracks in that singular playlist
	storage = x.PlaylistTracks.Sum(plt => plt.Track.Bytes/1000000.0)
}

