using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additonal Namespaces
using ChinookSystem.BLL;
using ChinookSystem.Data.POCOs;
#endregion

namespace WebApp.SamplePages
{
    public partial class ManagePlaylist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TracksSelectionList.DataSource = null;
        }

        protected void CheckForException(object sender, ObjectDataSourceStatusEventArgs e)
        {
            MessageUserControl.HandleDataBoundException(e);
        }

        protected void ArtistFetch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ArtistName.Text))
            {
                //message to the user
                MessageUserControl.ShowInfo("Entry Error", "Select an artist name or part of.");
            }
            else
            {
                TracksBy.Text = "Artist";
                SearchArg.Text = ArtistName.Text;
                TracksSelectionList.DataBind();
            }
        }

        protected void MediaTypeDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MediaTypeDDL.SelectedIndex == 0)
            {
                //message to the user
                MessageUserControl.ShowInfo("Selection Error", "Enter a media type");
            }
            else
            {
                TracksBy.Text = "MediaType";
                SearchArg.Text = MediaTypeDDL.SelectedValue;
                TracksSelectionList.DataBind();
            }
        }

        protected void GenreFetch_Click(object sender, EventArgs e)
        {
            TracksBy.Text = "Genre";
            SearchArg.Text = GenreDDL.SelectedValue;
            TracksSelectionList.DataBind();
            
        }

        protected void AlbumFetch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(AlbumTitle.Text))
            {
                //message to the user
                MessageUserControl.ShowInfo("Entry Error", "Enter an album title or part of.");
            }
            else
            {
                TracksBy.Text = "Album";
                SearchArg.Text = AlbumTitle.Text;
                TracksSelectionList.DataBind();
            }
        }

        protected void PlayListFetch_Click(object sender, EventArgs e)
        {
            //Security is yet to be implemented. 
            //This page needs to know the username of the currently logged in user. 
            //Temp, we will hardcode the username
            string username = "HansenB";
            if (string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Entry Error", "Enter a playlist name.");
            }
            else
            {
                //Your code does NOT need to have a try/catch
                //The try/catch is embedded within MessageUserControl
                //The syntax for executing with MessageUserControl
                //  MessageUserControl.Tryrun(() => { coding block }, "Success Title", "Success Message");
                MessageUserControl.TryRun(() =>
                {
                    PlaylistTracksController sysmgr = new PlaylistTracksController();
                    List<UserPlaylistTrack> info = sysmgr.List_TracksForPlaylist(PlaylistName.Text, username);
                    PlayList.DataSource = info;
                    PlayList.DataBind();
                }, "Playlist", "View Current Tracks on Playlist");
            }
 
        }

        protected void MoveDown_Click(object sender, EventArgs e)
        {
            if (PlayList.Rows.Count == 0)
            {
                MessageUserControl.ShowInfo("Track Movement", "You must have a playlist showing. Fetch a playlist.");
            }
            else
            {
                if (string.IsNullOrEmpty(PlaylistName.Text))
                {
                    MessageUserControl.ShowInfo("Track Movement", "You must have a playlist name.");
                }
                else
                {
                    //Determine if a single song on the playlist has been selected.
                    //collect the trackid, tracknumber
                    int trackid = 0;
                    int tracknumber = 0;
                    int rowsselected = 0;
                    CheckBox songSelectetd = null;  //A reference pointer to a control

                    //traverse the song list
                    //testing to see if its selected
                    //Logic will change if there is paging. I would have to query to find the total length of the list.
                    //These lists do not have paging
                    for (int rowindex = 0; rowindex < PlayList.Rows.Count; rowindex++)
                    {
                        //point to a checkbox on the gridview row
                        songSelectetd = PlayList.Rows[rowindex].FindControl("Selected") as CheckBox;
                        //Selected?
                        if (songSelectetd.Checked)
                        {
                            trackid = int.Parse((PlayList.Rows[rowindex].FindControl("TrackID") as Label).Text);
                            tracknumber = int.Parse((PlayList.Rows[rowindex].FindControl("TrackNumber") as Label).Text);
                            rowsselected++;
                        }
                    }

                    if (rowsselected != 1)
                    {
                        MessageUserControl.ShowInfo("Track Movement", "You must select a single song to move.");
                    }
                    else
                    {
                        //Check to see if the selected song is NOT at the bottom of the list
                        if(tracknumber == PlayList.Rows.Count)
                        {
                            MessageUserControl.ShowInfo("Track Movement", "Song is at the bottom of the play list. Cannot move further down.");
                        }
                        else
                        {
                            MoveTrack(trackid, tracknumber, "down");
                        }
                    }
                }
            }
 
        } //EOM

        protected void MoveUp_Click(object sender, EventArgs e)
        {
            if (PlayList.Rows.Count == 0)
            {
                MessageUserControl.ShowInfo("Track Movement", "You must have a playlist showing. Fetch a playlist.");
            }
            else
            {
                if (string.IsNullOrEmpty(PlaylistName.Text))
                {
                    MessageUserControl.ShowInfo("Track Movement", "You must have a playlist name.");
                }
                else
                {
                    //Determine if a single song on the playlist has been selected.
                    //collect the trackid, tracknumber
                    int trackid = 0;
                    int tracknumber = 0;
                    int rowsselected = 0;
                    CheckBox songSelectetd = null;  //A reference pointer to a control

                    //traverse the song list
                    //testing to see if its selected
                    //Logic will change if there is paging. I would have to query to find the total length of the list.
                    //These lists do not have paging
                    for (int rowindex = 0; rowindex < PlayList.Rows.Count; rowindex++)
                    {
                        //point to a checkbox on the gridview row
                        songSelectetd = PlayList.Rows[rowindex].FindControl("Selected") as CheckBox;
                        //Selected?
                        if (songSelectetd.Checked)
                        {
                            trackid = int.Parse((PlayList.Rows[rowindex].FindControl("TrackID") as Label).Text);
                            tracknumber = int.Parse((PlayList.Rows[rowindex].FindControl("TrackNumber") as Label).Text);
                            rowsselected++;
                        }
                    }

                    if (rowsselected != 1)
                    {
                        MessageUserControl.ShowInfo("Track Movement", "You must select a single song to move.");
                    }
                    else
                    {
                        //Check to see if the selected song is NOT at the Top of the list
                        if (tracknumber == 1)
                        {
                            MessageUserControl.ShowInfo("Track Movement", "Song is at the top of the play list. Cannot move further up.");
                        }
                        else
                        {
                            MoveTrack(trackid, tracknumber, "up");
                        }
                    }
                }
            }

        }

        protected void MoveTrack(int trackid, int tracknumber, string direction)
        {
            string username = "HansenB";
            //call BLL to move track
            MessageUserControl.TryRun(() =>
            {
                PlaylistTracksController sysmgr = new PlaylistTracksController();
                sysmgr.MoveTrack(username, PlaylistName.Text, trackid, direction);
                List<UserPlaylistTrack> info = sysmgr.List_TracksForPlaylist(PlaylistName.Text, username);
                PlayList.DataSource = info;
                PlayList.DataBind();
                //optionally flag the song that was moved
                //  traverse the GridView again looking for the trackid
            }, "Track Movement", "Track has been moved.");
 
        }


        protected void DeleteTrack_Click(object sender, EventArgs e)
        {
            //code to go here
 
        }

        protected void TracksSelectionList_ItemCommand(object sender, 
            ListViewCommandEventArgs e)
        {
            string username = "HansenB";
            if (string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Entry Error", "Enter a playlist name.");
            }
            else
            {
                MessageUserControl.TryRun(() =>
                {
                    PlaylistTracksController sysmgr = new PlaylistTracksController();
                    sysmgr.Add_TrackToPLaylist(PlaylistName.Text, username, int.Parse(e.CommandArgument.ToString()));
                    List<UserPlaylistTrack> info = sysmgr.List_TracksForPlaylist(PlaylistName.Text, username);
                    PlayList.DataSource = info;
                    PlayList.DataBind();
                }, "Add Track to Playlist", "Track has been added to the Playlist");
            }

        }


    }
}