using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment1.Models;

public class Pet 
{
    
    public int Id { get; set; }
    public string MicrochipId { get; set; }
    public string Name { get; set; }
    public string Species { get; set; }
    public int VetDoctorId { get; set; }
    
    //foregin key
    [ForeignKey("VetDoctorId")]
    public virtual VetDoctor VetDoctor { get; set; }
    
    
    //one to one 
    public virtual PetProfile PetProfile { get; set; }
    
}