using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ChinookSystem.Data.Entities;
using ChinookSystem.Data.DTOs;
using ChinookSystem.Data.POCOs;
using ChinookSystem.DAL;
using System.ComponentModel;
using DMIT2018Common.UserControls;
#endregion

namespace ChinookSystem.BLL
{
    public class PlaylistTracksController
    {
        public List<UserPlaylistTrack> List_TracksForPlaylist(
            string playlistname, string username)
        {
            using (var context = new ChinookContext())
            {

                List<UserPlaylistTrack> results = (from x in context.PlaylistTracks
                                                   where x.Playlist.Name.Equals(playlistname)
                                                      && x.Playlist.UserName.Equals(username)
                                                   orderby x.TrackNumber
                                                   select new UserPlaylistTrack
                                                   {
                                                       TrackID = x.TrackId,
                                                       TrackNumber = x.TrackNumber,
                                                       TrackName = x.Track.Name,
                                                       Milliseconds = x.Track.Milliseconds,
                                                       UnitPrice = x.Track.UnitPrice
                                                   }).ToList();
                return results;
            }
        }//eom
        public void Add_TrackToPLaylist(string playlistname, string username, int trackid)
        {
            using (var context = new ChinookContext())
            {
                //Transaction(TRX)
                //Query the Playlist table to see if the playlist name exists for the user
                //If not
                //      Create instance of Playlist
                //      Load
                //      Add
                //      Set trackNumber to 1
                //if yes
                //      Query Playlist track for trackID
                //      If found
                //              Throw an error
                //      If not found
                //              Query Playlist tracks for max tracknumber, then increment++
                //              Create an instance of PlayListTrack
                //              Load
                //              Add
                //              Save Changes
                List<string> errors = new List<string>();
                int tracknumber = 0;
                PlaylistTrack newtrack = null;
                Playlist exists = (from x in context.Playlists
                                  where x.Name.Equals(playlistname)
                                  && x.UserName.Equals(username)
                                  select x).FirstOrDefault();
                
                if (exists == null)
                {
                    //new playlist
                    exists = new Playlist();
                    exists.Name = playlistname;
                    exists.UserName = username;
                    context.Playlists.Add(exists);
                    tracknumber = 1;
                }
                else
                {
                    //existing playlist
                    //This basically will make newtrack be null if there is NO instance of that track id in the playlist.
                    newtrack = (from x in context.PlaylistTracks
                                where x.Playlist.Name.Equals(playlistname)
                                && x.Playlist.UserName.Equals(username)
                                && x.TrackId == trackid
                                select x).FirstOrDefault();

                    if(newtrack == null)
                    {
                        //Adding the new track to the playlist.
                        //This selects the highest tracknumber
                        tracknumber = (from x in context.PlaylistTracks
                                       where x.Playlist.Name.Equals(playlistname)
                                        && x.Playlist.UserName.Equals(username)
                                       select x.TrackNumber).Max();
                        tracknumber++;
                    }
                    else
                    {
                        //Track on playlist.
                        //Business rule states no duplicates

                        //Throw an exception
                        //throw new Exception("Track already on the playlist. Duplicates not allowed.");

                        //throw a business rule exception.
                        //collect the errors into a List<String>
                        //After all validation is done, test the collection (List<t>) for
                        //  having any messages, if so, throw new BusinessRuleException()
                        errors.Add("Track already on the playlist. Duplicates not allowed.");
                    }

                }

                //All validation of Playlist and Playstlisttrack is complete
                if (errors.Count() > 0)
                {
                    throw new BusinessRuleException("Adding a Track", errors);
                }
                else
                {
                    //create & load & add a PlaylistTrack
                    newtrack = new PlaylistTrack();
                    newtrack.TrackId = trackid;
                    newtrack.TrackNumber = tracknumber;
                    exists.PlaylistTracks.Add(newtrack);   //stages ONLY, USE THE PARENT

                    context.SaveChanges();  //physical addition
                }
            }
        }//eom
        public void MoveTrack(string username, string playlistname, int trackid, string direction)
        {
            using (var context = new ChinookContext())
            {
                //Business rules need to be executed within our BLL
                var exists = (from x in context.Playlists
                              where x.UserName.Equals(username)
                              && x.Name.Equals(playlistname)
                              select x).FirstOrDefault();
                
                if (exists == null)
                {
                    throw new Exception("Play list has been removed from the system.");
                }
                else
                {
                    //is the track still there
                    var moveTrack = (from x in exists.PlaylistTracks
                                     where x.TrackId == trackid
                                     select x).FirstOrDefault();
                    if(moveTrack == null)
                    {
                        throw new Exception("Track has been removed from the system.");
                    }
                    else
                    {
                        PlaylistTrack otherTrack = null;
                        //try moving
                        //check movement is still possible
                        //determine direction
                        if (direction.Equals("up"))
                        {
                            //up
                            if(moveTrack.TrackNumber == 1)
                            {
                                throw new Exception("Song is at the top of the play list. Cannot move further up");
                            }
                            else
                            {
                                //prep for move
                                otherTrack = (from x in exists.PlaylistTracks
                                                  where x.TrackNumber == moveTrack.TrackNumber - 1
                                                  select x).FirstOrDefault();
                                if(otherTrack == null)
                                {
                                    throw new Exception("Missing required other song track record.");
                                }
                                else
                                {
                                    moveTrack.TrackNumber -= 1;
                                    otherTrack.TrackNumber += 1;
                                }
                            }
                        }
                        else
                        {
                            //down
                            if (moveTrack.TrackNumber == exists.PlaylistTracks.Count)
                            {
                                throw new Exception("Song is at the bottom of the play list. Cannot move further down");
                            }
                            else
                            {
                                //prep for move
                                otherTrack = (from x in exists.PlaylistTracks
                                                  where x.TrackNumber == moveTrack.TrackNumber + 1
                                                  select x).FirstOrDefault();
                                if (otherTrack == null)
                                {
                                    throw new Exception("Missing required other song track record.");
                                }
                                else
                                {
                                    moveTrack.TrackNumber += 1;
                                    otherTrack.TrackNumber -= 1;
                                }
                            }
                        }
                        //alter the database
                        //this is NOT a CRUD update where multiple fields on a record
                        //  could be altered and you do not know which fields to alter
                        //The only field being altered is Tracknumber
                        //Transaction
                        context.Entry(moveTrack).Property("TrackNumber").IsModified = true;
                        context.Entry(otherTrack).Property("TrackNumber").IsModified = true;
                    }

                }
            }
        }//eom


        public void DeleteTracks(string username, string playlistname, List<int> trackstodelete)
        {
            using (var context = new ChinookContext())
            {                
                var exists = (from x in context.Playlists
                              where x.UserName.Equals(username)
                              && x.Name.Equals(playlistname)
                              select x).FirstOrDefault();
                if (exists == null)
                {
                    throw new Exception("Play list has been removed from the system.");
                }
                else
                {
                    //find the songs to keep
                    var trackstkept = exists.PlaylistTracks
                                            .Where(tr => !trackstodelete.Any(tod => tod == tr.TrackId))
                                            .OrderBy(tr => tr.TrackNumber)
                                            .Select(tr => tr);

                    //Remove tracks to delete.
                    PlaylistTrack item = null;
                    //make a list of tracks to delete
                    foreach(var dtrackid in trackstodelete)
                    {
                        item = exists.PlaylistTracks
                                        .Where(tr => tr.TrackId == dtrackid)
                                        .Select(tr => tr)
                                        .FirstOrDefault();
                        if (item != null)
                        {
                            exists.PlaylistTracks.Remove(item);
                        }
                    }

                    //Resequence the kept tracks
                    int number = 1;
                    foreach(var tKept in trackstkept)
                    {
                        tKept.TrackNumber = number;
                        context.Entry(tKept).Property("TrackNumber").IsModified = true;
                        number++;
                    }
                    context.SaveChanges();
                }
            }
        }//eom
    }
}
