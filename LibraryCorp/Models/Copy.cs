namespace LibraryCorp.Models
{
    public class Copy : Aggregate
    {
        public Copy(string libraryId, string brandId, string serialNumber)
        {
            BrandId = brandId;
            CopyNumber = serialNumber;
            IsTaken = false;
        }

        private Copy()
        {
            
        }

        public void Block(){
            IsTaken = true;
        }

        public void Release(){
            IsTaken = false;
        }

        public string BrandId { get; private set; }

        public string CopyNumber { get; private set; }

        public bool IsTaken { get; private set; }
    }
}
