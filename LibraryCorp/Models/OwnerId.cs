using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryCorp.Models
{
    public class OwnerId: TypedIdValueBase
    {
        public OwnerId(Guid value) : base(value)
        {
        }

        public OwnerId(ReservationId id): base(id.Value){ }

        public OwnerId(BorrowId id): base(id.Value){ }

        private OwnerId(){ }
    }
}
