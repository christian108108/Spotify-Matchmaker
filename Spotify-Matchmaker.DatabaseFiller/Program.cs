﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using SpotifyMatchmaker.Models;

namespace DatabaseFiller
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            List<Party> parties = new List<Party>();

            for(int i=0; i<100; i++)
            {
                Party newParty = MakeRandomParty();

                parties.Add(newParty);

                ;
            }

            //open file stream
            using (StreamWriter file = File.CreateText(@"testParties.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(file, parties);
            }
            ;
        }

        public static Party MakeRandomParty()
        {
            Party party = new Party();

            party.PartyCode = RandomString(6);
            party.Host = RandomString(258);
            party.Person2 = RandomString(258);
            party.Person3 = RandomString(258);
            party.Person4 = RandomString(258);
            party.Person5 = RandomString(258);

            return party;
        }

        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
