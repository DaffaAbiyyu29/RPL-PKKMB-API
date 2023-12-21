using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PKKMB_API.Model;

namespace PKKMB_API.Repository
{
	public class TugasRepository
	{
		private readonly string _connectionString;
		private readonly SqlConnection _connection;
		private readonly ResponseModel _response = new ResponseModel();

		public TugasRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
			_connection = new SqlConnection(_connectionString);
		}

		public List<TugasModel> GetAllData()
		{
			List<TugasModel> tugasList = new List<TugasModel>();
			try
			{
				string query = "SELECT * FROM pkm_trtugas";
				SqlCommand command = new SqlCommand(query, _connection);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					TugasModel tugas = new TugasModel
					{
						tgs_idtugas = reader["tgs_idtugas"].ToString(),
						tgs_nim = reader["tgs_nim"].ToString(),
						tgs_jenistugas = reader["tgs_jenistugas"].ToString(),
						tgs_tglpemberiantugas = DateTime.Parse(reader["tgs_tglpemberiantugas"].ToString()),
						tgs_filetugas = reader["tgs_filetugas"].ToString(),
						tgs_deadline = DateTime.Parse(reader["tgs_deadline"].ToString()),
						tgs_status = reader["tgs_status"].ToString(),
					};
					tugasList.Add(tugas);
				}
				reader.Close();
				_connection.Close();
				return tugasList;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}

		public TugasModel GetData(string tgs_idtugas)
		{
			TugasModel tugas = new TugasModel();
			try
			{
				string query = "SELECT * FROM pkm_trtugas WHERE tgs_idtugas = @p1";
				SqlCommand command = new SqlCommand(query, _connection);
				command.Parameters.AddWithValue("@p1", tgs_idtugas);
				_connection.Open();
				SqlDataReader reader = command.ExecuteReader();

				if (reader.Read())
				{
					tugas = new TugasModel
					{
						tgs_idtugas = reader["tgs_idtugas"].ToString(),
						tgs_nim = reader["tgs_nim"].ToString(),
						tgs_jenistugas = reader["tgs_jenistugas"].ToString(),
						tgs_tglpemberiantugas = DateTime.Parse(reader["tgs_tglpemberiantugas"].ToString()),
						tgs_filetugas = reader["tgs_filetugas"].ToString(),
						tgs_deadline = DateTime.Parse(reader["tgs_deadline"].ToString()),
						tgs_status = reader["tgs_status"].ToString(),
					};

					reader.Close();
					_connection.Close();
					return tugas;
				}
				else
				{
					// Tugas not found
					reader.Close();
					_connection.Close();
					return null;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}

		public ResponseModel InsertTugas(TugasModel tugasModel)
		{
			try
			{
				SqlCommand command = new SqlCommand("sp_TambahTugas", _connection);
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@p_nim", tugasModel.tgs_nim);
				command.Parameters.AddWithValue("@p_jenistugas", tugasModel.tgs_jenistugas);
				command.Parameters.AddWithValue("@p_tglpemberiantugas", tugasModel.tgs_tglpemberiantugas);
				command.Parameters.AddWithValue("@p_filetugas", tugasModel.tgs_filetugas);
				command.Parameters.AddWithValue("@p_deadline", tugasModel.tgs_deadline);
				command.Parameters.AddWithValue("@p_status", tugasModel.tgs_status);

				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();

				_response.status = 200;
				_response.messages = "Tugas berhasil ditambahkan";
				_response.data = tugasModel;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				_response.status = 500;
				_response.messages = "Terjadi kesalahan saat menambahkan Tugas = " + ex.Message;
				_response.data = null;
			}

			return _response;
		}

		public ResponseModel UpdateTugas(TugasModel tugas)
		{
			try
			{
				using SqlCommand command = new SqlCommand("sp_UpdateTugas", _connection);
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@p_idtugas", tugas.tgs_idtugas);
				command.Parameters.AddWithValue("@p_nim", tugas.tgs_nim);
				command.Parameters.AddWithValue("@p_jenistugas", tugas.tgs_jenistugas);
				command.Parameters.AddWithValue("@p_tglpemberiantugas", tugas.tgs_tglpemberiantugas);
				command.Parameters.AddWithValue("@p_filetugas", tugas.tgs_filetugas);
				command.Parameters.AddWithValue("@p_deadline", tugas.tgs_deadline);
				command.Parameters.AddWithValue("@p_status", tugas.tgs_status);

				_connection.Open();
				command.ExecuteNonQuery();
				_connection.Close();

				_response.status = 200;
				_response.messages = "Tugas berhasil diupdate";
				_response.data = tugas;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				_response.status = 500;
				_response.messages = "Gagal mengupdate Tugas, " + ex.Message;
				return _response;
			}

			return _response;
		}
	}
}
