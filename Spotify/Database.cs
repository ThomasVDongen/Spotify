using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc.Html;
using System.Web.UI.WebControls;
using Antlr.Runtime.Misc;
using Oracle.ManagedDataAccess.Client;
using Spotify.Models;
using Spotify.Models.Objecten;

namespace Spotify
{
    public static class Database
    {
        /// <summary>
        /// login naam van mijn athena
        /// </summary>
        private const string USER = "dbi323229";
        /// <summary>
        /// The password used to connect with the database.
        /// </summary>
        private const string PASSWORD = "wXkDxOdQUV";
        /// <summary>
        /// The oracle-specific string used to connect with the database.
        /// </summary>
        private const string CONNECTION_STRING = "User Id= " + USER + ";Password= " + PASSWORD + ";Data Source=" + @" //192.168.15.50:1521/fhictora" + ";";
        /// <summary>
        /// connection aangemaakt zodat deze gebruikt kan worden.
        /// </summary>
        private static readonly OracleConnection Conn;
        /// <summary>
        /// instantie van de database klasse maakt een connectie als deze wordt aangeroepen.
        /// </summary>
        static Database()
        {
            try
            {
                Conn = new OracleConnection(CONNECTION_STRING);
            }
            catch (OracleException oEx)
            {
                throw oEx;
            }
        }
        /// <summary>
        /// Opens a connection with the database.
        /// </summary>
        /// <returns>A bool wether the connection was sucessfully opened</returns>
        public static bool OpenConnection()
        {
            Console.WriteLine(Conn.State == System.Data.ConnectionState.Open
                ? "Database Connection was already open"
                : "Opening Database Connection..");
            try
            {
                if (Conn.State != ConnectionState.Open)
                {
                    Conn.Open();
                }
                return true;
            }
            catch (OracleException oEx)
            {
                throw oEx;
            }
        }
        /// <summary>
        /// Closes the connection with the database.
        /// </summary>
        /// <returns>A bool wether the connection was successfully closed</returns>
        public static bool CloseConnection()
        {
            Console.WriteLine(Conn.State == System.Data.ConnectionState.Closed
                ? "Database Connection was already closed"
                : "Closing Database Connection..");
            try
            {
                Conn.Close();
                return true;
            }
            catch (OracleException oEx)
            {
                return false;
            }
        }
        /// <summary>
        /// methode die voor mij de open connection handelt zodat ik dat niet altijd hoef te doen.
        /// ik hoef hier alleen het oraclecommand aan mee te geven en dan returned hij de reader
        /// </summary>
        /// <param name="sqlC"></param>
        /// <returns></returns>
        private static OracleDataReader ReadData(OracleCommand sqlC)
        {
            if (!OpenConnection())
                return null;
            try
            {
                sqlC.Connection = Conn;
                return sqlC.ExecuteReader();
            }
            catch (OracleException oEx)
            {
                CloseConnection();
                return null;
            }
            catch (NullReferenceException exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Haal alle songs op die bij de user horen
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<Song> GetSongs(int musicid)
        {
            string sqlS = "SELECT * FROM NUMMER N " + "JOIN MUZIEK_NUMMER MN ON N.ID = MN.NUMMERID " +
                          "WHERE MN.MUZIEKID =" + musicid;
            OracleCommand sqlC = new OracleCommand(sqlS);
            List<Song> songs = new List<Song>();

            OracleDataReader oraReader = ReadData(sqlC);

            if (oraReader == null)
                return songs;
            while (oraReader.Read())
            {
                Song song = new Song();
                song.ID = Convert.ToInt32(oraReader["ID"]);
                song.Artists = GetArtists(song.ID);
                song.Releasedate = Convert.ToDateTime(oraReader["releasedate"]);
                song.Name = Convert.ToString(oraReader["Titel"]);
                song.Speelduur = Convert.ToDouble(oraReader["SPEELDUUR"]);
                song.Genres = GetGenres(song.ID);
                songs.Add(song);
            }

            CloseConnection();

            return songs;
        }
        /// <summary>
        /// haal alle ids en namen op van alle artiesten zodat deze makkelijk op kan halen zodra iemand op de artiesten pagina klikt.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<Artist> GetArtists(int songid)
        {
            string sqlS = "SELECT A.ID, A.NAAM FROM ARTIEST A " + "JOIN NUMMER_ARTIEST NA ON NA.ARTIESTID = A.ID " +
                          "JOIN NUMMER N ON NA.NUMMERID = N.ID " + "WHERE N.ID = " + songid;
            OracleCommand sqlC = new OracleCommand(sqlS);
            List<Artist> Artists = new List<Artist>();
            OracleDataReader oraReader = ReadData(sqlC);


            if (oraReader == null)
                return Artists;
            while (oraReader.Read())
            {
                Artist artist = new Artist();
                artist.ID = Convert.ToInt32(oraReader["ID"]);
                artist.Name = Convert.ToString(oraReader["Naam"]);
                Artists.Add(artist);

            }
            return Artists;
        }
        /// <summary>
        /// haal alle ids op van alle genres zodat deze makkelijk op kan halen zodra iemand op de nummers pagina klikt.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<Genre> GetGenres(int songid)
        {
            string sqlS = "SELECT G.ID, G.NAAM FROM GENRE G " + "JOIN NUMMER_GENRE NG ON NG.GENREID = G.ID " +
                          "JOIN NUMMER N ON NG.NUMMERID = N.ID " + "WHERE N.ID = " + songid;
            OracleCommand sqlC = new OracleCommand(sqlS);
            List<Genre> Genres = new List<Genre>();


            OracleDataReader oraReader = ReadData(sqlC);

            if (oraReader == null)
                return Genres;
            while (oraReader.Read())
            {
                Genre genre = new Genre();
                genre.ID = Convert.ToInt32(oraReader["ID"]);
                genre.Name = Convert.ToString(oraReader["Naam"]);
                Genres.Add(genre);

            }
            return Genres;
        }
        /// <summary>
        /// haal alle afspeellijsten op die bij het musicid horen.
        /// </summary>
        /// <param name="musicid"></param>
        /// <returns></returns>
        public static List<Playlist> GetPlaylists(int musicid)
        {
            string sqlS2 = "SELECT A.TITEL, A.ID FROM AFSPEELLIJST A " +
                           "JOIN MUZIEK_AFSPEELLIJST MA ON A.ID = MA.AFSPEELLIJSTID " +
                           "JOIN MUZIEK M ON MA.MUZIEKID = M.ID " + "WHERE M.ID = " + musicid;
            OracleCommand sqlC2 = new OracleCommand(sqlS2);
            List<Playlist> playlists = new List<Playlist>();


            try
            {
                OracleDataReader oraReader2 = ReadData(sqlC2);

                if (oraReader2 == null)
                    return playlists;
                while (oraReader2.Read())
                {
                    Playlist playlist = new Playlist();
                    playlist.ID = Convert.ToInt32(oraReader2["ID"]);
                    playlist.Name = Convert.ToString(oraReader2["TITEL"]);
                    playlists.Add(playlist);
                }


            }
            catch (InvalidOperationException ex)
            {
                return playlists;
            }
            return playlists;


        }
        /// <summary>
        /// kijken of er een account bestaat met die login en wachtwoord.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool Login(string login, string password)
        {
            OracleCommand cmd =
                new OracleCommand("SELECT * FROM account WHERE email = :email AND wachtwoord = :wachtwoordHash");
            cmd.Parameters
                .Add("email", OracleDbType.Varchar2).Value = login;

            cmd.Parameters
                .Add("wachtwoordHash", OracleDbType.Varchar2).Value = password;

            OracleDataReader oraReader = ReadData(cmd);
            if (oraReader.HasRows)
            {
                oraReader.Dispose();
                cmd.Dispose();
                CloseConnection();
                return true;
            }
            else
            {
                oraReader.Dispose();
                cmd.Dispose();
                CloseConnection();
                return false;
            }

        }
        /// <summary>
        /// account gegevens ophalen op email als iemand op zijn eigen naam klikt.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static Account GetAccount(string email)

        {
            Account account = new Account();
            OracleCommand cmd = new OracleCommand(@"SELECT * FROM account WHERE email = :email");
            cmd.Parameters.Add("email", OracleDbType.Varchar2).Value = email;

            try
            {
                OracleDataReader oraReader = ReadData(cmd);

                if (oraReader == null)
                    return account;
                while (oraReader.Read())
                {
                    account.ID = Convert.ToInt32(oraReader["ID"]);
                    account.Name = Convert.ToString(oraReader["Naam"]);
                    account.Email = email;
                    Music music = new Music();
                    music.ID = account.ID;
                    account.Music = music;
                    account.Music.Playlists = GetPlaylists(account.Music.ID);
                    foreach (Playlist playlist in account.Music.Playlists)
                    {
                        playlist.Songs = GetSongsPlaylist(playlist.ID);
                    }
                }


            }
            catch (InvalidOperationException ioe)
            {
                CloseConnection();
                return account;
            }

            CloseConnection();
            return account;
        }
        /// <summary>
        /// Haal alle gegevens op van een song
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Song GetSong(int songid)
        {
            string sqlS = "SELECT * FROM NUMMER " + "WHERE id =" + songid;
            OracleCommand sqlC = new OracleCommand(sqlS);
            Song song = new Song();
            try
            {
                OracleDataReader oraReader = ReadData(sqlC);


                if (oraReader == null)

                    return song;
                while (oraReader.Read())
                {
                    song.ID = Convert.ToInt32(oraReader["ID"]);
                    song.Artists = GetArtists(song.ID);
                    song.Releasedate = Convert.ToDateTime(oraReader["releasedate"]);
                    song.Name = Convert.ToString(oraReader["Titel"]);
                    song.Speelduur = Convert.ToDouble(oraReader["SPEELDUUR"]);
                    song.Genres = GetGenres(song.ID);
                }

                CloseConnection();

            }
            catch
                (InvalidOperationException ioe)
            {
                return song;
                throw;
            }

            return song;
        }
        /// <summary>
        /// voeg een song toe aan een playlist
        /// </summary>
        /// <param name="songID"></param>
        /// <param name="PlaylistID"></param>
        /// <returns></returns>
        public static bool AddSongToPlaylist(int songid, int playlistid)
        {
            string insertString =
                string.Format("INSERT INTO NUMMER_AFSPEELLIJST(NUMMERID, AFSPEELLIJSTID) " + "VALUES({0}, {1})", songid,
                    playlistid);

            OracleCommand insert = new OracleCommand(insertString, Conn);
            OracleCommand commit = new OracleCommand("Commit", Conn);

            try
            {
                Conn.Open();
                insert.ExecuteNonQuery();
                commit.ExecuteNonQuery();
            }
            catch (OracleException oe)
            {
                return false;
            }
            finally
            {
                CloseConnection();
            }
            return true;
        }
        /// <summary>
        /// Haal een playlist op
        /// </summary>
        /// <param name="playlistid"></param>
        /// <returns></returns>
        public static Playlist GetPlaylist(int playlistid)
        {
            string sqlS = "SELECT P.ID, P.TITEL FROM AFSPEELLIJST P " + "WHERE P.ID = " + playlistid;
            OracleCommand sqlC = new OracleCommand(sqlS);
            Playlist playlist = new Playlist();
            try
            {
                OracleDataReader oraReader = ReadData(sqlC);
                if (oraReader == null)

                    return playlist;
                while (oraReader.Read())
                {
                    playlist.ID = Convert.ToInt32(oraReader["ID"]);
                    playlist.Name = Convert.ToString(oraReader["TITEL"]);
                    playlist.Songs = GetSongsPlaylist(playlistid);
                }

            }
            catch
                (InvalidOperationException ioe)
            {
                CloseConnection();
                return playlist;
            }
            CloseConnection();
            return playlist;
        }
        /// <summary>
        /// haal alle liedjes op van playlist
        /// </summary>
        /// <param name="playlistid"></param>
        /// <returns></returns>
        public static List<Song> GetSongsPlaylist(int playlistid)
        {
            string sqlS = "SELECT * FROM NUMMER N " + "JOIN NUMMER_AFSPEELLIJST NA ON NA.NUMMERID = N.ID " +
                          "JOIN AFSPEELLIJST A ON NA.AFSPEELLIJSTID = A.ID " + "WHERE A.ID = " + playlistid;
            OracleCommand sqlC = new OracleCommand(sqlS);
            List<Song> songs = new List<Song>();

            try
            {
                OracleDataReader oraReader = ReadData(sqlC);
                if (oraReader == null)

                    return songs;
                while (oraReader.Read())
                {
                    Song song = new Song();
                    song.ID = Convert.ToInt32(oraReader["ID"]);
                    song.Artists = GetArtists(song.ID);
                    song.Name = Convert.ToString(oraReader["Titel"]);
                    song.Releasedate = Convert.ToDateTime(oraReader["ReleaseDate"]);
                    song.Genres = GetGenres(song.ID);
                    song.Speelduur = Convert.ToDouble(oraReader["Speelduur"]);
                    songs.Add(song);
                }
            }
            catch (InvalidOperationException ioe)
            {
                return songs;
            }

            return songs;
        }
        /// <summary>
        /// Haal alle songs op voor een album.
        /// </summary>
        /// <param name="albumID"></param>
        /// <returns></returns>
        public static List<Song> GetSongAlbum(int albumID)
        {
            string sqlS = "SELECT * FROM NUMMER N " + "JOIN ALBUM_NUMMER NA ON NA.NUMMERID = N.ID " +
                          "JOIN ALBUM A ON NA.ALBUMID = A.ID " + "WHERE A.ID = " + albumID;
            OracleCommand sqlC = new OracleCommand(sqlS);
            List<Song> songs = new List<Song>();

            try
            {
                OracleDataReader oraReader = ReadData(sqlC);
                if (oraReader == null)

                    return songs;
                while (oraReader.Read())
                {
                    Song song = new Song();
                    song.ID = Convert.ToInt32(oraReader["ID"]);
                    song.Artists = GetArtists(song.ID);
                    song.Name = Convert.ToString(oraReader["Titel"]);
                    song.Releasedate = Convert.ToDateTime(oraReader["ReleaseDate"]);
                    song.Genres = GetGenres(song.ID);
                    song.Speelduur = Convert.ToDouble(oraReader["Speelduur"]);
                    songs.Add(song);
                }
            }
            catch (InvalidOperationException ioe)
            {
                return songs;
            }

            return songs;
        }
        /// <summary>
        /// haal alle info van een album op
        /// </summary>
        /// <param name="albumID"></param>
        /// <returns></returns>
        public static Album GetAlbum(int albumID)
        {
            string sqlS = "SELECT * FROM ALBUM A WHERE A.ID = " + albumID;
            OracleCommand sqlC = new OracleCommand(sqlS);
            Album album = new Album();
            try
            {
                OracleDataReader oraReader = ReadData(sqlC);
                if (oraReader == null)
                    return album;

                while (oraReader.Read())
                {
                    album.ID = albumID;
                    album.ReleaseDate = Convert.ToDateTime(oraReader["ReleaseDate"]);
                    album.Titel = Convert.ToString(oraReader["TITEL"]);
                    album.Songs = GetSongAlbum(albumID);
                    album.Artist = GetArtistAlbum(albumID);
                }

            }
            catch (OracleException oe)
            {
                CloseConnection();
                return album;
            }
            CloseConnection();
            return album;
        }
        /// <summary>
        /// haal de artiest bij het album
        /// </summary>
        /// <param name="albumid"></param>
        /// <returns></returns>
        public static Artist GetArtistAlbum(int albumid)
        {
            string sqlS = "SELECT * FROM ARTIEST A " + "JOIN ALBUM_ARTIEST AA ON A.ID = AA.ARTIESTID " +
                          "JOIN ALBUM AL ON AL.ID = AA.ALBUMID " + "WHERE AL.ID = " + albumid;
            OracleCommand sqlC = new OracleCommand(sqlS);
            Artist artist = new Artist();

            try
            {
                OracleDataReader oraReader = ReadData(sqlC);
                if (oraReader == null)
                    return artist;
                while (oraReader.Read())
                {
                    artist.ID = Convert.ToInt32(oraReader["ID"]);
                    artist.Name = Convert.ToString(oraReader["Naam"]);
                }

            }
            catch (OracleException oe)
            {
                return artist;
            }
            return artist;

        }
        /// <summary>
        /// alle albums ophalen per account
        /// </summary>
        /// <param name="accountid"></param>
        /// <returns></returns>
        public static List<Album> GetAlbums(int accountid)
        {
            string sqlS = "Select A.Titel, A.ID, A.Releasedate from Album A " +
                          "JOIN MUZIEK_ALBUM MA ON A.ID = MA.ALBUMID " +
                          "JOIN MUZIEK M ON MA.MUZIEKID = M.ID " + "WHERE M.ID = " + accountid;
            OracleCommand slqC = new OracleCommand(sqlS);
            List<Album> albums = new List<Album>();

            try
            {
                OracleDataReader oraReader = ReadData(slqC);
                if (oraReader == null)
                    return albums;
                while (oraReader.Read())
                {
                    Album album = new Album();
                    album.ID = Convert.ToInt32(oraReader["ID"]);
                    album.Titel = Convert.ToString(oraReader["Titel"]);
                    album.ReleaseDate = Convert.ToDateTime(oraReader["Releasedate"]);
                    album.Artist = GetArtistAlbum(album.ID);
                    albums.Add(album);
                }
            }
            catch (OracleException)
            {
                CloseConnection();
                return albums;
            }
            CloseConnection();
            return albums;

        }
        /// <summary>
        /// haal alle artiest informatie op
        /// </summary>
        /// <param name="artistid"></param>
        /// <returns></returns>
        public static Artist GetArtist(int artistid)
        {
            string sqlS = "SELECT * FROM ARTIEST A " + "WHERE A.ID = " + artistid;
            OracleCommand sqlC = new OracleCommand(sqlS);
            Artist artist = new Artist();
            try
            {
                OracleDataReader oraReader = ReadData(sqlC);
                if (oraReader == null)
                    return artist;
                while (oraReader.Read())
                {
                    artist.ID = artistid;
                    artist.Name = Convert.ToString(oraReader["Naam"]);
                    artist.Songs = GetSongsArtist(artistid);
                    artist.Albums = GetAlbumsArtist(artistid);
                }
            }
            catch (OracleException oe)
            {
                CloseConnection();
                return artist;
            }
            CloseConnection();
            return artist;
        }
        /// <summary>
        /// haal alle nummers van de artiest op
        /// </summary>
        /// <param name="artistid"></param>
        /// <returns></returns>
        public static List<Song> GetSongsArtist(int artistid)
        {
            string sqlS = "SELECT * FROM NUMMER N " + "JOIN NUMMER_ARTIEST NA ON NA.NUMMERID = N.ID " + "JOIN ARTIEST A ON NA.ARTIESTID = A.ID " + "WHERE A.ID = " + artistid;
            OracleCommand slqC = new OracleCommand(sqlS);
            List<Song> songs = new List<Song>();

            try
            {
                OracleDataReader oraReader = ReadData(slqC);
                if (oraReader == null)
                    return songs;
                while (oraReader.Read())
                {
                    Song song = new Song();
                    song.ID = Convert.ToInt32(oraReader["ID"]);
                    song.Name = Convert.ToString(oraReader["Titel"]);
                    song.Releasedate = Convert.ToDateTime(oraReader["Releasedate"]);
                    song.Artists = GetArtists(song.ID);
                    song.Genres = GetGenres(song.ID);
                    song.Speelduur = Convert.ToDouble(oraReader["Speelduur"]);
                    songs.Add(song);
                }
            }
            catch (OracleException)
            {
                return songs;
            }
            return songs;
        }
        /// <summary>
        /// haal alle albums op van de artiest
        /// </summary>
        /// <param name="artistid"></param>
        /// <returns></returns>
        public static List<Album> GetAlbumsArtist(int artistid)
        {
            string sqlS = "SELECT * FROM ALBUM A " + "JOIN ALBUM_ARTIEST AA ON A.ID = AA.ALBUMID " + "JOIN ARTIEST AR ON AR.ID = AA.ARTIESTID " + "WHERE AR.ID =" + artistid;
            OracleCommand slqC = new OracleCommand(sqlS);
            List<Album> albums = new List<Album>();

            try
            {
                OracleDataReader oraReader = ReadData(slqC);
                if (oraReader == null)
                    return albums;
                while (oraReader.Read())
                {
                    Album album = new Album();
                    album.ID = Convert.ToInt32(oraReader["ID"]);
                    album.Titel = Convert.ToString(oraReader["Titel"]);
                    album.ReleaseDate = Convert.ToDateTime(oraReader["Releasedate"]);
                    album.Artist = GetArtistAlbum(album.ID);
                    albums.Add(album);
                }
            }
            catch (OracleException)
            {
                return albums;
            }
            return albums;
        }
        /// <summary>
        /// remove a song from a playlist
        /// </summary>
        /// <param name="playlistid"></param>
        /// <param name="songid"></param>
        /// <returns></returns>
        public static bool SongRemovePlaylist(int playlistid, int songid)
        {
            string sqlS = "DELETE FROM NUMMER_AFSPEELLIJST " + "WHERE AFSPEELLIJSTID = " + playlistid +
                          " AND NUMMERID = " + songid;
            OracleCommand sqlC = new OracleCommand(sqlS, Conn);
            OracleCommand commit = new OracleCommand("commit", Conn);

            try
            {
                Conn.Open();
                sqlC.ExecuteNonQuery();
                commit.ExecuteNonQuery();
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                CloseConnection();
            }
            return true;
        }
        /// <summary>
        /// zoekt in de database naar een string
        /// </summary>
        /// <param name="zoekwaarde"></param>
        /// <returns></returns>
        public static ZoekenModel Zoeken(string zoekwaarde)
        {
            ZoekenModel model = new ZoekenModel();
            string sqlAlbum = "SELECT * FROM ALBUM WHERE upper(TITEL) LIKE upper(" + zoekwaarde + ")";
            string sqlSong = "SELECT * FROM NUMMER WHERE UPPER(TITEL) LIKE UPPER(" + zoekwaarde + ")";
            string sqlArtist = "SELECT * FROM ARTIEST WHERE UPPER(NAAM) LIKE UPPER(" + zoekwaarde + ")";

            OracleCommand album = new OracleCommand(sqlAlbum);
            OracleCommand song = new OracleCommand(sqlSong);
            OracleCommand artist = new OracleCommand(sqlArtist);
            model.Albums = new List<Album>();
            model.Artists = new List<Artist>();
            model.Songs = new List<Song>();

            try
            {
                OracleDataReader albumReader = ReadData(album);
                OracleDataReader songReader = ReadData(song);
                OracleDataReader artistReader = ReadData(artist);

                if (albumReader != null)
                {
                    while (albumReader.Read())
                    {
                        Album al = new Album();
                        al.ID = Convert.ToInt32(albumReader["ID"]);
                        al.Titel = Convert.ToString(albumReader["TITEL"]);
                        al.Artist = GetArtistAlbum(al.ID);
                        al.Songs = GetSongAlbum(al.ID);
                        al.ReleaseDate = Convert.ToDateTime(albumReader["releasedate"]);
                        model.Albums.Add(al);
                    }
                }

                if (songReader != null)
                {
                    while (songReader.Read())
                    {
                        Song s = new Song();
                        s.ID = Convert.ToInt32(songReader["ID"]);
                        s.Name = Convert.ToString(songReader["Titel"]);
                        s.Releasedate = Convert.ToDateTime(songReader["Releasedate"]);
                        s.Artists = GetArtists(s.ID);
                        s.Genres = GetGenres(s.ID);
                        s.Speelduur = Convert.ToDouble(songReader["Speelduur"]);
                        model.Songs.Add(s);

                    }
                }
                if (artistReader != null)
                {
                    while (artistReader.Read())
                    {
                        Artist ar = new Artist();
                        ar.ID = Convert.ToInt32(artistReader["ID"]);
                        ar.Name = Convert.ToString(artistReader["Naam"]);
                        model.Artists.Add(ar);
                    }
                }

            }
            catch (OracleException)
            {
                CloseConnection();
                return model;
            }
            CloseConnection();
            return model;
        }
    }
}



