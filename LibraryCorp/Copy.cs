using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LibraryCorp
{
    public class Copy
    {
        public Copy(string libraryId, string brandId, string serialNumber)
        {
            Id = Guid.NewGuid().ToString();
            LibraryId = libraryId;
            BrandId = brandId;
            CopyNumber = serialNumber;
            IsTaken = false;
        }

        public void Block(){
            IsTaken = true;
        }

        public void Release(){
            IsTaken = false;
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        public string LibraryId { get; private set; }

        public string CopyNumber { get; private set; }

        public string BrandId { get; private set; }

        public bool IsTaken { get; private set; }

    }
}
