using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;

namespace SpotifyMatchmaker.Library.Models
{

    public partial class Party : TableEntity
    {
        /// <summary>
        /// Unique party code to pair friends together
        /// </summary>
        /// <value></value>
        public string PartyCode { get => this.RowKey; set => this.RowKey = value; }

        /// <summary>
        /// The host's Spotify access token
        /// </summary>
        /// <value></value>
        public string Host { get; set; }

        /// <summary>
        /// Spotift access token for person #2
        /// </summary>
        /// <value></value>
        public string Person2 { get; set; }

        /// <summary>
        /// Spotify access token for person #3
        /// </summary>
        /// <value></value>
        public string Person3 { get; set; }

        /// <summary>
        /// Spotify access token for person #4
        /// </summary>
        /// <value></value>
        public string Person4 { get; set; }

        /// <summary>
        /// Spotify access token for person #5
        /// </summary>
        /// <value></value>
        public string Person5 { get; set; }

        public Party()
        {
        }

        public Party(string partyCode)
        {
            this.PartyCode = partyCode;
            this.PartitionKey = "parties";
        }

        public string[] GetAccessTokens()
        {
            var accessTokens = new List<string>();

            if(!String.IsNullOrEmpty(this.Host))
            {
                accessTokens.Add(this.Host);
            }
            if(!String.IsNullOrEmpty(this.Person2))
            {
                accessTokens.Add(this.Person2);
            }
            if(!String.IsNullOrEmpty(this.Person3))
            {
                accessTokens.Add(this.Person3);
            }
            if(!String.IsNullOrEmpty(this.Person4))
            {
                accessTokens.Add(this.Person4);
            }
            if(!String.IsNullOrEmpty(this.Person5))
            {
                accessTokens.Add(this.Person5);
            }

            return accessTokens.ToArray();
        }
    }
}
