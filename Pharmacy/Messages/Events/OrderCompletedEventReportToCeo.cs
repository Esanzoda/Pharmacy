namespace Pharmasy.Messages.Events;

public class OrderCompletedEventReportToCeo
{
    public DateTime  day{ get; set; }
    public int  count { get; set; }
    public decimal  totalamout { get; set; }
}