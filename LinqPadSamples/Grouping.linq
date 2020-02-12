<Query Kind="Expression">
  <Connection>
    <ID>455c9eac-3497-46b6-9888-54fba6b62429</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>Chinook</Database>
  </Connection>
</Query>

//Grouping

//Basically, is the technique of placing a large pile of data
//	into smaller piles of data depending on a criteria

//Navigational properties allow for natural grouping of paren tchild (pkey/fkey)
// collections
//ex tablerowinstance.childnavproperty.Count() counts all the child records
//	associated with the parent instance

//Problem: what if there is no navigational property for the grouping of the
//	data collection

//Here you can use the group clause to create a set of smaller collections
//	based on the desired criteria

//It is IMPORTANT to remember, that once the smaller groups are created, ALL
//	reporting MUST use the smaller groups as the collection reference.

//Report albums by ReleaseYear

from x in Albums
group x by x.ReleaseYear into gYear
select gYear

//Parts of a Group
//Key Component
//Instance collection component

//side note: HotKey for commenting
//	ctrl + k + C
//	ctrl + k + U

//The Key component is created by the "by" criteria
//The "by" criteria can be 
//	a) a single attribute/property
//	b) a class
//	c) a list of attributes/properties

//Where and Orderby clauses can be executed against the 
//	group key component or group field
//You can filter(where) or order before grouping
//However, Orderby before grouping is basically useless.

//Report albums by ReleaseYear showing the year and
//	the number of Albums for that year. Order by year
//	with the most albums, then by the year within count.

from x in Albums
group x by x.ReleaseYear into gYear
orderby gYear.Count() descending, gYear.Key
select new
{
	year = gYear.Key,
	albumcount = gYear.Count()
}

//Report albums by ReleaseYear showing the year and
//	the number of Albums for that year. Order by year
//	with the most albums, then by the year within count.
//Report the album title, artist name and number of album tracks.
//Report only albums of the 90s

from x in Albums
where x.ReleaseYear > 1989 && x.ReleaseYear < 2000
group x by x.ReleaseYear into gYear
orderby gYear.Count() descending, gYear.Key
select new
{
	year = gYear.Key,
	albumcount = gYear.Count(),
	albumandartist = from gr in gYear
						select new
						{
							title = gr.Title,
							artist = gr.Artist.Name,
							trackcount = gr.Tracks.Count(trk => trk.AlbumId == gr.AlbumId)	//When grouping, Count ALSO needs a delegate
						}
}

//Grouping can be done on entity attributes determined  using a
//	navigational property.
//List tracks for albums produced after 2010 by Genre name.
//	Count tracks for the Name

from trk in Tracks
where trk.Album.ReleaseYear > 2010
group trk by trk.Genre.Name into gTemp
orderby gTemp.Key
select new
{
	genreName = gTemp.Key,
	genretrackcount = gTemp.Count()
}


//same report, but using the entity as the group criteria.
//when you have multiple attributes in a group key
//	you MUST reference the attribute using Key.Attribute

from trk in Tracks
where trk.Album.ReleaseYear > 2010
group trk by trk.Genre into gTemp
//select gTemp
orderby gTemp.Key.Name
select new
{
	genreName = gTemp.Key.Name,
	genretrackcount = gTemp.Count()
}

//Using Group Techs
//Create a list of customers
//	BY employee support indvidual showing the employee 
//	lastname, firstname (phone), the number of customers for this employee
//	and a customer list for the employee. In this customer list, show the state, city, and 
//	customer name (last, first).
//	Order the customer list by state, then city

//Decision 1, where to start: Group the customers.
//	Why? Easy to reach up to parent info using Nav Properties.

//group on what: Group customers onto a specific employee
//				:Also report info on the employee.
//Would grouping onto the employee entity supply the employee info in the KEY
//Decision 2, group the customers by SupportRepIdEmployee.
from c in Customers
group c by c.SupportRepIdEmployee into gCust
//select gCust
select new
{
	empnamephone = gCust.Key.LastName + ", " + gCust.Key.FirstName + " (" + gCust.Key.Phone + ")",
	customercount = gCust.Count(),
	clist= from gc in gCust
				orderby gc.State, gc.City
				select new 
				{
					state = gc.State,
					city = gc.City,
					name = gc.LastName + ", " + gc.FirstName
				}			
}


//Grouping on multiple attributes not in a class/entity
//List of Customers
//Grouped by Country and state
from c in Customers
group c by new {c.Country, c.State} into gResidence
orderby gResidence.Count() descending
select new
{
	country = gResidence.Key.Country,
	state = gResidence.Key.State,
	nofcustomers = gResidence.Count()
}