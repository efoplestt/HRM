﻿using System;
using System.Windows.Forms;

namespace HRMapp
{
    public partial class LOGINform : Form
    {
        private bool flagUsername = false;
        private bool flagPassword = false;

        public LOGINform()
        {
            InitializeComponent();
        }

        private void loginform_Load(object sender, EventArgs e)
        {
            flagUsername = true;
            flagPassword = true;
            textBoxPassword.PasswordChar = '\0';
        }

        private enum AccessLevels
        {
            hr = 1,
            dm = 2,
            ceo = 3
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (HRMDBDataSet ds = new HRMDBDataSet())
            {
                using (HRMDBDataSetTableAdapters.UsersTableAdapter ta = new HRMDBDataSetTableAdapters.UsersTableAdapter())
                {
                    ta.FillByNameAndPassword(ds.Users, textBoxUsername.Text, textBoxPassword.Text);

                    if (ds.Users.Count == 0)
                    {
                        MessageBox.Show("Wrong input");
                        return;
                    }
                    else if (ds.Users.Count > 1)
                    {
                        MessageBox.Show("More than 1 users with same Username and Password, please check the Database");
                        return;
                    }
                    else
                    {
                        HRMDBDataSet.UsersRow userRow = ds.Users.Rows[0] as HRMDBDataSet.UsersRow;
                        int usersProfessionID = userRow.ProfessionID;

                        switch (usersProfessionID)
                        {
                            case (int)AccessLevels.hr:
                                HRform f = new HRform(userRow);
                                this.Hide();
                                f.Show();
                                break;

                            case (int)AccessLevels.dm:
                                //dm login
                                DMform dmf = new DMform(userRow);
                                this.Hide();
                                dmf.Show();
                                break;

                            case (int)AccessLevels.ceo:
                                //ceo login
                                CEOform ceof = new CEOform();
                                this.Hide();
                                ceof.Show();
                                break;

                            default:
                                MessageBox.Show("There was a problem on button1_Click function!!");
                                break;
                        }
                    }
                }
            }
        }

        private void ClickAndHide(TextBox txb, ref bool flag)
        {
            try
            {
                if (flag == true)
                {
                    txb.Text = String.Empty;
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxUsername_Leave(object sender, EventArgs e)
        {
            if (textBoxUsername.Text == String.Empty)
            {
                textBoxUsername.Text = "Username";
                flagUsername = true;
            }
        }

        private void textBoxPassword_Leave(object sender, EventArgs e)
        {
            if (textBoxPassword.Text == String.Empty)
            {
                textBoxPassword.PasswordChar = '\0';
                textBoxPassword.Text = "Password";
                flagPassword = true;
            }
        }

        private void textBoxUsername_Enter(object sender, EventArgs e)
        {
            ClickAndHide(this.textBoxUsername, ref flagUsername);
        }

        private void textBoxPassword_Enter(object sender, EventArgs e)
        {
            ClickAndHide(this.textBoxPassword, ref flagPassword);
            textBoxPassword.PasswordChar = '*';
        }
    }
}