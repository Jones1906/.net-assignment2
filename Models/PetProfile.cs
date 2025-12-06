using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment1.Models;

public class PetProfile
{
    public int Id { get; set; }
    public int PetId { get; set; }
    
    //foreign key
    [ForeignKey("PetId")]
    public virtual Pet Pet { get; set; }
    
    public string VetNotes { get; set; }
    
    [NotMapped]
    public IFormFile VetNotesFile { get; set; }
}