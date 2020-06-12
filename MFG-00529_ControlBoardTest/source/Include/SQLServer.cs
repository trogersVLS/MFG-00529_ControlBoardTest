using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Text;
using System.Data.OleDb;
using System.Configuration;
using System.Data;
using System;
using System.Threading.Tasks;

namespace ControlBoardTest
{
    public class SQLServer
    {
        public static bool PingServer(string ConnectionString)
        {
            int value = 1;
            OleDbConnection conn = new OleDbConnection(ConnectionString);
            OleDbCommand command = new OleDbCommand(String.Format("SELECT {0};",value), conn);

            try
            {
                conn.Open();
                int ping = (int)command.ExecuteScalar();
                if (ping == value)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                conn.Close();
            }

        }


        public static async Task<int> InsertOneRow(string ConnectionString, string TableName, Dictionary<string, string> data)
        {
            //Create the necessary INSERT string
            StringBuilder commandStr = new StringBuilder();
            commandStr.Append("INSERT INTO " + TableName + " ");

            StringBuilder Columns = new StringBuilder("(");
            StringBuilder Values = new StringBuilder("VALUES (");
            foreach (KeyValuePair<string, string> kv in data)
            {
                Columns.Append("[" + kv.Key + "],");
                Values.Append("\'" + kv.Value + "\',");
            }
            Columns.Length--;
            Values.Length--;

            Columns.Append(") ");
            Values.Append(");");

            //Values.Append(");");

            commandStr.Append(Columns.ToString());
            commandStr.Append(" OUTPUT INSERTED.[test-id] ");
            commandStr.Append(Values.ToString());


            OleDbConnection conn = new OleDbConnection();
            OleDbCommand cmd = new OleDbCommand(commandStr.ToString(), conn);
            int rowId = 0;
            try
            {
                conn.ConnectionString = ConnectionString;
                conn.Open();
                Task<object> insertingRow =  cmd.ExecuteScalarAsync();

                object tmp = (await insertingRow);
                rowId = int.Parse(tmp.ToString());
                
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Violation of PRIMARY KEY constraint"))
                { //Data exists in database already, ignore and don't update. Else, throw exception
                    
                }
                else if(e.Message.Contains("Client with IP address"))
                {
                    //Ip address is not allowed access to the server. Need to warn user to contact a database administrator to grant the IP address access.

                    Exception ex = new Exception("Unable to access database. Please contact the database administrator so that they can add your faciliy's IP address to the list of acceptable IP addresses for this tool.");
                    throw ex;
                }
                else
                {
                    throw e;
                }

            }
            finally
            {
                conn.Close();
            }

            return rowId;
        }
        public static async Task<int> UpdateResult(string ConnectionString, string TableName, string TestId, Dictionary<string, string> DataToChange)
        {
            //Create the necessary INSERT string
            StringBuilder commandStr = new StringBuilder();
            commandStr.Append("UPDATE " + TableName + " ");

            StringBuilder KeyValues = new StringBuilder("SET ");
            
            foreach (KeyValuePair<string, string> kv in DataToChange)
            {
                KeyValues.Append(kv.Key + " = \'" + kv.Value + "\',");
            }
            KeyValues.Length--;

            KeyValues.Append(" WHERE [test-id] = \'" + TestId + "\';");

            commandStr.Append(KeyValues.ToString());

            OleDbConnection conn = new OleDbConnection(ConnectionString);
            OleDbCommand cmd = new OleDbCommand(commandStr.ToString(), conn);
            int rowsAffected = 0;
            try
            {
                conn.Open();
                //rowsAffected = cmd.ExecuteNonQuery();

                Task<int> updating = cmd.ExecuteNonQueryAsync();

                rowsAffected = await updating;

                if (rowsAffected != 1)
                {
                    throw new InvalidOperationException("The number of rows affected in SQL insert do not match");
                }
            }
            catch (Exception e)
            {
                if (!e.Message.Contains("Violation of PRIMARY KEY constraint"))
                { //Data exists in database already, ignore and don't update
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }

            return rowsAffected;
        }

        public static async Task<DataTable> GetRows(string ConnectionString, string Query)
        {
            OleDbConnection conn = new OleDbConnection(ConnectionString);
            OleDbCommand cmd = new OleDbCommand(Query, conn);
            DataTable dtRecord = new DataTable();
            try
            {
                conn.Open();
                OleDbDataAdapter sqlDataAdap = new OleDbDataAdapter(cmd);
                sqlDataAdap.Fill(dtRecord);
                sqlDataAdap.Dispose();
                conn.Close();
            }
            catch (Exception e)
            {
                throw e;
            }


            return dtRecord;
        }

        public static async Task<int> TransferData(string FromConnectionString, string FromTable, string ToConnectionString, string ToTable)
        {
            string getQuery = "SELECT * FROM " + FromTable;
            Task<DataTable> gettingRows = GetRows(FromConnectionString, getQuery);

            DataTable data = await gettingRows;

            int cnt = 0;

            try
            {
                foreach (DataRow row in data.Rows)
                {
                    Dictionary<string, string> dict = new Dictionary<string, string>();

                    foreach (DataColumn c in data.Columns)
                    {
                        dict.Add(c.ColumnName, row[c.ColumnName].ToString());
                    }
                    Task<int> rowsUpdated = InsertOneRow(ToConnectionString, ToTable, dict);
                    cnt += await rowsUpdated;
                }
            }
            catch (Exception e) { throw e; } //Error occurred, most likely because server is unavailable

            //Clear the local database.
            OleDbConnection from = new OleDbConnection(FromConnectionString);
            OleDbCommand delete = new OleDbCommand("DELETE FROM " + FromTable + " ;", from);
            try
            {
                from.Open();
                delete.ExecuteNonQuery();
            }
            catch { }
            finally
            {
                from.Close();
            }
            return cnt;
        }

        //public static string DataUpload()
        //{
        //    StringBuilder errorMessages = new StringBuilder();
        //    string currentrecord;
        //    int result;

        //    try
        //    {


        //        string str = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source= " + ConfigurationManager.AppSettings["LocalDB"] + ";";

        //        string sqlLocal = "Select * from Local_MainChassis order by StartTime";

        //        //query from database
        //        SqlConnectionStringBuilder sqlconn = new SqlConnectionStringBuilder
        //        {
        //            DataSource = ConfigurationManager.AppSettings["db_path"],
        //            PersistSecurityInfo = false,
        //            InitialCatalog = "Production_Test_Data"
        //        };


        //        if (false)
        //        {
        //            sqlconn.UserID = ConfigurationManager.AppSettings["db_user"];
        //            sqlconn.Password = ConfigurationManager.AppSettings["db_pass"];
        //            sqlconn.IntegratedSecurity = false;
        //        }
        //        else
        //        {
        //            //sqlconn.UserID = ConfigurationManager.AppSettings["db_user"];
        //            //sqlconn.Password = ConfigurationManager.AppSettings["db_pass"];
        //            sqlconn.IntegratedSecurity = true;
        //        }
        //        SqlConnection connRemote = new SqlConnection(sqlconn.ConnectionString);



        //        using (OleDbConnection connLocal = new OleDbConnection(str))    //make connection to local Access database
        //        {
        //            connLocal.Open();
        //            using (OleDbCommand commandLocal = new OleDbCommand(sqlLocal, connLocal))    //create a command and set its connection to local Access database
        //            {
        //                using (OleDbDataReader dataReader = commandLocal.ExecuteReader())
        //                {

        //                    string[] records = new string[dataReader.FieldCount];
        //                    connRemote.Open();

        //                    while (dataReader.Read())  //increments through number of rows
        //                    {
        //                        string query = "INSERT INTO dbo.Production_Test_MainChassis (Serial," +
        //                                                                        "TestName," +
        //                                                                        "StartTime," +
        //                                                                        "StopTime," +
        //                                                                        "OperatorID," +
        //                                                                        "StationID," +
        //                                                                        "NC," +
        //                                                                        "OverallPassFail," +
        //                                                                        "TestSpec," +
        //                                                                        "TestSoftwareRevision," +
        //                                                                        "PartNumber," +
        //                                                                        "BondLeakMeas," +
        //                                                                        "BondLeakLL," +
        //                                                                        "BondLeakHL," +
        //                                                                        "BondLeakResult," +
        //                                                                        "NebPressMeas," +
        //                                                                        "NebPressLL," +
        //                                                                        "NebPressHL," +
        //                                                                        "NebPressResult," +
        //                                                                        "NebLeakMeas," +
        //                                                                        "NebLeakLL," +
        //                                                                        "NebLeakHL," +
        //                                                                        "NebLeakResult," +
        //                                                                        "O2FlowMeas," +
        //                                                                        "O2FlowLL," +
        //                                                                        "O2FlowHL," +
        //                                                                        "O2FlowResult)" +
        //                                                                "VALUES (@Serial," +
        //                                                                        "@TestName," +
        //                                                                        "@StartTime," +
        //                                                                        "@StopTime," +
        //                                                                        "@OperatorID," +
        //                                                                        "@StationID," +
        //                                                                        "@NC," +
        //                                                                        "@OverallPassFail," +
        //                                                                        "@TestSpec," +
        //                                                                        "@TestSoftwareRevision," +
        //                                                                        "@PartNumber," +
        //                                                                        "@BondLeakMeas," +
        //                                                                        "@BondLeakLL," +
        //                                                                        "@BondLeakHL," +
        //                                                                        "@BondLeakResult," +
        //                                                                        "@NebPressMeas," +
        //                                                                        "@NebPressLL," +
        //                                                                        "@NebPressHL," +
        //                                                                        "@NebPressResult," +
        //                                                                        "@NebLeakMeas," +
        //                                                                        "@NebLeakLL," +
        //                                                                        "@NebLeakHL," +
        //                                                                        "@NebLeakResult," +
        //                                                                        "@O2FlowMeas," +
        //                                                                        "@O2FlowLL," +
        //                                                                        "@O2FlowHL," +
        //                                                                        "@O2FlowResult);";

        //                        using (SqlCommand commandRemote = new SqlCommand(query, connRemote))
        //                        {
        //                            commandRemote.Parameters.Add("@Serial", System.Data.SqlDbType.NVarChar).Value = dataReader["Serial"];
        //                            commandRemote.Parameters.Add("@TestName", System.Data.SqlDbType.NVarChar).Value = dataReader["TestName"];
        //                            commandRemote.Parameters.Add("@StartTime", System.Data.SqlDbType.DateTime).Value = dataReader["StartTime"];
        //                            commandRemote.Parameters.Add("@StopTime", System.Data.SqlDbType.DateTime).Value = dataReader["StopTime"];
        //                            commandRemote.Parameters.Add("@OperatorID", System.Data.SqlDbType.NVarChar).Value = dataReader["OperatorID"];
        //                            commandRemote.Parameters.Add("@StationID", System.Data.SqlDbType.NVarChar).Value = dataReader["StationID"];
        //                            commandRemote.Parameters.Add("@NC", System.Data.SqlDbType.NVarChar).Value = dataReader["NC"];
        //                            commandRemote.Parameters.Add("@OverallPassFail", System.Data.SqlDbType.NVarChar).Value = dataReader["OverallPassFail"];
        //                            commandRemote.Parameters.Add("@TestSpec", System.Data.SqlDbType.NVarChar).Value = dataReader["TestSpec"];
        //                            commandRemote.Parameters.Add("@TestSoftwareRevision", System.Data.SqlDbType.NVarChar).Value = dataReader["TestSoftwareRevision"];
        //                            commandRemote.Parameters.Add("@PartNumber", System.Data.SqlDbType.NVarChar).Value = dataReader["PartNumber"];
        //                            commandRemote.Parameters.Add("@BondLeakMeas", System.Data.SqlDbType.NVarChar).Value = dataReader["BondLeakMeas"];
        //                            commandRemote.Parameters.Add("@BondLeakLL", System.Data.SqlDbType.NVarChar).Value = dataReader["BondLeakLL"];
        //                            commandRemote.Parameters.Add("@BondLeakHL", System.Data.SqlDbType.NVarChar).Value = dataReader["BondLeakHL"];
        //                            commandRemote.Parameters.Add("@BondLeakResult", System.Data.SqlDbType.NVarChar).Value = dataReader["BondLeakResult"];
        //                            commandRemote.Parameters.Add("@NebPressMeas", System.Data.SqlDbType.NVarChar).Value = dataReader["NebPressMeas"];
        //                            commandRemote.Parameters.Add("@NebPressLL", System.Data.SqlDbType.NVarChar).Value = dataReader["NebPressLL"];
        //                            commandRemote.Parameters.Add("@NebPressHL", System.Data.SqlDbType.NVarChar).Value = dataReader["NebPressHL"];
        //                            commandRemote.Parameters.Add("@NebPressResult", System.Data.SqlDbType.NVarChar).Value = dataReader["NebPressResult"];
        //                            commandRemote.Parameters.Add("@NebLeakMeas", System.Data.SqlDbType.NVarChar).Value = dataReader["NebLeakMeas"];
        //                            commandRemote.Parameters.Add("@NebLeakLL", System.Data.SqlDbType.NVarChar).Value = dataReader["NebLeakLL"];
        //                            commandRemote.Parameters.Add("@NebLeakHL", System.Data.SqlDbType.NVarChar).Value = dataReader["NebLeakHL"];
        //                            commandRemote.Parameters.Add("@NebLeakResult", System.Data.SqlDbType.NVarChar).Value = dataReader["NebLeakResult"];
        //                            commandRemote.Parameters.Add("@O2FlowMeas", System.Data.SqlDbType.NVarChar).Value = dataReader["O2FlowMeas"];
        //                            commandRemote.Parameters.Add("@O2FlowLL", System.Data.SqlDbType.NVarChar).Value = dataReader["O2FlowLL"];
        //                            commandRemote.Parameters.Add("@O2FlowHL", System.Data.SqlDbType.NVarChar).Value = dataReader["O2FlowHL"];
        //                            commandRemote.Parameters.Add("@O2FlowResult", System.Data.SqlDbType.NVarChar).Value = dataReader["O2FlowResult"];

        //                            result = commandRemote.ExecuteNonQuery();

        //                            currentrecord = dataReader["StartTime"].ToString();



        //                            // Check Error
        //                            if (result != 1)
        //                            {
        //                                MessageBox.Show("Error inserting data into Database!");
        //                            }


        //                        }


        //                    }

        //                    dataReader.Close();
        //                    sqlLocal = "Delete from Local_MainChassis ;";


        //                    commandLocal.CommandText = sqlLocal;
        //                    commandLocal.ExecuteNonQuery();
        //                }
        //            }

        //            connRemote.Close();
        //            connLocal.Close();


        //        }

        //        str = "Data Upload Complete";
        //        return str;
        //    }


        //    catch (SqlException ex)
        //    {
        //        for (int i = 0; i < ex.Errors.Count; i++)
        //        {
        //            errorMessages.Append("Message: " + ex.Errors[i].Message + "\n" +
        //                "Error Number: " + ex.Errors[i].Number + "\n" +
        //                "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
        //                "Source: " + ex.Errors[i].Source + "\n\n");


        //        }

        //        MessageBox.Show(errorMessages.ToString());
        //        return null;

        //    }

        //}
    }
}
