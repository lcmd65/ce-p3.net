using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Common;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Common.SQL
{
    public class Program_SQL
    {
        public void connect_sql(ExcelWorksheet worksheet, string textip, string textport,string sqlconnectStr)
        {
            if (sqlconnectStr == null)
            {
                sqlconnectStr = "Data Source=" + textip + "," + textport + ";Initial Catalog=TutorialDB;User ID=sa;Password=Password.1";
            }
            SqlConnection connection = new SqlConnection(sqlconnectStr);
            connection.Open();                 // Mở kết nối - hoặc  connection.OpenAsync(); nếu dùng async
            for (int i = 6; i < 377; i++) 
            {
                for (int j = 3; j <= 8; j++)
                {
                    if (worksheet.Cells[i, j].Value == null)
                    {
                        worksheet.Cells[i, j].Value = 0;
                    }
                }
                using (var Cm = new SqlCommand("", connection))
                {
                    Cm.CommandText = "INSERT INTO LASER_P3(CATALOG_NAME, PARA_NAME , POR_VALUE , PRIORITY_VALUE, TOOL_VALUE , CE) VALUES (@CATALOG_NAME, @PARA_NAME , @POR_VALUE ,@PRIORITY_VALUE, @TOOL_VALUE , @CE)";
                    Cm.Parameters.AddWithValue("@CATALOG_NAME", worksheet.Cells[i, 3].Value.ToString());
                    Cm.Parameters.AddWithValue("@PARA_NAME", worksheet.Cells[i, 4].Value.ToString());
                    Cm.Parameters.AddWithValue("@POR_VALUE", worksheet.Cells[i, 5].Value.ToString()); 
                    Cm.Parameters.AddWithValue("@PRIORITY_VALUE", worksheet.Cells[i, 6].Value.ToString());
                    Cm.Parameters.AddWithValue("@TOOL_VALUE", worksheet.Cells[i, 7].Value.ToString());
                    Cm.Parameters.AddWithValue("@CE", worksheet.Cells[i, 8].Value.ToString());
                    Cm.ExecuteNonQuery();
                }
            
            }
            connection.Close();

            //..                                    // thực hiện cá tác  vụ truy vấn CSDL (CRUD - Insert, Select, Update, Delete
        }
    }
}
