using Azure;
using Azure.Data.Tables;
using System;

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*START*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

namespace CLDV6212_ST10381071_POEPart1.Models
{
    // defining customerProfile that implements te ITableEntity interface
    public class customerProfile : ITableEntity
    {
        public string PartionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? TimeStamp { get; set; }
        public Etag Etag { get; set; }


        // Custom Properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public customerProfile()
        {
            PartionKey = "CustomerProfile";
            RowKey = Guid.NewGuid().ToString();
        }
    }
}
