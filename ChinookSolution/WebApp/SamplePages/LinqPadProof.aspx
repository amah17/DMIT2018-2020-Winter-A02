<%@ Page Title="LinqPadProof" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LinqPadProof.aspx.cs" Inherits="WebApp.SamplePages.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Prooof of Linq Pad</h1>

    <blockquote class="alert alert-info">This page uses a query developed in LinqPad using the Program editor window.
        The POCO class was created and tested in LinqPad. The was created and tested in LinqPad.
        The tested code was then placed within the application with one change to the query: the context. was placed in front of the data collection expression.
    </blockquote>
    <asp:GridView ID="LinqPadQueryList" runat="server" AutoGenerateColumns="False" DataSourceID="LinqPadQueryListODS" AllowPaging="True">
        <Columns>
            <asp:TemplateField HeaderText="AlbumTitle" SortExpression="AlbumTitle">
                <EditItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("AlbumTitle") %>' ID="TextBox1"></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("AlbumTitle") %>' ID="Label1"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="Year" HeaderText="Year" SortExpression="Year"></asp:BoundField>
            <asp:BoundField DataField="ArtistName" HeaderText="ArtistName" SortExpression="ArtistName"></asp:BoundField>
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="LinqPadQueryListODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="AlbumArtists" TypeName="ChinookSystem.BLL.AlbumController"></asp:ObjectDataSource>

</asp:Content>
