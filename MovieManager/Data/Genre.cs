using System.ComponentModel.DataAnnotations;

namespace MovieManager.Data;

public class Genre
{
    public int Id { get; set; }

    [Display(Name = "Tür Adı")]
    [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
    [MinLength(4, ErrorMessage = "{0} alanı en az {1} karakter olmalıdır.")]
    public required string Name { get; set; }

    public virtual ICollection<Movie> Movies { get; set; } = new HashSet<Movie>();
}


