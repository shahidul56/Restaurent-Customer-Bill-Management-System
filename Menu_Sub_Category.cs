﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace Cui_Jall_Restraurent__Management
{
    public partial class Menu_Sub_Category : Form
    {

        ConnectionString cs = new ConnectionString();
        CommonClasses cc = new CommonClasses();
        clsFunc cf = new clsFunc();
        string st1;
        string st2;
        public Menu_Sub_Category()
        {
            InitializeComponent();
        }
        public void FillCombo()
        {
            try
            {
                cc.con = new SqlConnection(cs.DBConn);
                cc.con.Open();
                string ct = "select RTRIM(CategoryName) from Category order by CategoryName";
                cc.cmd = new SqlCommand(ct);
                cc.cmd.Connection = cc.con;
                cc.rdr = cc.cmd.ExecuteReader();
                while (cc.rdr.Read())
                {
                    cmbCategory.Items.Add(cc.rdr[0]);
                }
                cc.con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void frmCategory_Load(object sender, EventArgs e)
        {
            Autocomplete();
            GetData();
            FillCombo();
        }
        private void delete_records()
        {

            try
            {
                int RowsAffected = 0;
                cc.con = new SqlConnection(cs.DBConn);
                cc.con.Open();
                string cq = "delete from SubCategory where SCat_ID=" + txtSubCategoryID.Text + "";
                cc.cmd = new SqlCommand(cq);
                cc.cmd.Connection = cc.con;
                RowsAffected = cc.cmd.ExecuteNonQuery();
                if (RowsAffected > 0)
                {
                    st1 = lblUser.Text;
                    st2 = "deleted the Sub category '" + txtSubCategoryName.Text + "'";
                    cf.LogFunc(st1, System.DateTime.Now, st2);
                    Reset();
                    Autocomplete();
                    GetData();
                    MessageBox.Show("Successfully deleted", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   
                }
                else
                {
                    MessageBox.Show("No Record found", "Sorry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Reset();
                    Autocomplete();
                    GetData();
                }
                if (cc.con.State == ConnectionState.Open)
                {
                    cc.con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Autocomplete()
        {
            try
            {
                cc.con = new SqlConnection(cs.DBConn);
                cc.con.Open();
                cc.cmd = new SqlCommand("SELECT distinct SubCategoryname FROM SubCategory", cc.con);
                cc.ds = new DataSet();
                cc.da = new SqlDataAdapter(cc.cmd);
                cc.da.Fill(cc.ds, "SubCategory");
                AutoCompleteStringCollection col = new AutoCompleteStringCollection();
                int i = 0;
                for (i = 0; i <= cc.ds.Tables[0].Rows.Count - 1; i++)
                {
                    col.Add(cc.ds.Tables[0].Rows[i]["SubCategoryname"].ToString());

                }
                txtSubCategoryName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtSubCategoryName.AutoCompleteCustomSource = col;
                txtSubCategoryName.AutoCompleteMode = AutoCompleteMode.Suggest;
                cc.con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
 
        public void Reset()
        {
            txtSubCategoryName.Text = "";
            txtCategoryID.Text = "";
            cmbCategory.SelectedIndex=-1;
            txtSubCategoryID.Text = "";
            btnSave.Enabled = true;
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
            txtSubCategoryName.Focus();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtSubCategoryName.Text == "")
            {
                MessageBox.Show("Please enter sub category", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSubCategoryName.Focus();
                return;
            }
            if (cmbCategory.Text == "")
            {
                MessageBox.Show("Please select category", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategory.Focus();
                return;
            }
            try
            {

                cc.con = new SqlConnection(cs.DBConn);
                cc.con.Open();
                string ct = "select SubCategoryName from SubCategory where SubCategoryName=@d1";
                cc.cmd = new SqlCommand(ct);
                cc.cmd.Connection = cc.con;
                cc.cmd.Parameters.AddWithValue("@d1", txtSubCategoryName.Text);
                cc.rdr = cc.cmd.ExecuteReader();
                if (cc.rdr.Read())
                {
                    MessageBox.Show("Sub Category Already Exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSubCategoryName.Text = "";
                    txtSubCategoryName.Focus();
                    if ((cc.rdr != null))
                    {
                        cc.rdr.Close();
                    }
                    return;
                }

                cc.con = new SqlConnection(cs.DBConn);
                cc.con.Open();
                string cb = "insert into SubCategory(SubCategoryName,CategoryID) VALUES (@d1," + txtCategoryID.Text + ")";
                cc.cmd = new SqlCommand(cb);
                cc.cmd.Connection = cc.con;
                cc.cmd.Parameters.AddWithValue("@d1", txtSubCategoryName.Text);
                cc.cmd.ExecuteReader();
                cc.con.Close();
                st1 = lblUser.Text;
                st2 = "added the new sub category '" + txtSubCategoryName.Text + "'";
                cf.LogFunc(st1,System.DateTime.Now,st2);
                Autocomplete();
                GetData();
                btnSave.Enabled = false;
                MessageBox.Show("Successfully saved", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
              

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                delete_records();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtSubCategoryName.Text == "")
                {
                    MessageBox.Show("Please enter sub category", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSubCategoryName.Focus();
                    return;
                }
                if (cmbCategory.Text == "")
                {
                    MessageBox.Show("Please select category", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbCategory.Focus();
                    return;
                }
                cc.con = new SqlConnection(cs.DBConn);
                cc.con.Open();
                string cb = "update SubCategory set SubCategoryName=@d1,CategoryID=" + txtCategoryID.Text + " where SCat_ID=" + txtSubCategoryID.Text + "";
                cc.cmd = new SqlCommand(cb);
                cc.cmd.Connection = cc.con;
                cc.cmd.Parameters.AddWithValue("@d1", txtSubCategoryName.Text);
                cc.cmd.ExecuteReader();
                cc.con.Close();
                st1 = lblUser.Text;
                st2 = "updated the sub category '" + txtSubCategoryName.Text + "'";
                cf.LogFunc(st1, System.DateTime.Now, st2);
                Autocomplete();
                GetData();
                btnUpdate.Enabled = false;
                MessageBox.Show("Successfully updated", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgw_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            string strRowNumber = (e.RowIndex + 1).ToString();
            SizeF size = e.Graphics.MeasureString(strRowNumber, this.Font);
            if (dgw.RowHeadersWidth < Convert.ToInt32((size.Width + 20)))
            {
                dgw.RowHeadersWidth = Convert.ToInt32((size.Width + 20));
            }
            Brush b = SystemBrushes.ControlText;
            e.Graphics.DrawString(strRowNumber, this.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2));
    
        }

        private void dgw_MouseClick(object sender, MouseEventArgs e)
        {
            DataGridViewRow dr = dgw.SelectedRows[0];
            // or simply use column name instead of index
            //dr.Cells["id"].Value.ToString();
            txtSubCategoryID.Text = dr.Cells[0].Value.ToString();
            txtSubCategoryName.Text = dr.Cells[1].Value.ToString();
            txtCategoryID.Text = dr.Cells[2].Value.ToString();
            cmbCategory.Text = dr.Cells[3].Value.ToString();
            btnDelete.Enabled = true;
            btnUpdate.Enabled = true;
            txtSubCategoryName.Focus();
            btnSave.Enabled = false;
        }
        public void GetData()
        {
            try
            {
                cc.con = new SqlConnection(cs.DBConn);
                cc.con.Open();
                String sql = "SELECT RTRIM(SCat_ID),RTRIM(SubCategoryName),RTRIM(Cat_ID),RTRIM(CategoryName) from Category,SubCategory where Category.Cat_ID=SubCategory.CategoryID order by SubCategoryName";
                cc.cmd = new SqlCommand(sql, cc.con);
                cc.rdr = cc.cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dgw.Rows.Clear();
                while (cc.rdr.Read() == true)
                {
                    dgw.Rows.Add(cc.rdr[0], cc.rdr[1],cc.rdr[2],cc.rdr[3]);
                }
                cc.con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cc.con = new SqlConnection(cs.DBConn);
                cc.con.Open();
                cc.cmd = cc.con.CreateCommand();
                cc.cmd.CommandText = "SELECT Cat_ID from Category WHERE CategoryName=@d1";
                cc.cmd.Parameters.AddWithValue("@d1", cmbCategory.Text);
                cc.rdr = cc.cmd.ExecuteReader();
                if (cc.rdr.Read())
                {
                    txtCategoryID.Text = cc.rdr.GetInt32(0).ToString().Trim();
                }
                if ((cc.rdr != null))
                {
                    cc.rdr.Close();
                }
                if (cc.con.State == ConnectionState.Open)
                {
                    cc.con.Close();
                }
             }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    
    }
}
