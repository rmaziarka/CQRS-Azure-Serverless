using System;

namespace LibraryCorp.Models
{
    public class Copy : IAggregate
    {
        public Copy(BrandId brandId, string serialNumber)
        {
            CopyId = new CopyId(Guid.NewGuid());
            BrandId = brandId;
            CopyNumber = serialNumber;
            IsTaken = false;
        }

        private Copy()
        {
            
        }

        public void Block(OwnerId ownerId){
            IsTaken = true;
            OwnerId = ownerId;
        }

        public void ChangeOwner(OwnerId newOwnerId)
        {
            OwnerId = newOwnerId;
        }

        public void Release(){
            IsTaken = false;
            OwnerId = null;
        }

        public CopyId CopyId { get; private set; }

        public BrandId BrandId { get; private set; }

        public string CopyNumber { get; private set; }

        public bool IsTaken { get; private set; }

        public OwnerId OwnerId { get; private set; }
    }
}
