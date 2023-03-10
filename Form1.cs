using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Mail;
using Common.SQL;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using DataTable = System.Data.DataTable;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;
using System.Runtime.CompilerServices;
using OfficeOpenXml.ThreadedComments;
using System.Timers;
using System.Diagnostics;
using System.Net.NetworkInformation;
using static OfficeOpenXml.ExcelErrorValue;
using OfficeOpenXml.Drawing.Chart;

namespace CE_Laser_App
{
    public partial class Form1 : Form
    {
        static System.Windows.Forms.Timer aTimer = new System.Windows.Forms.Timer();
        static bool exitFlag = false;
        public XDocument xdoc1; // XML public variable
        public XDocument xdoc2; // XML recipe variable
        public bool check_color; // temp variable for checking color cell
        public string Path_file; // varible file path
        public string sqlconnectStr;
        public Form1()
        {
            InitializeComponent();
        } // Form 1 init

        // Read CE laser Excel File
        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog(); //open dialog to choose file
            if (file.ShowDialog() == DialogResult.OK) //if there is a file chosen by the user
            {
                string fileExt = Path.GetExtension(file.FileName); //get the file extension
                Path_file = Path.GetFullPath(file.FileName);
                if (fileExt.CompareTo(".xls") == 0 || fileExt.CompareTo(".xlsx") == 0)
                {

                    FileInfo fi = new FileInfo(Path.GetFullPath(file.FileName));
                    using (ExcelPackage excelPackage = new ExcelPackage(fi))
                    {
                        //Processing
                        ExcelWorkbook workBook = excelPackage.Workbook;
                        ExcelWorksheet worksheet = workBook.Worksheets["5M+E Tool Parameter P3"];

                        // xml 
                        if (xdoc1 == null)
                        {
                            OpenFileDialog file_xml = new OpenFileDialog(); //open dialog to choose file
                            if (file_xml.ShowDialog() == DialogResult.OK) //if there is a file chosen by the user
                            {
                                string fileExt_xml = Path.GetExtension(file_xml.FileName); //get the file extension
                                string filePath_temp_xml = Path.GetFullPath(file_xml.FileName);
                                if (fileExt_xml.CompareTo(".xml") == 0 || fileExt_xml.CompareTo(".mpara") == 0)
                                {

                                    StringBuilder result = new StringBuilder();
                                    //Load xml
                                    xdoc1 = XDocument.Load(filePath_temp_xml); // getting XML file with name xdoc 
                                }
                            }
                        }
                        if (xdoc2 == null)
                        {
                            OpenFileDialog file_xml2 = new OpenFileDialog(); //open dialog to choose file
                            if (file_xml2.ShowDialog() == DialogResult.OK) //if there is a file chosen by the user
                            {
                                string fileExt_xml2 = Path.GetExtension(file_xml2.FileName); //get the file extension
                                string filePath_temp_xml2 = Path.GetFullPath(file_xml2.FileName);
                                if (fileExt_xml2.CompareTo(".xml") == 0 || fileExt_xml2.CompareTo(".mreci") == 0)
                                {
                                    StringBuilder result = new StringBuilder();
                                    //Load xml
                                    xdoc2 = XDocument.Load(filePath_temp_xml2); // getting XML file with name xdoc 
                                }
                            }
                        }
                        Trigger_xml(worksheet,xdoc1,xdoc2);
                        DataTable dt = new DataTable();
                        dataGridView1.RowCount = 377;
                        dataGridView1.ColumnCount = 20;
                        int rows = 377;
                        int cols = 20;
                        for (int i = 1; i <= rows; i++)
                        {
                            // Read new line
                            for (int j = 1; j <= cols; j++)
                            {
                                // Write to cell
                                if (worksheet.Cells[i, j].Value != null)
                                {
                                    dataGridView1.Rows[i - 1].Cells[j - 1].Value = worksheet.Cells[i, j].Value.ToString();
                                }
                            }
                        }
                        for (int i = 6; i <= 377; i++) // For loop to Highlight in excel CE file
                        {
                            // Making temp variable for getting Value2 object
                            // Type of data to Range
                            var temp_next = worksheet.Cells[i, 6].Value;
                            if (temp_next != null)
                            {
                                if (temp_next.ToString() == "R")
                                {
                                    // Init value POR and toolactual to compare
                                    object POR_value = dataGridView1.Rows[i - 1].Cells[4].Value;
                                    object tool_value = dataGridView1.Rows[i - 1].Cells[6].Value;
                                    if (POR_value != null && tool_value != null)
                                    {
                                        compare_Value(POR_value, tool_value);
                                        if (check_color == true)// Adjustment actual value
                                        {
                                            // Current Cell Red color make 
                                            worksheet.Cells[i, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                            worksheet.Cells[i, 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                                            dataGridView1.Rows[i - 1].Cells[6].Style.BackColor = Color.LightGreen;
                                        }
                                        else if (check_color == false)
                                        {
                                            worksheet.Cells[i, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                            worksheet.Cells[i, 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                                            dataGridView1.Rows[i - 1].Cells[6].Style.BackColor = Color.Red;
                                        }
                                    }
                                }
                                else
                                {
                                    // Current Cell None color  make
                                    worksheet.Cells[i, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[i, 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                                    dataGridView1.Rows[i - 1].Cells[6].Style.BackColor = Color.LightGreen;
                                }
                            }
                        }
                        //Save CE file
                        excelPackage.Save();
                    }
                }
                else
                {
                    MessageBox.Show("Please choose .xls or .xlsx file only.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error); //custom messageBox to show error
                }
            }
        }

        // Compare function 
        void compare_Value(object POR_value, object tool_value)
        {
            if (POR_value != null && tool_value != null)
            {

                string a = (string)POR_value.ToString();
                string b = (string)tool_value.ToString();
                if (POR_value.Equals(tool_value) == true)
                {
                    check_color = true;
                }
                else if (a.ToString() == "Uncheck" || ((a.ToString()== "Check" || a.ToString() == "check" ) && b != "0") || a.ToString() == "uncheck") 
                {
                    check_color = true;
                }
                else if ((a.ToString() =="On" && b.ToString() == "1")|| (a.ToString() == "Off" && b.ToString() == "0"))
                {
                    check_color = true;
                }
                else if((a.ToString()== b.ToString()+"s") || (a.ToString() == b.ToString() + "mm"))
                {
                    check_color = true;
                }
                else if (a.Contains(b) == true && (a.Contains(b +".")== true|| a.Contains(b+" ")==true) )
                {
                    check_color = true;
                }
                else if (a.ToLower() == b.ToLower())
                {
                    check_color = true;
                }
                else
                {
                    check_color = false;
                }
            }
        }

        // Read and processing CE file and XML file function 
        void Trigger_xml(ExcelWorksheet worksheet, XDocument xdoc1, XDocument xdoc2)
        {
            try
            {
                for (int i = 6; i <= 377; i++)
                {
                    if (worksheet.Cells[i, 15].Value != null)
                    {
                        // Querry from xdoc1 
                        IEnumerable<XElement> address_container =
                            from el in xdoc1.Root.Elements("step")// Select one step elemment group
                            where (string)el.Attribute("name") == worksheet.Cells[i, 15].Value.ToString() // Reference Element name from excel column define name 
                            select el;
                        // Querry form xdoc2
                        IEnumerable<XElement> address_container_2 =
                            from el in xdoc2.Root.Elements("step")// Select one step elemment group
                            where (string)el.Attribute("name") == worksheet.Cells[i, 15].Value.ToString() // Reference Element name from excel column define name 
                            select el;
                        if (address_container != null)
                        {
                            IEnumerable<XElement> address_element_temp =
                                     (from el2 in address_container.Elements()// Select one step elemment group
                                      where (string)el2.Attribute("key") == "10" // Reference Element name from excel column define name 
                                      select el2); // Return null if row = 0
                            var address_element = address_element_temp.FirstOrDefault();
                            if (address_element != null) // Return value in <prop key ="10...>
                            {
                                string temp = (address_element).Value.ToString();
                                worksheet.Cells[i, 7].Value = temp;
                            }
                            else
                            {
                                try
                                {
                                    var temp = address_element_temp.ElementAt(0);
                                    worksheet.Cells[i, 7].Value = temp.Value.ToString();
                                }
                                catch (Exception) { }
                            }
                        }
                        if (address_container_2 != null)
                        {
                            IEnumerable<XElement> address_element_temp =
                                (from el2 in address_container_2.Elements()// Select one step elemment group
                                 where (string)el2.Attribute("key") == "10" // Reference Element name from excel column define name 
                                 select el2); // Return null if row = 0
                            var address_element = address_element_temp.FirstOrDefault();
                            if (address_element != null) // Return value in <prop key ="10...>
                            {
                                string temp = (address_element).Value.ToString();
                                worksheet.Cells[i, 7].Value = temp;
                            }
                            else
                            {
                                try
                                {
                                    var temp = address_element_temp.ElementAt(0);
                                    worksheet.Cells[i, 7].Value = temp.Value.ToString();
                                }
                                catch (Exception) { }
                            }
                        }
                    }
                    if (worksheet.Cells[i, 20].Value != null)
                    {
                        IEnumerable<XElement> address_container =
                            from el in xdoc1.Root.Elements("step")// Select one step elemment group
                            where (string)el.Attribute("guid") == worksheet.Cells[i, 20].Value.ToString() // Reference Element name from excel column define name 
                            select el;
                        if (address_container != null) // Get element with key = 10 in List
                        {
                            IEnumerable<XElement> address_element_temp =
                                    (from el2 in address_container.Elements()// Select one step elemment group
                                     where (string)el2.Attribute("key") == "10" // Reference Element name from excel column define name 
                                     select el2); // Return null if row = 0
                            var address_element = address_element_temp.FirstOrDefault();
                            if (address_element != null) // Return value in <prop key ="10...>
                            {
                                string temp = (address_element).Value.ToString();
                                worksheet.Cells[i, 7].Value = temp;
                            }
                            else
                            {
                                try
                                {
                                    var temp = address_element_temp.ElementAt(0);
                                    worksheet.Cells[i, 7].Value = temp.Value.ToString();
                                }
                                catch (Exception) { }
                            }
                        }
                    }
                }
            }

            catch (Exception)
            {
                MessageBox.Show("Processing XML false");
            }
        }

        // Read XML lazer LOCAL program files to public variable
        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog(); //open dialog to choose file
            if (file.ShowDialog() == DialogResult.OK) //if there is a file chosen by the user
            {
                string fileExt = Path.GetExtension(file.FileName); //get the file extension
                string filePath_temp = Path.GetFullPath(file.FileName);
                if (fileExt.CompareTo(".xml") == 0 || fileExt.CompareTo(".mpara") == 0)
                {

                    StringBuilder result = new StringBuilder();
                    //Load xml
                    xdoc1 = XDocument.Load(filePath_temp); // getting XML file with name xdoc 
                }
            }
        }

        //Trigger data Lazer and lazer file with email sending and datagrid highlight
        private void button9_Click(object sender, EventArgs e)
        {
            Email email = new Email();
            email.SendEmail(textBox8.Text, "CE AUTO PROGRAM", "Auto Email From Laser PROGRAM", Path_file);
            MessageBox.Show("Send Mail");
        }

        // Read XMl rescipe program file to public variable
        private void button11_Click(object sender, EventArgs e)
        {

            OpenFileDialog file = new OpenFileDialog(); //open dialog to choose file
            if (file.ShowDialog() == DialogResult.OK) //if there is a file chosen by the user
            {
                string fileExt = Path.GetExtension(file.FileName); //get the file extension
                string filePath_temp = Path.GetFullPath(file.FileName);
                if (fileExt.CompareTo(".xml") == 0 || fileExt.CompareTo(".mreci") == 0)
                {

                    StringBuilder result = new StringBuilder();
                    //Load xml
                    xdoc2 = XDocument.Load(filePath_temp); // getting XML file with name xdoc 
                }
            }
        }

        /// /////////////////////////////////////////////////////////
        //Cancel
        private void Cancle_Click(object sender, EventArgs e)
        {
            aTimer.Stop();
            this.Close();
        }

        // Auto Click
        private void button8_Click(object sender, EventArgs e)
        {
            string file_temp_ce = System.IO.Path.Combine(textBox2.Text, "LASER_CE.xlsx");
            string file_temp_global = System.IO.Path.Combine(textBox2.Text, "P3C globals.xml");
            string file_temp_recipe = System.IO.Path.Combine(textBox2.Text, "S6 plus P3_EL V3 268.xml");
            xdoc1 = new XDocument();
            xdoc2 = new XDocument();
            xdoc1 = XDocument.Load(file_temp_global);
            xdoc2 = XDocument.Load(file_temp_recipe);
            using (ExcelPackage excelPackage = new ExcelPackage(file_temp_ce))
            {
                //Processing
                ExcelWorkbook workBook = excelPackage.Workbook;
                ExcelWorksheet worksheet = workBook.Worksheets["5M+E Tool Parameter P3"];

                // xml 
                if (xdoc1 == null)
                {
                    OpenFileDialog file_xml = new OpenFileDialog(); //open dialog to choose file
                    if (file_xml.ShowDialog() == DialogResult.OK) //if there is a file chosen by the user
                    {
                        string fileExt_xml = Path.GetExtension(file_xml.FileName); //get the file extension
                        string filePath_temp_xml = Path.GetFullPath(file_xml.FileName);
                        if (fileExt_xml.CompareTo(".xml") == 0 || fileExt_xml.CompareTo(".mpara") == 0)
                        {

                            StringBuilder result = new StringBuilder();
                            //Load xml
                            xdoc1 = XDocument.Load(filePath_temp_xml); // getting XML file with name xdoc 
                        }
                    }
                }

                if (xdoc2 == null)
                {
                    OpenFileDialog file_xml2 = new OpenFileDialog(); //open dialog to choose file
                    if (file_xml2.ShowDialog() == DialogResult.OK) //if there is a file chosen by the user
                    {
                        string fileExt_xml2 = Path.GetExtension(file_xml2.FileName); //get the file extension
                        string filePath_temp_xml2 = Path.GetFullPath(file_xml2.FileName);
                        if (fileExt_xml2.CompareTo(".xml") == 0 || fileExt_xml2.CompareTo(".mreci") == 0)
                        {
                            StringBuilder result = new StringBuilder();
                            //Load xml
                            xdoc2 = XDocument.Load(filePath_temp_xml2); // getting XML file with name xdoc 
                        }
                    }
                }
                Trigger_xml(worksheet, xdoc1, xdoc2);
                DataTable dt = new DataTable();
                dataGridView1.RowCount = 377;
                dataGridView1.ColumnCount = 20;
                int rows = 377;
                int cols = 20;
                for (int i = 1; i <= rows; i++)
                {
                    // Read new line
                    for (int j = 1; j <= cols; j++)
                    {
                        // Write to cell
                        if (worksheet.Cells[i, j].Value != null)
                        {
                            dataGridView1.Rows[i - 1].Cells[j - 1].Value = worksheet.Cells[i, j].Value.ToString();
                        }
                    }
                }
                for (int i = 6; i <= 377; i++)
                {
                    // Making temp variable for getting Value2 object
                    // Type of data to Range
                    var temp_next = worksheet.Cells[i, 6].Value;
                    if (temp_next != null)
                    {
                        if (temp_next.ToString() == "R")
                        {
                            // Init value POR and toolactual to compare
                            object POR_value = dataGridView1.Rows[i - 1].Cells[4].Value;
                            object tool_value = dataGridView1.Rows[i - 1].Cells[6].Value;
                            if (POR_value != null && tool_value != null)
                            {
                                compare_Value(POR_value, tool_value);
                                if (check_color == true)// Adjustment actual value
                                {
                                    // Current Cell Red color make 
                                    worksheet.Cells[i, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[i, 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                                    dataGridView1.Rows[i - 1].Cells[6].Style.BackColor = Color.LightGreen;
                                }
                                else if (check_color == false)
                                {
                                    worksheet.Cells[i, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[i, 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                                    dataGridView1.Rows[i - 1].Cells[6].Style.BackColor = Color.Red;
                                }
                            }
                        }
                        else
                        {
                            // Current Cell None color  make
                            worksheet.Cells[i, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[i, 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                            dataGridView1.Rows[i - 1].Cells[6].Style.BackColor = Color.LightGreen;
                        }
                    }
                }
                excelPackage.Save();
                excelPackage.SaveAs(@"C:\LASER_CE");
                Email email = new Email();
                email.SendEmail(textBox8.Text, "CE AUTO PROGRAM", "Auto Email From Laser PROGRAM", Path_file);
                MessageBox.Show("Send Mail");
            }
        }

        // Ping to host server/database Ip
        public static bool PingHost(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                MessageBox.Show("Can't ping");
            }

            return pingable;
        }

        // Trigger one click with root folder 
        private void trigger_daily()
        {
            {
                PingHost( textBox1.Text); 
                string file_temp_ce = System.IO.Path.Combine(textBox2.Text, "LASER_CE.xlsx");
                string file_temp_global = System.IO.Path.Combine(textBox2.Text, "P3C globals.xml");
                string file_temp_recipe = System.IO.Path.Combine(textBox2.Text, "S6 plus P3_EL V3 268.xml");
                xdoc1 = new XDocument();
                xdoc2 = new XDocument();
                xdoc1 = XDocument.Load(file_temp_global);
                xdoc2 = XDocument.Load(file_temp_recipe);
                using (ExcelPackage excelPackage = new ExcelPackage(file_temp_ce))
                {
                    //Processing
                    ExcelWorkbook workBook = excelPackage.Workbook;
                    ExcelWorksheet worksheet = workBook.Worksheets["5M+E Tool Parameter P3"];

                    // xml 
                    if (xdoc1 == null)
                    {
                        OpenFileDialog file_xml = new OpenFileDialog(); //open dialog to choose file
                        if (file_xml.ShowDialog() == DialogResult.OK) //if there is a file chosen by the user
                        {
                            string fileExt_xml = Path.GetExtension(file_xml.FileName); //get the file extension
                            string filePath_temp_xml = Path.GetFullPath(file_xml.FileName);
                            if (fileExt_xml.CompareTo(".xml") == 0 || fileExt_xml.CompareTo(".mpara") == 0)
                            {

                                StringBuilder result = new StringBuilder();
                                //Load xml
                                xdoc1 = XDocument.Load(filePath_temp_xml); // getting XML file with name xdoc 
                            }
                        }
                    }

                    if (xdoc2 == null)
                    {
                        OpenFileDialog file_xml2 = new OpenFileDialog(); //open dialog to choose file
                        if (file_xml2.ShowDialog() == DialogResult.OK) //if there is a file chosen by the user
                        {
                            string fileExt_xml2 = Path.GetExtension(file_xml2.FileName); //get the file extension
                            string filePath_temp_xml2 = Path.GetFullPath(file_xml2.FileName);
                            if (fileExt_xml2.CompareTo(".xml") == 0 || fileExt_xml2.CompareTo(".mreci") == 0)
                            {
                                StringBuilder result = new StringBuilder();
                                //Load xml
                                xdoc2 = XDocument.Load(filePath_temp_xml2); // getting XML file with name xdoc 
                            }
                        }
                    }
                    Trigger_xml(worksheet, xdoc1, xdoc2);
                    DataTable dt = new DataTable();
                    dataGridView1.RowCount = 377;
                    dataGridView1.ColumnCount = 20;
                    int rows = 377;
                    int cols = 20;
                    for (int i = 1; i <= rows; i++)
                    {
                        // Read new line
                        for (int j = 1; j <= cols; j++)
                        {
                            // Write to cell
                            if (worksheet.Cells[i, j].Value != null)
                            {
                                dataGridView1.Rows[i - 1].Cells[j - 1].Value = worksheet.Cells[i, j].Value.ToString();
                            }
                        }
                    }
                    for (int i = 6; i <= 377; i++)
                    {
                        // Making temp variable for getting Value2 object
                        // Type of data to Range
                        var temp_next = worksheet.Cells[i, 6].Value;
                        if (temp_next != null)
                        {
                            if (temp_next.ToString() == "R")
                            {
                                // Init value POR and toolactual to compare
                                object POR_value = dataGridView1.Rows[i - 1].Cells[4].Value;
                                object tool_value = dataGridView1.Rows[i - 1].Cells[6].Value;
                                if (POR_value != null && tool_value != null)
                                {
                                    compare_Value(POR_value, tool_value);
                                    if (check_color == true)// Adjustment actual value
                                    {
                                        // Current Cell Red color make 
                                        worksheet.Cells[i, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[i, 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                                        dataGridView1.Rows[i - 1].Cells[6].Style.BackColor = Color.LightGreen;
                                    }
                                    else if (check_color == false)
                                    {
                                        worksheet.Cells[i, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[i, 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                                        dataGridView1.Rows[i - 1].Cells[6].Style.BackColor = Color.Red;
                                    }
                                }
                            }
                            else
                            {
                                // Current Cell None color  make
                                worksheet.Cells[i, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[i, 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                                dataGridView1.Rows[i - 1].Cells[6].Style.BackColor = Color.LightGreen;
                            }
                        }
                    }
                    try
                    {
                        Program_SQL program_sql = new Program_SQL();
                        program_sql.connect_sql(worksheet, textBox1.Text, textBox3.Text, sqlconnectStr);
                        MessageBox.Show("Database update");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Can't push data to DB");
                    }
                    excelPackage.Save();
                    try
                    {
                        excelPackage.SaveAs(@"C:\LASER_CE");
                    }
                    catch (Exception) { }
                    try
                    {
                        Email email = new Email();
                        email.SendEmail(textBox8.Text, "CE AUTO PROGRAM", "Auto Email From Laser PROGRAM", file_temp_ce);
                        MessageBox.Show("Send Mail");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Can't send mail");
                    }
                }
            }
        }
        
        // Reset Timer and running
        private void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            aTimer.Stop();
            // Displays a message box asking whether to continue running the timer.
            if (MessageBox.Show("Continue running?", "next", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // Restarts the timer and increments the counter.
                this.trigger_daily();
                aTimer.Enabled = true;
            }
            else
            {
                // Stops the timer.
                exitFlag = true;
            }
        }

        // Start auto function runtime - auto run funtion after static timer
        private void button12_Click(object sender, EventArgs e)
        {
            aTimer.Tick += new EventHandler(TimerEventProcessor);
            // Sets the timer interval to 5 seconds.
            aTimer.Interval = Int32.Parse(textBox5.Text);
            aTimer.Start();
            // Runs the timer, and raises the event.
            while (exitFlag == false)
            {
                // Processes all the events in the queue.
                Application.DoEvents();
            }
        }
        
        // Change connect database string with input textbox
        private void button2_Click(object sender, EventArgs e)
        {
            sqlconnectStr = "Datasource=" + textBox1.Text + "," + textBox3.Text + ";Initial Catalog=" + textBox4.Text + ";User ID=" + textBox6.Text + ";Password=" + textBox7.Text;
        }

        // Change connect database string by input dialogBox to write string
        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(sqlconnectStr);
            form2.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        // function processing SQL server
    }
}
