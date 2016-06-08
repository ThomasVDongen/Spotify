using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
        private const string PASSWORD = "thomas";

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
            string sqlS = "SELECT * FROM ";
            OracleCommand sqlC = new OracleCommand(sqlS);
            List<Song> songs = new List<Song>();

            OracleDataReader oraReader = ReadData(sqlC);

            if (oraReader == null)
                return songs;
            while (oraReader.Read())
            {
                
            }

            CloseConnection();

            return songs;
        }

    }
    }
