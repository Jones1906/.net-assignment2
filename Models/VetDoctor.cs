namespace Assignment1.Models;

public class VetDoctor
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Specialty { get; set; }
    
   
    public virtual ICollection<Pet> Pets { get; set; } = new  List<Pet>();
}