using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using static accessdb.CCsv;

namespace accessdb
{
    class Program
    {
        public class Municipality
        {
            public ObjectId _id { get; set; }
            public string Name { get; set; }
            public string NameLower { get; set; }
            public string UpdatedAt { get; set; }
            public string CreatedAt { get; set; }
            public bool Enabled { get; set; }
            public string HubID { get; set; }
        }
        public class ClientAddress
        {
            public ObjectId _id { get; set; }
            public string City { get; set; }
            public ObjectId CityId { get; set; }
            public string Street { get; set; }
            public string StreetLower { get; set; }
            public string CivicNumber { get; set; }
            public string PostalCode { get; set; }
            public int ExternalId { get; set; }
            public string AptNumber { get; set; }
        }
        public class Client
        {
            public ObjectId _id { get; set; }
            public string FirstName { get; set; }
            public string FirstNameLower { get; set; }
            public string LastName { get; set; }
            public string LastNameLower { get; set; }
            public string OBNLNumber { get; set; }
            public string OBNLNumbers { get; set; }
            public string Category { get; set; }
            public string CategoryCustom { get; set; }
            public string PhoneNumber { get; set; }
            public string MobilePhoneNumber { get; set; }
            public string Email { get; set; }
            public string EmailLower { get; set; }
            public ClientAddress Address { get; set; }
            public ClientAddress[] PreviousAddresses { get; set; }
            public Object Hub { get; set; }
            public string MunicipalityId { get; set; }
            public int Status { get; set; }
            public string RegistrationDate { get; set; }
            public string LastChange { get; set; }
            public bool Verified { get; set; }
            public string Comments { get; set; }
            public bool VisitsLimitExceeded { get; set; }
            public int PersonalVisitsLimit { get; set; }
            public string RefId { get; set; }
        }
        static void Main(string[] args)
        {
            List<Table> m_rows = new CCsv().ReadCsv<Table>("history.csv");

            MongoClient dbClient = new MongoClient("mongodb://127.0.0.1:27017");
            IMongoDatabase db = dbClient.GetDatabase("test");
            var municipality = db.GetCollection<Municipality>("municipality");
            var address = db.GetCollection<ClientAddress>("ClientAddress");
            var client = db.GetCollection<Client>("Client");

            for (int i = 0; i < 4; i++)
            {
                // Municipality
                Municipality mun = new Municipality();
                mun.Name = m_rows[i].City;
                mun.NameLower = m_rows[i].City.ToLower();
                mun.UpdatedAt = DateTime.UtcNow.ToString();
                mun.CreatedAt = DateTime.UtcNow.ToString();
                mun.Enabled = true;
                mun.HubID = null;
                municipality.InsertOne(mun);
                // Address
                ClientAddress adr = new ClientAddress();
                adr.City = m_rows[i].City;
                adr.CityId = mun._id;
                adr.Street = m_rows[i].Street;
                adr.StreetLower = m_rows[i].Street.ToLower();
                adr.CivicNumber = m_rows[i].CivicNumber;
                adr.PostalCode = m_rows[i].PostalCode;
                adr.ExternalId = 0;
                adr.AptNumber = m_rows[i].AptNumber;
                address.InsertOne(adr);
                // Client
                Client cli = new Client();
                cli.FirstName = m_rows[i].FirstName;
                cli.FirstNameLower = m_rows[i].FirstName.ToLower();
                cli.LastName= m_rows[i].LastName;
                cli.LastNameLower= m_rows[i].LastName.ToLower();
                cli.OBNLNumber = null;
                cli.OBNLNumbers = null;
                cli.Category = "resident";
                cli.CategoryCustom = null;
                cli.PhoneNumber = "";
                cli.MobilePhoneNumber = m_rows[i].MobilePhoneNumber;
                cli.Email = m_rows[i].Email;
                cli.EmailLower = m_rows[i].Email.ToLower();
                cli.Address = adr;
                cli.PreviousAddresses = new ClientAddress[0];
                cli.Hub = null;
                cli.MunicipalityId = null;
                cli.Status = 0;
                cli.RegistrationDate = DateTime.UtcNow.ToString();
                cli.LastChange = DateTime.UtcNow.ToString();
                cli.Verified = true;
                cli.Comments = "";
                cli.VisitsLimitExceeded = false;
                cli.PersonalVisitsLimit = 0;
                cli.RefId = null;
                client.InsertOne(cli);
            }
        }
    }
}
