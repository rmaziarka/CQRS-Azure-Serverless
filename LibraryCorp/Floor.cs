namespace LibraryCorp
{
    public class Floor
    {
        public Floor(string floorNumber, int size, int numberOfRooms)
        {
            FloorNumber = floorNumber;
            Size = size;
            NumberOfRooms = numberOfRooms;
        }

        public string FloorNumber { get; set; }

        public int Size { get; set; }

        public int NumberOfRooms { get; set; }
    }
}
