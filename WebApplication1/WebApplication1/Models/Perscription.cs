using Microsoft.VisualBasic;

namespace WebApplication1.Models;

public class Perscription
{
    public int PerscriptionID { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public string PatientLastName { get; set; }
    public string DoctorLastName { get; set; }
    
    // public int PatientID { get; set; }
    // public int DoctorID { get; set; }
}

public class Patients
{
   // public int PatientID { get; set; }
    public string PatientLastName { get; set; }
}

public class Doctor
{
  //  public int DoctorID { get; set; }
    public string DoctorLastName { get; set; }
}
