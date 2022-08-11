using PRP.PPL.System.include.config.connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PPL.Data.HRD.IOM.Add
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string UserInfo()
        {
            string UserName = HttpContext.Current.Session["USER_NAME"].ToString();
            return UserName;
        }

        [WebMethod]
        public static string Defaultdept()
        {
            db_ppl Connstring = new db_ppl();
            DataTable depts = Connstring.SqlDataTable(@"SELECT MainDepartment FROM hrd.IOMDepartmentPermission where PIN='" + HttpContext.Current.Session["EMP_ID"].ToString() + "'");
            string depts2 = depts.Rows[0][0].ToString();
            return depts2;
        }

        [WebMethod]
        public static string GetDept()
        {
            db_ppl Connstring = new db_ppl();
            DataTable deptCount = Connstring.SqlDataTable(@"SELECT COUNT(MainDepartment) AS Department FROM hrd.IOMDepartmentPermission where PIN='" + HttpContext.Current.Session["EMP_ID"].ToString() + "'");
            string depts = deptCount.Rows[0][0].ToString();
            return depts;
        }

        //-----Get Account Head ------//
        [WebMethod]
        public static List<ListItem> set_drop_down_list()
        {
            db_ppl Connstring = new db_ppl();
            string query = @"SELECT DISTINCT hrd.IOMDepartmentPermission.MainDepartment AS Code, hrd.IOMDepartment.MainDepartmentName AS Name FROM hrd.IOMDepartmentPermission INNER JOIN hrd.IOMDepartment ON hrd.IOMDepartmentPermission.MainDepartment = hrd.IOMDepartment.MainDepartment WHERE (hrd.IOMDepartmentPermission.PIN = '" + HttpContext.Current.Session["EMP_ID"].ToString() + "')";

            //  string query = @"SELECT hrd.IOMDepartmentPermission.MainDepartment AS Code, hrd.Department.Name FROM hrd.IOMDepartmentPermission INNER JOIN hrd.Department ON hrd.IOMDepartmentPermission.MainDepartment = hrd.Department.Code WHERE (hrd.IOMDepartmentPermission.PIN = '" + HttpContext.Current.Session["EMP_ID"].ToString() + "')";

            using (SqlConnection con = Connstring.getcon)
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    List<ListItem> publishers = new List<ListItem>();
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            publishers.Add(new ListItem
                            {
                                Value = sdr["Code"].ToString(),
                                Text = sdr["Name"].ToString()
                            });
                        }
                    }
                    con.Close();
                    return publishers;
                }
            }
        }

        [WebMethod]
        public static string cs_code()
        {
            db_ppl Connstring = new db_ppl();
            HttpContext.Current.Session["voucher_no"] = null;

            //Employee department
            DataTable dept = Connstring.SqlDataTable(@"SELECT Department_Code FROM hrd.Employee where PIN='" + HttpContext.Current.Session["EMP_ID"].ToString() + "'");
            string depts = dept.Rows[0][0].ToString();
            HttpContext.Current.Session["OwnCode"] = depts;

            //Head Department
            DataTable HeadDept = Connstring.SqlDataTable(@"SELECT DISTINCT hrd.IOMDepartmentPermission.MainDepartment FROM hrd.IOMDepartmentPermission INNER JOIN hrd.IOMDepartment ON hrd.IOMDepartmentPermission.MainDepartment = hrd.IOMDepartment.MainDepartment WHERE (hrd.IOMDepartment.SubDepartment = N'" + depts + "') AND (hrd.IOMDepartmentPermission.PIN = N'" + HttpContext.Current.Session["EMP_ID"].ToString() + "')");
            string HeadDepts = HeadDept.Rows[0][0].ToString();


            HttpContext.Current.Session["deptCode"] = HeadDepts;

            string year = DateTime.Now.ToString("yy");
            string month = DateTime.Now.ToString("MM");
            if (month == "07" || month == "08" || month == "09" || month == "10" || month == "11" || month == "12")
            {
                string fy = DateTime.Now.ToString("yy") + DateTime.Now.AddYears(+1).ToString("yy");
                HttpContext.Current.Session["fy"] = fy;
                HttpContext.Current.Session["month"] = month;
            }
            else if (month == "01" || month == "02" || month == "03" || month == "04" || month == "05" || month == "06")
            {
                string fy = DateTime.Now.AddYears(-1).ToString("yy") + DateTime.Now.ToString("yy");
                HttpContext.Current.Session["fy"] = fy;
                HttpContext.Current.Session["month"] = month.Substring(1);
            }

            DataTable dt = Connstring.SqlDataTable(@"SELECT No as MAX FROM hrd.IOM WHERE (ID = (SELECT DISTINCT MAX(ID) AS Expr1 FROM hrd.IOM AS IOM_1 WHERE (MainDepartment = '" + HeadDepts + "')))");

            if (dt.Rows.Count >= 1)
            {
                string sqldata = dt.Rows[0]["MAX"].ToString();
                string[] tokens = sqldata.Split('/');
                string middle = tokens[tokens.Length - 1];
                string[] middles = sqldata.Split('-');
                string sl = middles[middles.Length - 1];

                string increment = Convert.ToString(Convert.ToInt32(sl.ToString()) + 1);
                string zero = "000";
                int get_lengt = zero.Length - increment.Length;
                string lenght = zero.Substring(0, get_lengt);
                string cash_memo = lenght + increment;
                HttpContext.Current.Session["voucher_no"] = "PPL/" + HttpContext.Current.Session["deptCode"] + "/IOM/" + HttpContext.Current.Session["fy"] + "-" + cash_memo;
            }
            else
            {
                HttpContext.Current.Session["voucher_no"] = "PPL/" + HttpContext.Current.Session["deptCode"] + "/IOM/" + HttpContext.Current.Session["fy"] + "-" + "001";
            }

            string voucher_serial = HttpContext.Current.Session["voucher_no"].ToString();
            return voucher_serial;
        }

        [WebMethod]
        public static string cs_codetxtNo(string cboDepartment)
        {
            db_ppl Connstring = new db_ppl();
            HttpContext.Current.Session["voucher_no"] = null;

            //Employee department
            //   DataTable dept = Connstring.SqlDataTable(@"SELECT Department_Code FROM hrd.Employee where PIN='" + HttpContext.Current.Session["EMP_ID"].ToString() + "'");
            //  string depts = dept.Rows[0][0].ToString();

            //Head Department
            //DataTable HeadDept = Connstring.SqlDataTable(@"SELECT DISTINCT hrd.IOMDepartmentPermission.MainDepartment FROM hrd.IOMDepartmentPermission INNER JOIN hrd.IOMDepartment ON hrd.IOMDepartmentPermission.MainDepartment = hrd.IOMDepartment.MainDepartment WHERE (hrd.IOMDepartment.SubDepartment = N'" + depts + "') AND (hrd.IOMDepartmentPermission.PIN = N'" + HttpContext.Current.Session["EMP_ID"].ToString() + "')");
            //string HeadDepts = HeadDept.Rows[0][0].ToString();

            DataTable HeadDept = Connstring.SqlDataTable(@"SELECT DISTINCT hrd.IOMDepartmentPermission.MainDepartment FROM hrd.IOMDepartmentPermission INNER JOIN hrd.IOMDepartment ON hrd.IOMDepartmentPermission.MainDepartment = hrd.IOMDepartment.MainDepartment WHERE (hrd.IOMDepartmentPermission.PIN = N'" + HttpContext.Current.Session["EMP_ID"].ToString() + "') AND (hrd.IOMDepartmentPermission.MainDepartment = N'"+ cboDepartment + "')");
            string HeadDepts = HeadDept.Rows[0][0].ToString();

            HttpContext.Current.Session["deptCode"] = HeadDepts;

            string year = DateTime.Now.ToString("yy");
            string month = DateTime.Now.ToString("MM");
            if (month == "07" || month == "08" || month == "09" || month == "10" || month == "11" || month == "12")
            {
                string fy = DateTime.Now.ToString("yy") + DateTime.Now.AddYears(+1).ToString("yy");
                HttpContext.Current.Session["fy"] = fy;
                HttpContext.Current.Session["month"] = month;
            }
            else if (month == "01" || month == "02" || month == "03" || month == "04" || month == "05" || month == "06")
            {
                string fy = DateTime.Now.AddYears(-1).ToString("yy") + DateTime.Now.ToString("yy");
                HttpContext.Current.Session["fy"] = fy;
                HttpContext.Current.Session["month"] = month.Substring(1);
            }

            DataTable dt = Connstring.SqlDataTable(@"SELECT No as MAX FROM hrd.IOM WHERE (ID = (SELECT DISTINCT MAX(ID) AS Expr1 FROM hrd.IOM AS IOM_1 WHERE (MainDepartment = '" + HeadDepts + "')))");

            if (dt.Rows.Count >= 1)
            {
                string sqldata = dt.Rows[0]["MAX"].ToString();
                string[] tokens = sqldata.Split('/');
                string middle = tokens[tokens.Length - 1];
                string[] middles = sqldata.Split('-');
                string sl = middles[middles.Length - 1];

                string increment = Convert.ToString(Convert.ToInt32(sl.ToString()) + 1);
                string zero = "000";
                int get_lengt = zero.Length - increment.Length;
                string lenght = zero.Substring(0, get_lengt);
                string cash_memo = lenght + increment;
                HttpContext.Current.Session["voucher_no"] = "PPL/" + HttpContext.Current.Session["deptCode"] + "/IOM/" + HttpContext.Current.Session["fy"] + "-" + cash_memo;
            }
            else
            {
                HttpContext.Current.Session["voucher_no"] = "PPL/" + HttpContext.Current.Session["deptCode"] + "/IOM/" + HttpContext.Current.Session["fy"] + "-" + "001";
            }

            string voucher_serial = HttpContext.Current.Session["voucher_no"].ToString();
            return voucher_serial;
        }

        [WebMethod]
        public static void SaveData(string txtNo, string txtDate, string txtSubject, string cboType, string cboLanguage, string txtTo, string txtFrom, string txtThru, string txtCC, string txtBCC, string txtCCPersonnel, string txtTopic, string txtRegard)
        {
            db_ppl Connstring = new db_ppl();

            string sql = @"INSERT INTO hrd.IOM (No, Date, Subject, Type,Language, [TO], [From], Thru, CC, BCC,PCC, Topic, Regard,DepartmentCode,MainDepartment, AddDate, UserID) VALUES (@No, @Date, @Subject, @Type, @Language, @TO, @From, @Thru, @CC, @BCC, @PCC,@Topic, @Regard, @DepartmentCode, @MainDepartment, @AddDate, @UserID)";
            SqlCommand MyCommand = new SqlCommand(sql, Connstring.conn);

            MyCommand.Parameters.AddWithValue("@No", txtNo);
            DateTime dt2 = DateTime.ParseExact(txtDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string txtdate_format = dt2.ToString("yyyy-MM-dd");
            MyCommand.Parameters.AddWithValue("@Date", txtdate_format);
            MyCommand.Parameters.AddWithValue("@Subject", txtSubject);
            MyCommand.Parameters.AddWithValue("@Type", cboType);
            MyCommand.Parameters.AddWithValue("@Language", cboLanguage);
            MyCommand.Parameters.AddWithValue("@TO", txtTo);
            MyCommand.Parameters.AddWithValue("@From", txtFrom);
            MyCommand.Parameters.AddWithValue("@Thru", txtThru);
            MyCommand.Parameters.AddWithValue("@CC", txtCC);
            MyCommand.Parameters.AddWithValue("@BCC", txtBCC);
            MyCommand.Parameters.AddWithValue("@PCC", txtCCPersonnel);
            MyCommand.Parameters.AddWithValue("@Topic", txtTopic);
            MyCommand.Parameters.AddWithValue("@Regard", txtRegard);

            string fgdfg = HttpContext.Current.Session["deptCode"].ToString();

            if (HttpContext.Current.Session["deptCode"].ToString() == "ADHE")
            {
                MyCommand.Parameters.AddWithValue("@DepartmentCode", "ADHG");
            }
            else if (HttpContext.Current.Session["deptCode"].ToString() == "RND0")
            {
                MyCommand.Parameters.AddWithValue("@DepartmentCode", "RNDK");
            }
            else
            {                
                MyCommand.Parameters.AddWithValue("@DepartmentCode", HttpContext.Current.Session["OwnCode"].ToString());
            }
            MyCommand.Parameters.AddWithValue("@MainDepartment", HttpContext.Current.Session["deptCode"].ToString());

            MyCommand.Parameters.AddWithValue("@AddDate", DateTime.Now);
            MyCommand.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["USERID"].ToString());
            Connstring.conn.Open();
            MyCommand.ExecuteNonQuery();
            HttpContext.Current.Session["IOMNo"] = txtNo;
            Connstring.conn.Close();
        }
    }
}