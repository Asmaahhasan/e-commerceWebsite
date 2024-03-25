using FixIt.Models;

namespace FixIt;

public partial class OrderItem
{
    public int OitemId { get; set; }

    public int? Oid { get; set; }

    public int? SId { get; set; }

    public int? Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public virtual Payment? OidNavigation { get; set; }

    public virtual SparePart? SIdNavigation { get; set; }
}
