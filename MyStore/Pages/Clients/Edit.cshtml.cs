using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyStore.Pages.Clients
{
    public class EditModel : PageModel
	{
		public ClientInfo clientInfo = new ClientInfo();
		public string errorMessage = "";
		public string successMessage = "";
		public void OnGet()
        {
			String id = Request.Query["id"];

			try
			{
				String connectionString = "Data Source=localhost;Initial Catalog=mystore;Integrated Security=True";

				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					string sql = "Select * FROM clients where id=@id";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("id", id);
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								clientInfo.id = "" + reader.GetInt32(0);
								clientInfo.name = reader.GetString(1);
								clientInfo.email = reader.GetString(2);
								clientInfo.phone = reader.GetString(3);
								clientInfo.address = reader.GetString(4);
							}
						}
					}
				}
			}
			catch(Exception ex) 
			{
				errorMessage  = ex.Message;
			}
        }

		public void OnPost() 
		{
			clientInfo.id = Request.Form["id"];
			clientInfo.name = Request.Form["name"];
			clientInfo.email = Request.Form["email"];
			clientInfo.phone = Request.Form["phone"];
			clientInfo.address = Request.Form["address"];

			if (clientInfo.name.Length == 0 || clientInfo.email.Length == 0 || clientInfo.phone.Length == 0 || clientInfo.address.Length == 0)
			{
				errorMessage = "All field are required";
				return;
			}

			// save to db
			try
			{
				String connectionString = "Data Source=localhost;Initial Catalog=mystore;Integrated Security=True";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					string sql = "UPDATE clients " +
								 "set name=@name, email=@email, phone=@phone, address=@address " +
								 "WHERE id=@id";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@name", clientInfo.name);
						command.Parameters.AddWithValue("@email", clientInfo.email);
						command.Parameters.AddWithValue("@phone", clientInfo.phone);
						command.Parameters.AddWithValue("@address", clientInfo.address);
						command.Parameters.AddWithValue("@id", clientInfo.id);

						command.ExecuteNonQuery();
					}
				}

			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
			}
			//clientInfo.name = "";
			//clientInfo.email = "";
			//clientInfo.phone = "";
			//clientInfo.address = "";

			//successMessage = "New client added correctly";

			Response.Redirect("/Clients/Index");

		}
	}
}
