using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Repositories;

public class PerscriptionRepository
{
    private readonly IConfiguration _configuration;

    public PerscriptionRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<bool> dateChecker(NewPerscription newPerscription) // -> przyjmuje to po czym sprawdzamy, raczej id
    {
        var date = newPerscription.Date;
        var dueDate = newPerscription.DueDate;
        return dueDate > date;
    }
    public async Task<bool> DoesDoctorExist(string lastName) // -> przyjmuje to po czym sprawdzamy, raczej id
    {
        var query = "SELECT 1 FROM Doctor WHERE @lastName = LastName";
	
        //// sql connection ////
	
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@lastName", lastName);
        await connection.OpenAsync();		
        var reader = await command.ExecuteScalarAsync();
		
        if (reader == null)
        {
            return false;
        }

        return true;
    }
    
    	public async Task<List<Perscription>> getPerscription(string doctorLastName)
    {
		var query = "SELECT P.IdPrescription AS IDPERSCRIPTION," +
                    "P.Date AS PERSCRIPTIONDATE," +
                    "P.DueDate AS DUEDATE," +
                    "PT.LastName AS PATIENTLAST," +
                    "D.LastName AS DOCTORLAST " +
                    "FROM Prescription P " +
                    "JOIN Doctor D on P.IdDoctor = D.IdDoctor " +
                    "JOIN Patient PT ON P.IdPatient = PT.IdPatient " +
                    "WHERE D.LastName = @doctorLastName";
		
		await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@doctorLastName", doctorLastName);
        await connection.OpenAsync();
        
		var reader = await command.ExecuteReaderAsync();
        var perscriptionIdOrdinal = reader.GetOrdinal("IDPERSCRIPTION");
        var dateOrdinal = reader.GetOrdinal("PERSCRIPTIONDATE"); 
        var dueDateOrdinal = reader.GetOrdinal("DUEDATE");
        var ptLastNameOrdinal = reader.GetOrdinal("PATIENTLAST");
        var dLastNameOrdinal = reader.GetOrdinal("DOCTORLAST");
        List < Perscription > perscriptions = new List<Perscription>();
		Perscription perscription = null;
		while (await reader.ReadAsync())
        {
            
                perscription = new Perscription()
                {
                    PerscriptionID = reader.GetInt32(perscriptionIdOrdinal),
                    Date = reader.GetDateTime(dateOrdinal),
                    DueDate = reader.GetDateTime(dueDateOrdinal),
                    PatientLastName = reader.GetString(ptLastNameOrdinal),
                    DoctorLastName = reader.GetString(dLastNameOrdinal),
                    
                };
                perscriptions.Add(perscription);
        }

        return perscriptions;
    }
    public async Task<List<Perscription>> getPerscription()
    {
        var query = "SELECT P.IdPrescription AS IDPERSCRIPTION," +
                    "P.Date AS PERSCRIPTIONDATE," +
                    "P.DueDate AS DUEDATE," +
                    "PT.LastName AS PATIENTLAST," +
                    "D.LastName AS DOCTORLAST " +
                    "FROM Prescription P " +
                    "JOIN Doctor D on P.IdDoctor = D.IdDoctor " +
                    "JOIN Patient PT ON P.IdPatient = PT.IdPatient ";
		
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = query;
        //command.Parameters.AddWithValue("@doctorLastName", doctorLastName);
        await connection.OpenAsync();
        
        var reader = await command.ExecuteReaderAsync();
        var perscriptionIdOrdinal = reader.GetOrdinal("IDPERSCRIPTION");
        var dateOrdinal = reader.GetOrdinal("PERSCRIPTIONDATE"); 
        var dueDateOrdinal = reader.GetOrdinal("DUEDATE");
        var ptLastNameOrdinal = reader.GetOrdinal("PATIENTLAST");
        var dLastNameOrdinal = reader.GetOrdinal("DOCTORLAST");
        List < Perscription > perscriptions = new List<Perscription>();
        Perscription perscription = null;
        while (await reader.ReadAsync())
        {
            
            perscription = new Perscription()
            {
                PerscriptionID = reader.GetInt32(perscriptionIdOrdinal),
                Date = reader.GetDateTime(dateOrdinal),
                DueDate = reader.GetDateTime(dueDateOrdinal),
                PatientLastName = reader.GetString(ptLastNameOrdinal),
                DoctorLastName = reader.GetString(dLastNameOrdinal),
                    
            };
            perscriptions.Add(perscription);
        }

        return perscriptions;
    }
    public async Task addPerscription(NewPerscription newPerscription)
    {
        var insert = @"INSERT INTO Perscription VALUES(@Date, @DueDate, @IdPatient, @IdDoctor);
                   SELECT @@IDENTITY AS ID;"; // -> to daje auto id
    
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
    
        command.Connection = connection;
        command.CommandText = insert;
    
        command.Parameters.AddWithValue("@Date", newPerscription.Date);
        command.Parameters.AddWithValue("@DueDate", newPerscription.DueDate);
        command.Parameters.AddWithValue("@IdPatient", newPerscription.IdPatient);
        command.Parameters.AddWithValue("@IdDoctor", newPerscription.IdDoctor);
    
        await connection.OpenAsync();
        // tu ewentualnie transakcja

        await command.ExecuteNonQueryAsync();


    }
}