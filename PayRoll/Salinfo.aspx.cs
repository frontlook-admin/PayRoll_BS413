using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using frontlook_dotnetframework_library.FL_webpage.FL_Controls;
using MySql.Data.MySqlClient;
using _response = frontlook_dotnetframework_library.FL_webpage.FL_general.FL_response;
using _controls = frontlook_dotnetframework_library.FL_webpage.FL_Controls.FL_GetControl;
using _sql = frontlook_dotnetframework_library.FL_webpage.FL_DataBase.FL_MySql.FL_MySqlExecutor;
using _dtmod = frontlook_dotnetframework_library.FL_universal.FL_DataMods;

public partial class Salinfo : System.Web.UI.Page
{
    private static readonly string Constring = ConfigurationManager.ConnectionStrings["payrollConnectionString"].ConnectionString;

    private readonly MySqlConnection con =
        new MySqlConnection(Constring);

    private readonly MySqlCommand cmd = new MySqlCommand();
    private MySqlDataReader reader;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            OnPageLoad();
        }
        dynamiccontrols();
        
    }

    public void OnPageLoad()
    {
        //dynamiccontrols();
        Get_Elployees(emp);
    }

    private void dynamiccontrols()
    {
        try
        {
            cmd.Connection = con;
            //cmd.CommandText = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='payroll_db' AND TABLE_NAME='salary_info' AND COLUMN_NAME NOT IN (SELECT 'id');";
            cmd.CommandText = "SELECT salhead_name FROM salary_head;";
            _sql.Con_switch(con);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string a = reader["salhead_name"].ToString();
                add_sec_salinfo.Controls.Add(FL_Label_TextBox.FL_label_textbox1(a));
            }
            reader.Close();
            _sql.Con_switch(con);
        }
        catch (MySqlException exception)
        {
            Response.Write(_response.FL_message("Sorry..!! Unable to create the form. Please contact your developer for help."));
        }
    }

    private void Get_Elployees(DropDownList dl)
    {
        try
        {
            cmd.Connection = con;
            cmd.CommandText = "SELECT concat(IFNULL(CONCAT(employee.id,'     '),''),IFNULL(CONCAT(employee.fname,' '),''),IFNULL(CONCAT(employee.mname,' '),''),IFNULL(CONCAT(employee.lname,' '),'')) as name,id FROM employee;";
            /*DropDownList ddl = new DropDownList();
            add_sec_salinfo.Controls.Add(FL_Label_DropDownList.FL_form_create_dropdownlist1("Employee", con,cmd, 
                "SELECT concat(IFNULL(CONCAT(employee.fname,' '),''),IFNULL(CONCAT(employee.mname,' '),''),IFNULL(CONCAT(employee.lname,' '),'')) as Employee,id FROM employee;",
                "Employee","id"));*/
            dl.Items.Clear();
            var item1 = new ListItem
            {
                Text = "-Select Employee-",
                Value = "0"
            };
            dl.Items.Add(item1);
           
            _sql.Con_switch(con);
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var item = new ListItem
                {
                    Text = reader["name"].ToString(),
                    Value = reader["id"].ToString()
                };
                dl.Items.Add(item);
            }
            reader.Close();
            _sql.Con_switch(con);
        }
        catch (Exception e)
        {
            Response.Write(_response.FL_message("Message 3:" + e.Message));
        }
    }

    private void get_data(string id)
    {
        int count = 0;
        string c = "`";
        string a = "`,`";
        
        cmd.CommandText = "SELECT COUNT(salhead_name) as c FROM salary_head;";
        cmd.Connection = con;
        _sql.Con_switch(con);
        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            count = int.Parse(reader["c"].ToString());
            //Response.Write(_response.FL_message(count.ToString()));
        }
        reader.Close();
        _sql.Con_switch(con);
        string[] controlids = new string[count];
        string[] ids = new string[count];
        string[] vals = new string[count];
        cmd.CommandText = "SELECT salhead_name FROM salary_head;";
        int i = 0;
        cmd.Connection = con;
        _sql.Con_switch(con);
        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            ids[i] = reader["salhead_name"].ToString();
            controlids[i] = ids[i].Replace(" ", "");
            i++;
        }
        reader.Close();
        reader.Dispose();
        _sql.Con_switch(con);

        string q = "";
        for (int b = 0; b <= (count-1); b++)
        {
            if (b == 0)
            {
                if (count == 1)
                {
                    q = q +c+ ids[b]+c;
                }
                else
                {
                    q = q + c+ids[b]+a;
                }
            }
            else if(b > 0 && count > (b + 1))
            {
                q = q + ids[b] + a;
            }
            else if (b > 0 && count == (b + 1))
            {
                q = q + ids[b] + c;
            }
            else
            {
                break;
            }
        }
        if (emp.SelectedValue.Equals("0"))
        {
            for (int j = 0; j + 1 <= count; j++)
            {
                ((TextBox)_controls.FL_GetChildControl(add_sec_salinfo, controlids[j])).Text = string.Empty;

            }
        }
        else
        {

            string r = "SELECT " + q + " FROM salary_info WHERE id = " + id + ";";
            //Response.Write(r + "");
            cmd.CommandText = "SELECT " + q + " FROM salary_info WHERE id = " + id + ";";
            _sql.Con_switch(con);
            MySqlDataReader reader1 = cmd.ExecuteReader();

            while (reader1.Read())
            {
                if (emp.SelectedValue.Equals("0"))
                {
                    for (int j = 0; j + 1 <= count; j++)
                    {
                        ((TextBox)_controls.FL_GetChildControl(add_sec_salinfo, controlids[j])).Text = string.Empty;

                    }
                }
                else
                {
                    for (int j = 0; j + 1 <= count; j++)
                    {
                        string val = reader1[ids[j]].ToString();
                        ((TextBox)_controls.FL_GetChildControl(add_sec_salinfo, controlids[j])).Text = val;

                    }
                }
            }
            reader1.Close();
            _sql.Con_switch(con);
        }
    }

    private string queary_build_updatedata()
    {
        string q = "";
        string a = ", `";
        string b = "`=";
        string v = "";
        string c = "`";
        string d = ",";
        int count = 0;
        cmd.CommandText = "SELECT COUNT(salhead_name) as c FROM salary_head;";
        cmd.Connection = con;
        _sql.Con_switch(con);
        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            count = int.Parse(reader["c"].ToString());
            //Response.Write(_response.FL_message(count.ToString()));
        }
        reader.Close();
        _sql.Con_switch(con);
        string[] controlids = new string[count];
        string[] ids = new string[count];
        cmd.CommandText = "SELECT salhead_name FROM salary_head;";
        int i = 0;
        cmd.Connection = con;
        _sql.Con_switch(con);
        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            ids[i] = reader["salhead_name"].ToString();
            controlids[i] = ids[i].Replace(" ", "");
            i++;
        }
        reader.Close();
        reader.Dispose();
        _sql.Con_switch(con);

        for (int j = 0; j <= (count - 1); j++)
        {
            if (j == 0)
            {
                if (count == 1)
                {
                    v = _controls.FL_GetControlString(add_sec_salinfo, controlids[j]).Trim();
                    q = c + ids[j] + b + v;
                }
                else
                {
                    v = _controls.FL_GetControlString(add_sec_salinfo, controlids[j]).Trim();
                    q = c + ids[j] + b + v + a;
                }
            }
            else if (j > 0 && count > (j + 1))
            {
                v = _controls.FL_GetControlString(add_sec_salinfo, controlids[j]).Trim();
                q = q + ids[j] + b + v + a;
            }
            else if (j > 0 && count == (j + 1))
            {
                v = _controls.FL_GetControlString(add_sec_salinfo, controlids[j]).Trim();
                q = "UPDATE salary_info SET " + q + ids[j] + b + v + " WHERE id = " + emp.SelectedValue + "; ";
            }
        }

        //Response.Write(q.ToString());
        return q;
    }

    /*private string queary_build_savedata()
    {
        string q="";
        string a = "`, `";
        string g = ", ";
        string b = "(`";
        string c = "`) VALUES ();";
        string e;
        string v = "";
        string d = "`";
        string f = "";
        int count = 0;
        cmd.CommandText = "SELECT COUNT(salhead_name) as c FROM salary_head;";
        cmd.Connection = con;
        _sql.Con_switch(con);
        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            count = int.Parse(reader["c"].ToString());
            //Response.Write(_response.FL_message(count.ToString()));
        }
        reader.Close();
        _sql.Con_switch(con);

        cmd.CommandText = "SELECT salhead_name FROM salary_head;";
        int i = 0;
        cmd.Connection = con;
        _sql.Con_switch(con);
        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            if (i == 0)
            {
                if (count == 1)
                {
                    v = f+ _controls.FL_GetControlString(add_sec_salinfo, reader["salhead_name"].ToString().Replace(" ", "")).Trim() + f;
                    e = "`) VALUES (" + v + ");";
                    q = b + reader["salhead_name"].ToString() + e;
                }
                else
                {
                    v = d+_controls.FL_GetControlString(add_sec_salinfo, reader["salhead_name"].ToString().Replace(" ", "")).Trim()+ g;
                    q = b + reader["salhead_name"].ToString() + a;
                }
            }
            else if (i > 0 && count > (i + 1))
            {
                v = v+ _controls.FL_GetControlString(add_sec_salinfo, reader["salhead_name"].ToString().Replace(" ", "")).Trim() + g;
                q = q + reader["salhead_name"].ToString() + a;
            }
            else if (i > 0 && count == (i + 1))
            {
                v =v+ _controls.FL_GetControlString(add_sec_salinfo, reader["salhead_name"].ToString().Replace(" ", "")).Trim() + f;
                e = "`) VALUES (" + v + ") WHERE id = "+emp.SelectedValue+";";
                q = "INSERT INTO salary_info "+q + reader["salhead_name"].ToString() + e;
            }
            
            i++;
        }
        reader.Close();
        _sql.Con_switch(con);
        Response.Write(q.ToString());
        return q;
    }*/

    protected void update_salinfo_Click(object sender, EventArgs e)
    {
        try
        {
            string queary = queary_build_updatedata();
            //Response.Write(queary);
            cmd.CommandText = queary;
            _sql.Con_switch(con);
            int r = cmd.ExecuteNonQuery();
            _sql.Con_switch(con);
            if (r == 1)
            {
                Response.Write(_response.FL_message("Data Updated Successfully..!!"));
                emp.ClearSelection();
                emp.Items.FindByValue("0");
                get_data(emp.SelectedValue);
            }
            else
            {
                Response.Write(
                    _response.FL_message("Something went wrong..!! Contact your system administrator for help..!!"));
            }
        }
        catch (MySqlException ex)
        {
            Response.Write(_response.FL_message(ex.Message));
        }
        /*catch (Exception exs)
        {
            Response.Write(_response.FL_message("Fields must not contain any characters or special characters other than dot..!!"));
        }*/
    }

    protected void emp_SelectedIndexChanged(object sender, EventArgs e)
    {
        get_data(emp.SelectedValue);
    }
}