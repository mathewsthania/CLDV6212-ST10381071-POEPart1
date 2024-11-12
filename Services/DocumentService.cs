
//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*START*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//
using Azure.Storage.Files.Shares;
using Microsoft.Data.SqlClient;

namespace CLDV6212_ST10381071_POEPart1.Services
{
	public class DocumentService
	{
		private readonly IConfiguration _configuration;

		public DocumentService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		// Insert document into the database
		public async Task InsertDocumentAsync(byte[] documentData, string fileName)
		{
			var connectionString = _configuration.GetConnectionString("DefaultConnection");
			var query = @"INSERT INTO DocumentTable (DocumentFileName, DocumentData) VALUES (@DocumentFileName, @DocumentData)";

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				SqlCommand command = new SqlCommand(query, connection);
				command.Parameters.AddWithValue("@DocumentFileName", fileName);
				command.Parameters.AddWithValue("@DocumentData", documentData);

				connection.Open();
				await command.ExecuteNonQueryAsync();
			}
		}
	}
}

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*END*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//