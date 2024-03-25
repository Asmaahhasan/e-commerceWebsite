namespace FixIt.Models;

public partial class Cart
{
    public int CartId { get; set; }

    public int? UId { get; set; }

    public int? SId { get; set; }

    public int? Quantity { get; set; }

    public virtual SparePart? SIdNavigation { get; set; }

    public virtual Users? UIdNavigation { get; set; }
}
