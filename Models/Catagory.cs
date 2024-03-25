using System.ComponentModel.DataAnnotations.Schema;

namespace FixIt.Models;

public partial class Catagory
{
    public int CId { get; set; }

    public string? CName { get; set; }

    public int? BId { get; set; }
    [NotMapped]
    public IFormFile clientFile { get; set; }
    public byte[]? Photo { get; set; }

    public virtual Brand? BIdNavigation { get; set; }

    public virtual ICollection<SparePart> SpareParts { get; set; } = new List<SparePart>();
}
