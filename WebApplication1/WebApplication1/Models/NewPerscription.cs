using System.Collections;
using Microsoft.VisualBasic;

namespace WebApplication1.Models;

public class NewPerscription
{
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public int IdPatient { get; set; }
    public int IdDoctor { get; set; }
    
}