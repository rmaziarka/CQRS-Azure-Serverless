using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate
{
    public class CreateEstate
    {
        public Estate ToEstate()
        {
            return new Estate(companyId: CompanyId, name: Name, location: Location, price: Price, 
                floors: Floors.ConvertAll(thisFloor => new Floor(floorNumber: thisFloor.FloorNumber, size: thisFloor.Size, numberOfRooms: thisFloor.NumberOfRooms)));
        }

        public String CompanyId { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public decimal Price { get; set; }

        public List<CreateFloor> Floors { get; set; }
    }
}
