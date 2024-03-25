using FixIt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FixIt.Models;

public partial class SparePart
{
    public int SId { get; set; }

    public string? SName { get; set; }

    public string? Describtion { get; set; }

    public int? CId { get; set; }

    public double? Price { get; set; }
    [NotMapped]
    public IFormFile ClientFile { get; set; }
    public byte[]? Photo { get; set; }

    public int? Quantity { get; set; }

    public virtual Catagory? CIdNavigation { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}

