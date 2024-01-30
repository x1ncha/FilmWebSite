using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieManager.Data;

public class Movie
{
    public int Id { get; set; }

    [Display(Name = "Film Adı")]
    [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
    [MinLength(4, ErrorMessage = "{0} alanı en az {1} karakter olmalıdır.")]
    public required string Name { get; set; }

    [Display(Name = "Açıklama")]
    public string? Description { get; set; }
    
    [Display(Name = "Görsel")]
    public string? Image { get; set; }
    
    [Display(Name = "Yıl")]
    [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
    public int Year { get; set; }

    [Display(Name = "Görüntülenme")]
    public int Views { get; set; }

    [Display(Name = "Tür")]
    [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
    public int GenreId { get; set; }

    [NotMapped]
    public IFormFile? ImageFile { get; set; }

    public virtual Genre? Genre { get; set; }
    public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

}
