namespace HW1.BL
{
    public class RentRequest
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public DateTime RentStart { get; set; }
        public DateTime RentEnd { get; set; }
        public float TotalPrice { get; set; }

    }
}
