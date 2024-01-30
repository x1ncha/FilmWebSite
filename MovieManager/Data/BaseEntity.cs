using System.ComponentModel.DataAnnotations;

namespace MovieManager.Data
{
    public abstract class BaseEntity
    {
        public virtual int Id { get; set; }

        [Display(Name = "Aktif")]
        public bool Enabled { get; set; } = true;

        public virtual DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public virtual int UserId { get; set; }

        public virtual AppUser? CreatorUser { get; set; }
    }

}
