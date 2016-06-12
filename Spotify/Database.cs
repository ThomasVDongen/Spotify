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
        private const string CONNECTION_STRING =
            "User Id= " + USER + ";Password= " + PASSWORD + ";Data Source=" + @" //192.168.15.50:1521/fhictora" + ";";

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
        public static List<Song> GetSongs(int id)
        {
            string sqlS = "SELECT * FROM NUMMER N " + "JOIN MUZIEK_NUMMER MN ON N.ID = MN.NUMMERID " +
                          "WHERE MN.MUZIEKID =" + id;
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
        public static List<Artist> GetArtists(int id)
        {
            string sqlS = "SELECT A.ID, A.NAAM FROM ARTIEST A " + "JOIN NUMMER_ARTIEST NA ON NA.ARTIESTID = A.ID " +
                          "JOIN NUMMER N ON NA.NUMMERID = N.ID " + "WHERE N.ID = " + id;
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
        public static List<Genre> GetGenres(int id)
        {
            string sqlS = "SELECT G.ID, G.NAAM FROM GENRE G " + "JOIN NUMMER_GENRE NG ON NG.GENREID = G.ID " +
                          "JOIN NUMMER N ON NG.NUMMERID = N.ID " + "WHERE N.ID = " + id;
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
        public static Song GetSong(int id)
        {
            string sqlS = "SELECT * FROM NUMMER " + "WHERE id =" + id;
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

        public static bool AddSongToPlaylist(int songID, int PlaylistID)
        {
            string insertString =
                string.Format("INSERT INTO NUMMER_AFSPEELLIJST(NUMMERID, AFSPEELLIJSTID) " + "VALUES({0}, {1})", songID,
                    PlaylistID);

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
                Conn.Close();
            }
            return true;
        }

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
                    songs.Add(song);
                }
            }
            catch (InvalidOperationException ioe)
            {
                return songs;
            }

            return songs;
        }
    }
}



