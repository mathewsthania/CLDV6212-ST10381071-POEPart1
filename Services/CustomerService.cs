using Azure.Data.Tables;
using CLDV6212_ST10381071_POEPart1;
using CLDV6212_ST10381071_POEPart1.Models;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*START*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//

namespace CLDV6212_ST10381071_POEPart1.Services
{
    public class CustomerService 
    {
		private readonly IConfiguration _configuration;

		public CustomerService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task InsertCustomerAsync(CustomerProfile profile)
		{
			var connectionString = _configuration.GetConnectionString("DefaultConnection");
			var query = @"INSERT INTO CustomerProfile (FirstName, LastName, Email, PhoneNumber)
                          VALUES (@FirstName, @LastName, @Email, @PhoneNumber)";

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				SqlCommand command = new SqlCommand(query, connection);
				command.Parameters.AddWithValue("@FirstName", profile.FirstName);
				command.Parameters.AddWithValue("@LastName", profile.LastName);
				command.Parameters.AddWithValue("@Email", profile.Email);
				command.Parameters.AddWithValue("@PhoneNumber", profile.PhoneNumber);

				connection.Open();
				await command.ExecuteNonQueryAsync();
			}
		}
	}
}

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*END*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//