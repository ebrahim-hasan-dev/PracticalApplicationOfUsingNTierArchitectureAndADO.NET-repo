using System;
using System.Data;
using System.Data.SqlClient;
using ContactsApp_ModulesLayer;



namespace ContactsApp_DataAccessLayer
{
    public class ClsDataHandling
    {
        //static string _ConnectionString = "Server=.;Database=ContactsDB_3_TierArchitecture;User Id=sa;Password=123456";
        static string _ConnectionString = "Server=.;Database=ContactsDB_N_TierArchitecture;User Id=sa;Password=123456";

        static SqlConnection _connection = null;
        static SqlCommand _cmd = null;
        static SqlDataReader _reader = null;
        
        static int GetCountryID(string CountryName)
        {
            int ID = -1;

            _connection = new SqlConnection(_ConnectionString);

            _connection.Open();

            string QueryFindCountry = "select [CountryID] from [Countries] where [CountryName] = @CN;";

            _cmd = new SqlCommand(QueryFindCountry, _connection);
            _cmd.Parameters.AddWithValue("@CN", CountryName);

            object result = _cmd.ExecuteScalar();

            if (result != null)
            {
                ID = int.Parse(result.ToString());
            }
            else
            {
                if (_cmd != null)
                    _cmd.Dispose();

                QueryFindCountry = @"insert into [Countries] (CountryName, Code, PhoneCode) values (@CN, @C, @PHC);
                                        select Scope_Identity();";

                _cmd = new SqlCommand(QueryFindCountry, _connection);
                _cmd.Parameters.AddWithValue("@CN", CountryName);
                _cmd.Parameters.AddWithValue("@C", DBNull.Value);
                _cmd.Parameters.AddWithValue("@PHC", DBNull.Value);

                result = _cmd.ExecuteScalar();

                if (result != null)
                    ID = int.Parse(result.ToString());
            }

            if (_cmd != null)
                _cmd.Dispose();

            return ID;
        }

        static public bool AddNewContact(ClsContact Contact)
        {
            bool IsAdded = false;

            try
            {
                int CountryID = -1;

                if (!string.IsNullOrWhiteSpace(Contact.Country))
                {
                    CountryID = GetCountryID(Contact.Country);
                }


                if (CountryID != -1)
                {
                    string AddQuery = @"insert into [Contacts]
                                        ([FirstName], [LastName], [Email], [Phone], [Address], [DateOfBirth], [CountryID], [ImagePath])
                                        values (@FN, @LN, @EM, @PH, @AD, @DOB, @CID, @IP);";

                    _cmd = new SqlCommand(AddQuery, _connection);

                    _cmd.Parameters.AddWithValue("@FN", Contact.FirstName);
                    _cmd.Parameters.AddWithValue("@LN", Contact.LastName);
                    _cmd.Parameters.AddWithValue("@EM", Contact.Email);
                    _cmd.Parameters.AddWithValue("@PH", Contact.Phone);
                    _cmd.Parameters.AddWithValue("@AD", Contact.Address);
                    _cmd.Parameters.AddWithValue("@DOB", Contact.DateOfBirth);
                    _cmd.Parameters.AddWithValue("@CID", CountryID);

                    if (string.IsNullOrWhiteSpace(Contact.ImagePath))
                        _cmd.Parameters.AddWithValue("@IP", DBNull.Value);
                    else
                        _cmd.Parameters.AddWithValue("@IP", Contact.ImagePath);


                    byte NumberOfRowsAffected = (byte)_cmd.ExecuteNonQuery();

                    if (NumberOfRowsAffected > 0)
                        IsAdded = true;
                }
            }
            finally
            {
                if (_cmd != null)
                    _cmd.Dispose();

                if (_connection != null)
                {
                    _connection.Close();
                    _connection.Dispose();
                }
            }

            return IsAdded;
        }

        static string GetCountryName(int CountryID)
        {
            string FindCountryName = "select [CountryName] from [Countries] where [CountryID] = @CID;";

            _cmd = new SqlCommand(FindCountryName, _connection);

            _cmd.Parameters.AddWithValue("@CID", CountryID);

            object result = _cmd.ExecuteScalar();

            if (result != null)
                return result.ToString();
            else
                return null;

        }

        static public ClsContact FindContact(string FirstName, string LastName, ref int ID)
        {
            ClsContact Contact = null;

            if (!string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName))
            {
                try
                {
                    _connection = new SqlConnection(_ConnectionString);

                    _connection.Open();

                    string FindQuery = "select * from [Contacts] where [FirstName] = @FN and [LastName] = @LN;";

                    _cmd = new SqlCommand(FindQuery, _connection);

                    _cmd.Parameters.AddWithValue("@FN", FirstName);
                    _cmd.Parameters.AddWithValue("@LN", LastName);

                    _reader = _cmd.ExecuteReader();

                    int CountryID = -1;

                    while (_reader.Read())
                    {
                        Contact = new ClsContact();

                        ID = (int)_reader["ContactID"];
                        Contact.FirstName = _reader["FirstName"] as string;
                        Contact.LastName = _reader["LastName"] as string;
                        Contact.Email = _reader["Email"] as string;
                        Contact.Phone = _reader["Phone"] as string;
                        Contact.Address = _reader["Address"] as string;

                        object DateOfBirth = _reader["DateOfBirth"];

                        if (DateTime.TryParse(DateOfBirth.ToString(), out DateTime result))
                        {
                            Contact.DateOfBirth = result;
                        }

                        CountryID = (int)_reader["CountryID"];

                        if (_reader["CountryID"] != DBNull.Value)
                            Contact.ImagePath = _reader["ImagePath"] as string;
                        else
                            Contact.ImagePath = "";
                    }

                    if (_reader != null)
                    {
                        _reader.Close();
                        _reader.Dispose();
                    }

                    if (_cmd != null)
                        _cmd.Dispose();

                    if (CountryID != -1)
                    {
                        Contact.Country = GetCountryName(CountryID);
                    }
                }
                finally
                {
                    if (_reader != null)
                    {
                        _reader.Close();
                        _reader.Dispose();
                    }

                    if (_cmd != null)
                        _cmd.Dispose();

                    if (_connection != null)
                    {
                        _connection.Close();
                        _connection.Dispose();
                    }
                }
            }

            return Contact;
        }

        static public bool UpdateContact(ClsContact Contact, int ID)
        {
            bool Updated = false;
            int CountryID = -1;

            if (ID > 0 && Contact.IsFull())
            {
                try
                {
                    CountryID = GetCountryID(Contact.Country);

                    string UpdateQuery = @"update [Contacts] set [FirstName] = @FN, [LastName] = @LN, [Email] = @EM, [Phone] = @PH, [Address] = @AD,
                                       [DateOfBirth] = @DOB, [CountryID] = @ContryID, [ImagePath] = @IP where [ContactID] = @ID;";

                    _cmd = new SqlCommand(UpdateQuery, _connection);

                    _cmd.Parameters.AddWithValue("@FN", Contact.FirstName);
                    _cmd.Parameters.AddWithValue("@LN", Contact.LastName);
                    _cmd.Parameters.AddWithValue("@EM", Contact.Email);
                    _cmd.Parameters.AddWithValue("@PH", Contact.Phone);
                    _cmd.Parameters.AddWithValue("@AD", Contact.Address);
                    _cmd.Parameters.AddWithValue("@DOB", Contact.DateOfBirth);
                    _cmd.Parameters.AddWithValue("@ContryID", CountryID);

                    if (string.IsNullOrWhiteSpace(Contact.ImagePath))
                        _cmd.Parameters.AddWithValue("@IP", DBNull.Value);
                    else
                        _cmd.Parameters.AddWithValue("@IP", Contact.ImagePath);


                    _cmd.Parameters.AddWithValue("@ID", ID);

                    byte NumberOfRowsAffected = (byte)_cmd.ExecuteNonQuery();

                    if (NumberOfRowsAffected > 0)
                        Updated = true;
                }
                finally
                {
                    if (_cmd != null)
                        _cmd.Dispose();

                    if (_connection != null)
                    {
                        _connection.Close();
                        _connection.Dispose();
                    }
                }
            }

            return Updated;
        }

        static public bool DeleteContact(int ID)
        {
            bool Delete = false;

            if (ID > 0)
            {
                try
                {
                    _connection = new SqlConnection(_ConnectionString);

                    _connection.Open();

                    string DeleteQuery = "delete from [Contacts] where [ContactID] = @CID;";

                    _cmd = new SqlCommand(DeleteQuery, _connection);

                    _cmd.Parameters.AddWithValue("@CID", ID);

                    byte NumberOfRowsAffected = (byte)_cmd.ExecuteNonQuery();

                    if (NumberOfRowsAffected > 0)
                        Delete = true;
                }
                finally
                {
                    if (_cmd != null)
                        _cmd.Dispose();

                    if (_connection != null)
                    {
                        _connection.Close();
                        _connection.Dispose();
                    }
                }
            }

            return Delete;
        }

        static void SetCountriesNames(DataTable dataTable)
        {
            dataTable.Columns.Add("CountryName", typeof(string));

            foreach (DataRow row in dataTable.Rows)
            {
                row["CountryName"] = GetCountryName((int)row["CountryID"]);
            }

            dataTable.Columns.Remove("CountryID");
        }

        static public DataTable GetAllContacts()
        {
            DataTable dataTable = new DataTable();

            try
            {
                _connection = new SqlConnection(_ConnectionString);

                _connection.Open();

                string GetAllContactsQuery = "select * from [Contacts];";

                _cmd = new SqlCommand(GetAllContactsQuery, _connection);

                _reader = _cmd.ExecuteReader();

                if (_reader.HasRows)
                {
                    dataTable.Load(_reader);

                    if (_reader != null)
                    {
                        _reader.Close();
                        _reader.Dispose();
                    }

                    if (_cmd != null)
                        _cmd.Dispose();

                    SetCountriesNames(dataTable);
                }
            }
            finally
            {
                if (_reader != null)
                {
                    _reader.Close();
                    _reader.Dispose();
                }

                if (_cmd != null)
                    _cmd.Dispose();

                if (_connection != null)
                {
                    _connection.Close();
                    _connection.Dispose();
                }
            }

            return dataTable;
        }

        static public bool IsContactExist(string FirstName, string LastName)
        {
            bool Exist = false;

            try
            {
                _connection = new SqlConnection(_ConnectionString);

                _connection.Open();

                string ExistQuery = "select [ContactID] from [Contacts] where [FirstName] = @FN and [LastName] = @LN;";

                _cmd = new SqlCommand(ExistQuery, _connection);

                _cmd.Parameters.AddWithValue("@FN", FirstName);
                _cmd.Parameters.AddWithValue("@LN", LastName);

                object result = _cmd.ExecuteScalar();

                if (result != null)
                    Exist = true;
            }
            finally
            {
                if (_cmd != null)
                    _cmd.Dispose();

                if (_connection != null)
                {
                    _connection.Close();
                    _connection.Dispose();
                }
            }

            return Exist;
        }

        static public ClsCountry FindCountry(string CountryName, ref int CountryID)
        {
            ClsCountry Country = null;

            if (!string.IsNullOrWhiteSpace(CountryName))
            {
                try
                {
                    _connection = new SqlConnection(_ConnectionString);

                    _connection.Open();

                    string FindQuery = "select * from [Countries] where [CountryName] = @CN;";

                    _cmd = new SqlCommand(FindQuery, _connection);

                    _cmd.Parameters.AddWithValue("@CN", CountryName);

                    _reader = _cmd.ExecuteReader();

                    if (_reader.Read())
                    {
                        Country = new ClsCountry();

                        CountryID = (int)_reader["CountryID"];
                        Country.CountryName = _reader["CountryName"] as string;

                        if (_reader["Code"] != DBNull.Value)
                            Country.Code = _reader["Code"] as string;
                        else
                            Country.Code = "";

                        if (_reader["PhoneCode"] != DBNull.Value)
                            Country.PhoneCode = _reader["PhoneCode"] as string;
                        else
                            Country.PhoneCode = "";
                    }
                }
                finally
                {
                    if (_reader != null)
                    {
                        _reader.Close();
                        _reader.Dispose();
                    }

                    if (_cmd != null)
                        _cmd.Dispose();

                    if (_connection != null)
                    {
                        _connection.Close();
                        _connection.Dispose();
                    }
                }
            }

            return Country;
        }

        static public bool IsCountryExist(string CountryName)
        {
            bool Exist = false;

            try
            {
                _connection = new SqlConnection(_ConnectionString);

                _connection.Open();

                string ExistQuery = "select [CountryID] from [Countries] where [CountryName] = @CN;";

                _cmd = new SqlCommand(ExistQuery, _connection);

                _cmd.Parameters.AddWithValue("@CN", CountryName);

                object result = _cmd.ExecuteScalar();

                if (result != null)
                    Exist = true;
            }
            finally
            {
                if (_cmd != null)
                    _cmd.Dispose();

                if (_connection != null)
                {
                    _connection.Close();
                    _connection.Dispose();
                }
            }

            return Exist;
        }

        static public bool AddNewCountry(ClsCountry Country)
        {
            bool IsAdded = false;

            if (Country.IsFull())
            {
                try
                {
                    _connection = new SqlConnection(_ConnectionString);

                    _connection.Open();

                    string AddQuery = @"insert into [Countries] (CountryName, PhoneCode, Code) values (@CN, @PHC, @C);";

                    _cmd = new SqlCommand(AddQuery, _connection);

                    _cmd.Parameters.AddWithValue("@CN", Country.CountryName);

                    if (string.IsNullOrWhiteSpace(Country.PhoneCode))
                        _cmd.Parameters.AddWithValue("@PHC", DBNull.Value);
                    else
                        _cmd.Parameters.AddWithValue("@PHC", Country.PhoneCode);

                    if (string.IsNullOrWhiteSpace(Country.Code))
                        _cmd.Parameters.AddWithValue("@C", DBNull.Value);
                    else
                        _cmd.Parameters.AddWithValue("@C", Country.Code);


                    byte NumberOfRowsAffected = (byte)_cmd.ExecuteNonQuery();

                    if (NumberOfRowsAffected > 0)
                        IsAdded = true;
                }
                finally
                {
                    if (_cmd != null)
                        _cmd.Dispose();

                    if (_connection != null)
                    {
                        _connection.Close();
                        _connection.Dispose();
                    }
                }
            }

            return IsAdded;
        }

        static public DataTable GetAllCountries()
        {
            DataTable dataTable = new DataTable();

            try
            {
                _connection = new SqlConnection(_ConnectionString);

                _connection.Open();

                string GetAllContactsQuery = "select * from [Countries];";

                _cmd = new SqlCommand(GetAllContactsQuery, _connection);

                _reader = _cmd.ExecuteReader();

                if (_reader.HasRows)
                {
                    dataTable.Load(_reader);
                }
            }
            finally
            {
                if (_reader != null)
                {
                    _reader.Close();
                    _reader.Dispose();
                }

                if (_cmd != null)
                    _cmd.Dispose();

                if (_connection != null)
                {
                    _connection.Close();
                    _connection.Dispose();
                }
            }

            return dataTable;
        }

        static public bool UpdateCountry(ClsCountry Country, int CountryID)
        {
            bool Updated = false;

            if (CountryID > 0 && Country.IsFull())
            {
                try
                {
                    _connection = new SqlConnection(_ConnectionString);

                    _connection.Open();

                    string UpdateQuery = @"update [Countries] set [CountryName] = @CN, [PhoneCode] = @PHC, [Code] = @C where [CountryID] = @ID;";

                    _cmd = new SqlCommand(UpdateQuery, _connection);

                    _cmd.Parameters.AddWithValue("@CN", Country.CountryName);


                    if (string.IsNullOrWhiteSpace(Country.PhoneCode))
                        _cmd.Parameters.AddWithValue("@PHC", DBNull.Value);
                    else
                        _cmd.Parameters.AddWithValue("@PHC", Country.PhoneCode);


                    if (string.IsNullOrWhiteSpace(Country.Code))
                        _cmd.Parameters.AddWithValue("@C", DBNull.Value);
                    else
                        _cmd.Parameters.AddWithValue("@C", Country.Code);


                    _cmd.Parameters.AddWithValue("@ID", CountryID);

                    byte NumberOfRowsAffected = (byte)_cmd.ExecuteNonQuery();

                    if (NumberOfRowsAffected > 0)
                        Updated = true;
                }
                finally
                {
                    if (_cmd != null)
                        _cmd.Dispose();

                    if (_connection != null)
                    {
                        _connection.Close();
                        _connection.Dispose();
                    }
                }
            }

            return Updated;
        }




    }
}
