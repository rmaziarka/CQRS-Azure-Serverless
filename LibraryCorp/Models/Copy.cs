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

        public void Block(string ownerId){
            IsTaken = true;
            OwnerId = ownerId;
        }

        public void ChangeOwner(string newOwnerId)
        {
            OwnerId = newOwnerId;
        }

        public void Release(){
            IsTaken = false;
            OwnerId = null;
        }

        public string BrandId { get; private set; }

        public string CopyNumber { get; private set; }

        public bool IsTaken { get; private set; }

        public string OwnerId { get; private set; }
    }
}
