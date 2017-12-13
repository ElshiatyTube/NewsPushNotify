using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace WebSerNews
{
    /// <summary>
    /// Summary description for wsnew
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class wsnew : System.Web.Services.WebService
    {

        [WebMethod(MessageName = "SendNews", Description = "this method send new from android app to table in database")]
        [System.Xml.Serialization.XmlInclude(typeof(ReturnData))]

        public ReturnData SendNews( string txtNews)
        {
            int IsAdded = 1;
            string Message = "";

            try
            {
                using (SqlConnection connection = new SqlConnection(DBconnection.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO News (txtNews) VALUES (@txtNews)");
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                  //  cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@txtNews", txtNews);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();

                }
                Message = "Your News was Send to WS & Online Database";
            }
            catch(Exception ex)
            {
                IsAdded = 0;
                Message = "Cannot add your inforamtion" + ex.Message;// "Cannot add your inforamtion";
            }
            ReturnData rt = new ReturnData();
            rt.ID = IsAdded;
            rt.txtNews = txtNews;
            rt.Message = Message;

            return rt;
        }


        [WebMethod(MessageName = "GetJsontxtNews", Description = "Return txtNews data from DB Json Formatt")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]

        public string GetJsontxtNews()
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            SqlConnection conn = new SqlConnection(DBconnection.ConnectionString);

            //  SqlConnection coont=new SqlConnection(new dbConnectionClass().)

            //     SqlConnection connection = new SqlConnection(DBConnection.ConnectionString)

            // here I have to creare list of contact calss
         //   string Message2 = "";
            List<ReturnData> contactslist = new List<ReturnData>();
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                    SqlDataReader dr;

                    SqlCommand cmd = new SqlCommand("select ID, txtNews from News ", conn);
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        var contact = new ReturnData
                        {
                            //id = Int32.Parse(dr["id"].ToString()),
                            //name = dr["name"].ToString(),
                            //email = dr["email"].ToString(),
                            //mobileNumber = dr["mobile"].ToString()
                          ID = Int32.Parse(dr["ID"].ToString()),
                            txtNews = dr["txtNews"].ToString()
                           
                        };
                        contactslist.Add(contact);
                    }
                    dr.Close();

                }
              //  Message2 = "";
            }
            catch (SqlException ex)
            {

            }
            finally
            {
                conn.Close();
            }
            return ser.Serialize(contactslist);

        }


        [WebMethod(MessageName = "NewsNotify", Description = "Login Notify new News from users")]
        [System.Xml.Serialization.XmlInclude(typeof(ReturnData))]

        public ReturnData NewsNotify(int ID)  /// get list of notes
        {

            string Message = "";
            try
            {
                SqlDataReader reader;
                using (SqlConnection connection = new SqlConnection(DBconnection.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SELECT txtNews,ID FROM News where ID>@ID and ID=(SELECT MAX(ID) FROM[News])");
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    connection.Open();

                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Message = reader.GetString(0);
                        ID = reader.GetInt32(1);
                    }
                    if (Message.Length == 0)
                    {
                        ID = 0;
                        Message = "NO NEW News";
                    }
                    reader.Close();

                    connection.Close();
                }

            }
            catch
            {
                Message = "cannot access to the data";
            }
            ReturnData rt = new ReturnData();
            rt.Message = Message;
            rt.ID = ID;

            return rt;

        }

    }
}
