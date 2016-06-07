using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using Microsoft.SqlServer.Server;

namespace Spotify.Models.Objecten
{
    public class Account
    {
        private int iD;
        private string name;
        private string email;
        private List<Account> followList = new List<Account>();
        private List<Account> followers = new List<Account>();
        private int payDay;
        private DateTime trialStartDate;
        private int trial;

        public int ID
        {
            get { return iD; }
        }

        public string Name
        {
            get { return Name; }
            set { Name = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public List<Account> FollowList
        {
            get { return followList; }
            set { followList = value; }
        }

        public List<Account> Followers
        {
            get { return followers; }
            set { followers = value; }
        }

        public int PayDay
        {
            get { return payDay; }
            set { payDay = value; }
        }

        public DateTime TrialStartDate
        {
            get { return trialStartDate; }
            set { trialStartDate = value; }
        }

        public int Trial
        {
            get { return trial; }
            set { trial = value; }
        }

        public Music Music { get; set; }

        
        public int CalculateTrial(DateTime datetime)
        {
            return Convert.ToInt32(DateTime.Now - datetime);
        }

        public void StartTrial()
        {
            trialStartDate = DateTime.Now;
        }

        public void Follow(Account follow)
        {
            FollowList.Add(follow);
        }

        public void GetFollower(Account follower)
        {
            Followers.Add(follower);
        }



    }
}