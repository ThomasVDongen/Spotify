using System;
using System.Collections.Generic;
using System.Linq;
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
        private const string USER = "dbi323229";

        /// <summary>
        /// The password used to connect with the database.
        /// </summary>
        private const string PASSWORD = "wXkDxOdQUV";

        /// <summary>
        /// The oracle-specific string used to connect with the database.
        /// </summary>
        private const string CONNECTION_STRING = "User Id= " + USER + ";Password= " + PASSWORD + ";Data Source=" + @" //192.168.15.50:1521/fhictora" + ";";

        private static readonly OracleConnection Conn;

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
            Console.WriteLine(Conn.State == System.Data.ConnectionState.Open ? "Database Connection was already open" : "Opening Database Connection..");
            try
            {
                Conn.Open();
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
            Console.WriteLine(Conn.State == System.Data.ConnectionState.Closed ? "Database Connection was already closed" : "Closing Database Connection..");
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
        }

        public static List<Song> GetSongs()
        {
            string sqlS = "SELECT * FROM NUMMER";
            OracleCommand sqlC = new OracleCommand(sqlS);
            List<Song> songs = new List<Song>();

            OracleDataReader oraReader = ReadData(sqlC);

            if (oraReader == null)
                return songs;
            while (oraReader.Read())
            {
                Song song = new Song();
                song.ID = Convert.ToInt32(oraReader["ID"]);
                song.Artists = GetArtistsID(song.ID);
                song.Releasedate = Convert.ToDateTime(oraReader["releasedate"]);
                song.Name = Convert.ToString(oraReader["Titel"]);
                song.Speelduur = Convert.ToDouble(oraReader["SPEELDUUR"]);
                song.Genres = GetGenresID(song.ID);
            }

            CloseConnection();

            return songs;
        }

        public static List<Artist> GetArtistsID(int id)
        {
            string sqlS = "SELECT NA.ARTIESTID FROM NUMMER_ARTIEST NA" + "JOIN NUMMER N ON NA.NUMMERID = N.ID " + "WHERE N.ID = " + id;
            OracleCommand sqlC = new OracleCommand(sqlS);
            List<Artist> ArtistIDs = new List<Artist>();
            OracleDataReader oraReader = ReadData(sqlC);
            Artist artist = new Artist();

            if (oraReader == null)
                return ArtistIDs;
            while (oraReader.Read())
            {
                artist.ID = Convert.ToInt32(oraReader["ARTIESTID"]);
                ArtistIDs.Add(artist);
            }

            CloseConnection();
            return ArtistIDs;
        }

        public static List<Genre> GetGenresID(int id)
        {
            string sqlS = "SELECT NG.GENREID FROM NUMMER_GENRE NG" + "JOIN NUMMER N ON NG.NUMMERID = N.ID" + "WHERE N.ID = " + id;
            OracleCommand sqlC = new OracleCommand(sqlS);
            List<Genre> GenresID = new List<Genre>();
            Genre genre = new Genre();

            OracleDataReader oraReader = ReadData(sqlC);

            if (oraReader == null)
                return GenresID;
            while (oraReader.Read())
            {
                genre.ID = Convert.ToInt32(oraReader["GENREID"]);
                GenresID.Add(genre);

            }

            CloseConnection();
            return GenresID;
        }

        public static List<Playlist> GetPlaylists(int musicid)
        {
            string sqlS = "SELECT A.TITEL, A.ID FROM AFSPEELLIJST A" + "JOIN MUZIEK_AFSPEELLIJST MA ON A.ID = MA.AFSPEELLIJSTID" +
                "JOIN MUZIEK M ON MA.MUZIEKID = M.ID" + "WHERE M.ID =" + musicid;
            OracleCommand sqlC = new OracleCommand(sqlS);
            List<Playlist> playlists = new List<Playlist>();
            Playlist playlist = new Playlist();

            OracleDataReader oraReader = ReadData(sqlC);

            if (oraReader == null)
                return playlists;
            while (oraReader.Read())
            {
                playlist.ID = Convert.ToInt32(oraReader["ID"]);
                playlist.Name = Convert.ToString(oraReader["TITEL"]);
                playlists.Add(playlist);

            }

            CloseConnection();
            return playlists;

        }

        public static bool Login(string login, string password)
        {
            OracleCommand cmd = new OracleCommand("SELECT * FROM account WHERE email = :email AND wachtwoord = :wachtwoordHash");
            cmd.Parameters
                .Add("email", OracleDbType.NVarchar2)
                .Value = login;

            cmd.Parameters
                .Add("wachtwoordHash", OracleDbType.NVarchar2)
                .Value = password;

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

        public static Account GetAccount(string email)
        {
            Account account = new Account();
            OracleCommand cmd = new OracleCommand("SELECT * FROM account WHERE email = " + email);
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
            }

            CloseConnection();
            return account;

        }


    }
}
