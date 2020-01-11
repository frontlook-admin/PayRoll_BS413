using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using frontlook_dotnetframework_library.FL_webpage.FL_general;
using frontlook_dotnetframework_library.FL_webpage.FL_DataBase.FL_MySql;
using MySql.Data.MySqlClient;
using repository;

public partial class Salhead : System.Web.UI.Page
{
    private static readonly string Constring = ConfigurationManager.ConnectionStrings["payrollConnectionString"].ConnectionString;

    private readonly MySqlConnection con =
        new MySqlConnection(Constring);

    private readonly MySqlCommand cmd = new MySqlCommand();
    private MySqlDataReader reader;
    private string query = "";

    //Salhead_repo get_data = new Salhead_repo();
    //Salhead_repo persistant_data = new Salhead_repo();

    private string spaces, enter;

    protected void Page_Load(object sender, EventArgs e)
    {

        cmd.Connection = con;
        if (!IsPostBack)
        {
            Onpageload();
        }
        else
        {
            //Get_addgroupitems();
            //Get_salheadids();
            //Get_editgroupitems();
            //Modify_fetch_data();
            cmd.Connection = con;
        }
    }

    private void Con_switch_off()
    {
        if (con.State == ConnectionState.Open)
        {
            //_cmd.CommandText = "";
            con.Close();
        }
    }

    private void add_controls_clear()
    {
        var gdata = new Salhead_repo { _code = "", _formula = "", _group = null, _name = "", _operatorcode = "", _startdate = DateTime.Today };
        add_code.Text = gdata._code;
        add_name.Text = gdata._name;
        add_ddl_group.ClearSelection();
        add_ddl_operator.ClearSelection();
        add_group.Text = "";
        add_group.Visible = false;
        try
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            add_ddl_group.Items.FindByText(gdata._group).Selected = true;
            if (string.IsNullOrEmpty(gdata._operatorcode))
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                add_ddl_operator.Items.FindByValue(null).Selected = true;
            }
            else
                add_ddl_operator.Items.FindByText(gdata._operatorcode).Selected = true;
        }
        catch (Exception cv)
        {
            Console.WriteLine(cv.Message + enter + spaces + cv.StackTrace + enter + spaces + cv.Data + enter + spaces + cv.HelpLink +
                              enter + spaces + cv.Source + enter + spaces + cv.HResult + enter + spaces + cv.InnerException);
        }

        add_formula.Text = gdata._formula;
        add_startdate.Text = gdata._startdate.ToString("yyyy-MM-dd");
    }

    private void Listing_add_ddl()
    {
        Get_groupitems(add_ddl_group);
        Get_operator(add_ddl_operator);
        add_controls_clear();
    }

    private void Listing_edit_ddl()
    {
        Get_salheadids(salheadid);
        Get_groupitems(edit_ddl_group);
        Get_operator(edit_ddl_operator);
    }

    private void Get_salheadids(DropDownList dl)
    {
        try
        {
            dl.Items.Clear();
            var item1 = new ListItem
            {
                Text = "-Select Code-",
                Value = "0"
            };
            dl.Items.Add(item1);
            cmd.CommandText = "SELECT salhead_id,salhead_code FROM salary_head;";
            FL_MySqlExecutor.Con_switch(con);
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var item = new ListItem
                {
                    Text = reader["salhead_code"].ToString(),
                    Value = reader["salhead_id"].ToString()
                };
                dl.Items.Add(item);
            }
            reader.Close();
            FL_MySqlExecutor.Con_switch(con);
        }
        catch (Exception e)
        {
            Response.Write(FL_response.FL_message("Message 3:" + e.Message));
        }
    }


    private void Get_groupitems(DropDownList dl)
    {
        try
        {
            using (cmd)
            {
                //_cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT DISTINCT salhead_group FROM salary_head;";
                dl.Items.Clear();
                FL_MySqlExecutor.Con_switch(con);
                reader = cmd.ExecuteReader();
                var item1 = new ListItem { Text = "-Select Group-", Value = null };
                dl.Items.Add(item1);
                while (reader.Read())
                {
                    var item = new ListItem
                    {
                        Text = reader["salhead_group"].ToString(),
                        Value = reader["salhead_group"].ToString()
                    };
                    dl.Items.Add(item);
                }

                var item2 = new ListItem { Text = "Others", Value = "Others" };
                dl.Items.Add(item2);
                reader.Close();
                FL_MySqlExecutor.Con_switch(con);
            }
        }
        catch (Exception e)
        {
            Response.Write(FL_response.FL_message("Message 2:" + e.Message));
        }
    }


    private void Get_operator(DropDownList dl)
    {
        try
        {
            using (cmd)
            {
                cmd.CommandText = "SELECT operator_code,operator_name FROM head_operator;";
                dl.Items.Clear();
                FL_MySqlExecutor.Con_switch(con);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var item = new ListItem
                    {
                        Text = reader["operator_name"].ToString(),
                        Value = reader["operator_code"].ToString()
                    };
                    dl.Items.Add(item);
                }
                reader.Close();
                FL_MySqlExecutor.Con_switch(con);
            }
        }
        catch (Exception e)
        {
            Response.Write(FL_response.FL_message("Message 2:" + e.Message));
        }
    }

    private void Onpageload()
    {
        spaces = Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;");
        enter = Server.HtmlDecode("<br/><br/>");
        active_add_salaryhead_div.BackColor = System.Drawing.Color.DeepSkyBlue;
        active_edit_salaryhead_div.BackColor = System.Drawing.Color.Silver;
        add_sec_salhead.Visible = true;
        editdel_sec_salhead.Visible = false;
        Listing_add_ddl();
        add_group.Visible = string.Equals(add_ddl_group.SelectedItem.Text, "Others");
        add_startdate.Text = DateTime.Today.ToString("yyyy-MM-dd");
    }

    private void Insert_data(Salhead_repo ins)
    {
        cmd.Connection = con;

        cmd.CommandText = "CALL salary_head_insert ('" + ins._code + "','" + ins._name + "','" + ins._group +
                          "','" + ins._operatorcode + "','" + ins._formula + "','" + ins._startdate.ToString("yyyy-MM-dd") + "');";

        FL_MySqlExecutor.Con_switch(con);
        int r = cmd.ExecuteNonQuery();
        cmd.CommandText = "";
        FL_MySqlExecutor.Con_switch(con);

        if (r == 1)
        {
            cmd.CommandText = "ALTER TABLE salary_info ADD COLUMN `" + ins._name + "` DECIMAL(20,2);";
            FL_MySqlExecutor.Con_switch(con);
            try
            {
                var q = cmd.ExecuteNonQuery();
                FL_MySqlExecutor.Con_switch(con);
                cmd.CommandText = "";
                if (q == 1)
                {
                    Response.Write(FL_response.FL_message("Salary Head " + ins._name.ToUpper() + " Successfully Created...!!!",
                        "salaryhead.aspx';"));
                }
            }
            catch (MySqlException x)
            {
                Response.Write(FL_response.FL_message(x.Code + "\\n\\n" + x.SqlState + "\\n\\n" + x.StackTrace + "\\n\\n" + x.Message));
            }
            catch (Exception ex)
            {
                Response.Write(FL_response.FL_message("Message 1:" + ex.Message));
            }

        }
        else
        {
            Response.Write(FL_response.FL_message("Salary Head Is Already Present With Name " +
                                                  add_name.ToString().ToUpper()));
        }



    }

    private void Update_data(Salhead_repo set)
    {
        Response.Write(set._id);
        cmd.Connection = con;

        cmd.CommandText = "CALL salary_head_update(" + set._id + ",'" + set._code + "','" + set._name + "','" + set._group + "','" +
                          set._operatorcode + "','" + set._formula + "','" + set._startdate.ToString("yyyy-MM-dd") + "');";

        FL_MySqlExecutor.Con_switch(con);
        var r = cmd.ExecuteNonQuery();
        FL_MySqlExecutor.Con_switch(con);
        cmd.CommandText = "";

        if (r == 1)
        {
            cmd.CommandText = "ALTER TABLE salary_info CHANGE COLUMN `" + edit_oldname.Text + "` `" + set._name + "` DECIMAL(20,2);";
            FL_MySqlExecutor.Con_switch(con);
            var q = cmd.ExecuteNonQuery();
            FL_MySqlExecutor.Con_switch(con);
            if (q == 1)
            {
                Listing_edit_ddl();
                Response.Write(FL_response.FL_message("Salary Head Column " + edit_oldname.Text.ToUpper() + " Changed To" + set._name.ToUpper() +
                               " Successfully...!!!", "~/salaryhead.aspx"));
            }
        }
        else
        {

            Response.Write(FL_response.FL_message("Salary Head Is Already Present With Name " +
            set._name.ToUpper()));
        }


    }

    private void Delete_data()
    {
        cmd.Connection = con;
        try
        {
            cmd.CommandText = "CALL salary_head_delete(" + int.Parse(salheadid.SelectedValue) + ");";
            FL_MySqlExecutor.Con_switch(con);
            var r = cmd.ExecuteNonQuery();
            FL_MySqlExecutor.Con_switch(con);

            if (r == 1)
            {
                cmd.CommandText = "ALTER TABLE salary_info drop COLUMN `" + edit_oldname.Text + "`;";
                FL_MySqlExecutor.Con_switch(con);
                var q = cmd.ExecuteNonQuery();
                FL_MySqlExecutor.Con_switch(con);
                if (q == 1)
                {
                    Response.Write(FL_response.FL_message(edit_oldname.Text.ToUpper(), "salaryhead.aspx"));
                }
            }
            else
            {
                Response.Write(FL_response.FL_message("Something went wrong while deleting " +
                                                      edit_oldname.Text.ToUpper() + "...!!"));
            }
        }
        catch (Exception e)
        {
            Response.Write(FL_response.FL_message(e.Message));
        }
    }

    private void Set_data_for_saving(int a, int b)
    {
        var setData = new Salhead_repo
        {
            _group = b != 0 ? add_group.Text : add_ddl_group.SelectedItem.Text,
            _code = add_code.Text,
            _name = add_name.Text,
            _operatorcode = add_ddl_operator.SelectedItem.Text,
            _formula = add_formula.Text,
            _startdate = DateTime.Parse(add_startdate.Text)
        };
        if (a != 0)
        {
            Insert_data(setData);
        }
        else
        {
            Response.Write(FL_response.FL_message("No Fields Can Be Empty..!!"));
        }
        Listing_add_ddl();
    }

    private void Set_data_for_updating(int a, int b)
    {
        var set = new Salhead_repo
        {
            _group = b == 1 ? edit_group.Text : edit_ddl_group.SelectedItem.Text,
            _code = edit_code.Text,
            _name = edit_name.Text,
            _formula = edit_formula.Text,
            _operatorcode = edit_ddl_operator.SelectedItem.Text,
            _id = int.Parse(salheadid.SelectedValue),
            _oldname = edit_oldname.Text,
            _startdate = DateTime.ParseExact(edit_startdate.Text, "yyyy-MM-dd", null)
        };
        if (a == 1)
        {
            Update_data(set);
        }
    }

    private Salhead_repo Modify_data_allocation()
    {
        var gdata = new Salhead_repo { _id = int.Parse(salheadid.SelectedValue) };
        cmd.CommandText = "SELECT salhead_code,salhead_name,salhead_group,salhead_operator,salhead_formula,salhead_start_date FROM salary_head WHERE salhead_id = " + gdata._id + ";";
        FL_MySqlExecutor.Con_switch(con);
        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            gdata._code = reader["salhead_code"].ToString();
            gdata._name = reader["salhead_name"].ToString();
            gdata._group = reader["salhead_group"].ToString();
            gdata._operatorcode = reader["salhead_operator"].ToString();
            gdata._formula = reader["salhead_formula"].ToString();
            gdata._startdate = DateTime.Parse(reader["salhead_start_date"].ToString());
        }
        FL_MySqlExecutor.Con_switch(con);
        gdata._oldname = gdata._name;
        return gdata;
    }

    private void Modify_fetch_data()
    {
        var gdata = Modify_data_allocation();

        edit_code.Text = gdata._code;
        edit_name.Text = gdata._name;
        edit_oldname.Text = gdata._oldname;
        edit_ddl_group.ClearSelection();
        edit_ddl_operator.ClearSelection();
        try
        {
            //edit_ddl_group.Items.FindByValue(get_group.ToString()).Selected = true;
            edit_ddl_group.Items.FindByText(gdata._group).Selected = true;
            if (string.IsNullOrEmpty(gdata._operatorcode))
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                edit_ddl_operator.Items.FindByValue(null).Selected = true;
            }
            else
                edit_ddl_operator.Items.FindByText(gdata._operatorcode).Selected = true;
        }
        catch (Exception cv)
        {
            Console.WriteLine(cv.Message + enter + spaces + cv.StackTrace + enter + spaces + cv.Data + enter + spaces + cv.HelpLink +

                              enter + spaces + cv.Source + enter + spaces + cv.HResult + enter + spaces + cv.InnerException);
        }

        edit_formula.Text = gdata._formula;
        edit_startdate.Text = gdata._startdate.ToString("yyyy-MM-dd");

    }

    protected void Active_add_salaryhead_div_Click(object sender, EventArgs e)
    {
        active_add_salaryhead_div.BackColor = System.Drawing.Color.FromArgb(0, 102, 255);
        active_edit_salaryhead_div.BackColor = System.Drawing.Color.Silver;
        active_edit_salaryhead_div.ForeColor = System.Drawing.Color.Black;
        add_sec_salhead.Visible = true;
        editdel_sec_salhead.Visible = false;
        Listing_add_ddl();
        add_group.Visible = string.Equals(add_ddl_group.SelectedItem.Text, "Others");
        edit_group.Visible = string.Equals(edit_ddl_group.SelectedItem.Text, "Others");
    }

    protected void Active_edit_salaryhead_div_Click(object sender, EventArgs e)
    {
        active_edit_salaryhead_div.BackColor = System.Drawing.Color.FromArgb(0, 102, 255);
        active_add_salaryhead_div.BackColor = System.Drawing.Color.Silver;
        active_add_salaryhead_div.ForeColor = System.Drawing.Color.Black;
        editdel_sec_salhead.Visible = true;
        add_sec_salhead.Visible = false;
        Listing_edit_ddl();
        Modify_fetch_data();
        add_group.Visible = string.Equals(add_ddl_group.SelectedItem.Text, "Others");
        edit_group.Visible = string.Equals(edit_ddl_group.SelectedItem.Text, "Others");
    }

    protected void Save_salhead_Click(object sender, EventArgs e)
    {
        int a = 0;
        int b = 0;
        if (!add_ddl_group.SelectedValue.Equals(null))
        {
            if (!string.IsNullOrEmpty(add_code.Text) && !string.IsNullOrEmpty(add_name.Text) &&
                !string.IsNullOrEmpty(add_ddl_operator.SelectedValue))
            {
                if (add_ddl_group.SelectedItem.Text == "Others")
                {
                    if (add_group.Text != null || add_group.Text != string.Empty)
                    {
                        a = 1;
                        b = 1;
                    }
                }
                else
                {
                    a = 1;
                }
            }
        }
        Set_data_for_saving(a, b);
    }

    protected void Salheadid_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Response.Write(salheadid.SelectedValue);
        Modify_fetch_data();
    }

    protected void Update_salhead_Click(object sender, EventArgs e)
    {
        int a = 0;
        int b = 0;
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        if (!edit_ddl_group.SelectedValue.Equals(null))
        {
            if (!string.IsNullOrEmpty(edit_code.Text) && !string.IsNullOrEmpty(edit_name.Text))
            {
                if (string.Equals(edit_ddl_group.SelectedValue, "Others"))
                {
                    if (!string.IsNullOrEmpty(edit_group.Text) || !string.IsNullOrWhiteSpace(edit_group.Text))
                    {
                        a = 1;
                        b = 1;
                    }
                }
                else
                {
                    a = 1;
                }
            }
        }

        if (a != 0)
        {
            Set_data_for_updating(a, b);
        }
        else
        {
            Response.Write(FL_response.FL_message("No Fields Can Be Empty..!!"));
        }
        Listing_edit_ddl();
        Modify_fetch_data();
    }

    protected void Del_salhead_Click(object sender, EventArgs e)
    {
        Delete_data();
        Listing_edit_ddl();
        Modify_fetch_data();
    }

    protected void add_ddl_group_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (string.Equals(add_ddl_group.SelectedItem.Text, "Others"))
        {
            add_group.Visible = string.Equals(add_ddl_group.SelectedItem.Text, "Others");
            add_group.Focus();
        }
        else
        {
            add_group.Visible = string.Equals(add_ddl_group.SelectedItem.Text, "Others");
        }
    }

    protected void edit_ddl_group_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (string.Equals(edit_ddl_group.SelectedItem.Text, "Others"))
        {
            edit_group.Visible = string.Equals(edit_ddl_group.SelectedItem.Text, "Others");
            edit_group.Focus();
        }
        else
        {
            edit_group.Visible = string.Equals(edit_ddl_group.SelectedItem.Text, "Others");
        }
    }
}