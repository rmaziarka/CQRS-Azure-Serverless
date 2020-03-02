using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryCorp.Models
{
    public class ReservationId: TypedIdValueBase
    {
        public ReservationId(Guid value) : base(value)
        {
        }
    }
}
