using System;
using System.Data;
using System.Windows.Forms;
using ContactsApp_ModulesLayer;
using ContactsApp_BusinessLayer;
using System.Drawing;



namespace ContactsApp_NTierArchitecture
{
    public partial class ContactsApp : Form
    {
        public ContactsApp()
        {
            InitializeComponent();
        }


        ClsContact _Contact = null;
        int _ID = -1;

        void FillContact(ClsContact Contact, DateTime dateTime)
        {
            Contact.FirstName = txtFN.Text.Trim();

            Contact.LastName = txtLN.Text.Trim();

            Contact.Email = txtEM.Text.Trim();

            Contact.Phone = mtxtPh.Text.Trim();

            Contact.Address = txtAd.Text.Trim();

            Contact.DateOfBirth = dateTime;
           
            Contact.Country = txtCN.Text.Trim();

            if (string.IsNullOrWhiteSpace(Contact.ImagePath))
                Contact.ImagePath = "";
        }

        private void btAddContact_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtFN.Text) && !string.IsNullOrWhiteSpace(txtLN.Text) && !string.IsNullOrWhiteSpace(txtEM.Text) &&
                !string.IsNullOrWhiteSpace(txtAd.Text) && !string.IsNullOrWhiteSpace(mtxtPh.Text) && !string.IsNullOrWhiteSpace(mtxtDOB.Text) &&
                !string.IsNullOrWhiteSpace(txtCN.Text)
                )
            {
                DateTime result;
                
                if (!DateTime.TryParse(mtxtDOB.Text, out result))
                {
                    MessageBox.Show("Invalid Date Of Birth.");
                    return;
                }

                try
                {
                    if (_Contact == null)
                    {
                        _Contact = new ClsContact();
                    }

                    FillContact(_Contact, result);

                    if (ClsCRUD_Operations.AddContact(_Contact) == true)
                        MessageBox.Show("Contact Adding Successfully.");
                    else
                        MessageBox.Show("Contact Adding Faild.");

                }
                catch
                {
                    MessageBox.Show("Something went wrong.\nPlease contact us.");
                }
            }
            else
            {
                MessageBox.Show("Enter The Requirements.");
            }
        }

        void SetContactInfo(PictureBox pictureBox ,Label LabelID ,Label FN, Label LN, Label EM, Label PH, Label AD, Label CN, Label DOB, ClsContact contact, int ID)
        {
            LabelID.Text = ID.ToString();
            FN.Text = contact.FirstName;
            LN.Text = contact.LastName;
            EM.Text = contact.Email;
            PH.Text = contact.Phone;
            AD.Text = contact.Address;
            CN.Text = contact.Country;
            DOB.Text = contact.DateOfBirth.ToShortDateString();

            if (string.IsNullOrWhiteSpace(contact.ImagePath))
            {
                pictureBox.Image = null;
            }
            else
            {
                pictureBox.Image = Image.FromFile(contact.ImagePath);
            }
        }

        private void btSearchContact_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbFindSearchFN.Text) && !string.IsNullOrWhiteSpace(tbFindSearchLN.Text))
            {
                int ID = -1;

                try
                {
                    ClsContact contact = ClsCRUD_Operations.Find(tbFindSearchFN.Text, tbFindSearchLN.Text, ref ID);

                    if (contact != null && ID > -1)
                    {
                        SetContactInfo(pictureBox3 ,lFindResultID ,lFindResultFN, lFindResultLN, lFindResultEM, lFindResultPH, lFindResultAD, lFindResultCn, lFindResultDOB ,contact, ID);
                    }
                    else
                    {
                        MessageBox.Show("Contact Not Found.");
                    }
                }
                catch
                {
                    MessageBox.Show("Something went wrong.\nPlease contact us.");
                }
            }
            else
            {
                MessageBox.Show("Enter The Requirements.");
            }
        }

        bool UpdateContact(ClsContact contact)
        {
            contact.FirstName = tbUpdateFN.Text.Trim();
            contact.LastName = tbUpdateLN.Text.Trim();
            contact.Email = tbUpdateEM.Text.Trim();
            contact.Phone = mtbUpdatePH.Text.Trim();
            contact.Address = tbUpdateAD.Text.Trim();
            contact.Country = tbUpdateCN.Text.Trim();

            DateTime result;

            if (DateTime.TryParse(mtbUpdateDOB.Text, out result))
            {
                contact.DateOfBirth = result;

                return true;
            }
            else
            {
                MessageBox.Show("Invalid Date Of Birth.");

                return false;
            }
        }

        void ShowContactToUpdate(ClsContact contact)
        {
            tbUpdateFN.Text = contact.FirstName;
            tbUpdateLN.Text = contact.LastName;
            tbUpdateEM.Text = contact.Email;
            mtbUpdatePH.Text = contact.Phone;
            tbUpdateAD.Text = contact.Address;
            tbUpdateCN.Text = contact.Country;
            mtbUpdateDOB.Text = contact.DateOfBirth.ToString("MM-dd-yyyy");

            if (!string.IsNullOrWhiteSpace(contact.ImagePath))
            {
                pictureBox2.Image = Image.FromFile(contact.ImagePath);
            }
            else
            {
                pictureBox2.Image = null;
            }
        }

        private void btSearchToUpdate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbUpdateSearchFN.Text) && !string.IsNullOrWhiteSpace(tbUpdateSearchLN.Text))
            {
                try
                {
                    _Contact = ClsCRUD_Operations.Find(tbUpdateSearchFN.Text, tbUpdateSearchLN.Text, ref _ID);

                    if (_Contact != null && _ID > 0)
                    {
                        ShowContactToUpdate(_Contact);
                    }
                    else
                    {
                        MessageBox.Show("Contact Not Found.");
                    }
                }
                catch
                {
                    MessageBox.Show("Something went wrong.\nPlease contact us.");
                }
            }
            else
            {
                MessageBox.Show("Enter The Requirements.");
            }
        }

        private void btUpdateContact_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbUpdateSearchFN.Text) && !string.IsNullOrWhiteSpace(tbUpdateSearchLN.Text))
            {
                if (_Contact != null && _ID > 0)
                {
                    if (UpdateContact(_Contact))
                    {
                        try
                        {
                            if (ClsCRUD_Operations.Update(_Contact, _ID))
                            {
                                MessageBox.Show("Contact Updated Successfully.");
                            }
                            else
                            {
                                MessageBox.Show("Contact Updated Faild.");
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Something went wrong.\nPlease contact us.");
                        }
                        finally
                        {
                            _Contact = null;
                            _ID = -1;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Find Contact First.");
                }
            }
            else
            {
                MessageBox.Show("Enter The Requirements.");
            }
        }

        private void btDeleteContact_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbDeleteSearchFN.Text) && !string.IsNullOrWhiteSpace(tbDeleteSearchLN.Text))
            {
                int ID = -1;

                try
                {
                    ClsContact contact = ClsCRUD_Operations.Find(tbDeleteSearchFN.Text, tbDeleteSearchLN.Text, ref ID);

                    if (contact != null && ID > 0)
                    {
                        SetContactInfo(pictureBox4 ,llDeleteResultID, lDeleteResultFN, lDeleteResultLN, lDeleteResultEM, lDeleteResultPH, lDeleteResultAD,
                            lDeleteResultC, lDeleteResultDOB, contact, ID);

                        
                        if (MessageBox.Show("Are you sure delete this contact ?", "Deletion",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            if (ClsCRUD_Operations.Delete(ID))
                            {
                                MessageBox.Show("Contact Deleted Successfully.");
                            }
                            else
                            {
                                MessageBox.Show("Contact Deleted Faild.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Contact Not Deleted.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Contact Not Found.");
                    }
                }
                catch
                {
                    MessageBox.Show("Something went wrong.\nPlease contact us.");
                }
            }
            else
            {
                MessageBox.Show("Enter The Requirements.");
            }
        }

        private void btIsExistContact_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbFNExist.Text) && !string.IsNullOrWhiteSpace(tbLNExist.Text))
            {
                try
                {
                    if (ClsCRUD_Operations.IsExist(tbFNExist.Text, tbLNExist.Text))
                    {
                       MessageBox.Show("Contact Exist.");
                    }
                    else
                    {
                        MessageBox.Show("Contact Not Exist.");
                    }
                }
                catch
                {
                    MessageBox.Show("Something went wrong.\nPlease contact us.");
                }
            }
            else
            {
                MessageBox.Show("Enter The Requirements.");
            }
        }

        private void btShowAllContacts_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dataTable = ClsCRUD_Operations.GetAllContacts();

                if (dataTable.Rows.Count > 0)
                {
                    lvContacts.Items.Clear();

                    foreach (DataRow row in dataTable.Rows)
                    {
                        ListViewItem item = new ListViewItem();

                        item.Text = row["ContactID"].ToString();

                        item.SubItems.Add(row["FirstName"].ToString());
                        item.SubItems.Add(row["LastName"].ToString());
                        item.SubItems.Add(row["Email"].ToString());
                        item.SubItems.Add(row["Phone"].ToString());
                        item.SubItems.Add(row["Address"].ToString());
                        item.SubItems.Add(row["DateOfBirth"].ToString());
                        item.SubItems.Add(row["CountryName"].ToString());
                        item.SubItems.Add(row["ImagePath"].ToString());

                        lvContacts.Items.Add(item);
                    }
                }
                else
                {
                    MessageBox.Show("Not Contacts Found.");
                }
            }
            catch
            {
                MessageBox.Show("Something went wrong.\nPlease contact us.");
            }
        }

        void FillCountry(ClsCountry country)
        {
            country.CountryName = tbCountryName.Text.Trim();

            country.PhoneCode = mtbPhoneCode.Text.Trim();

            country.Code = mtbCode.Text.Trim();
        }

        private void btAddCountry_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbCountryName.Text))
            {
                if (string.IsNullOrWhiteSpace(mtbPhoneCode.Text))
                {
                    mtbPhoneCode.Text = "";
                }

                if (string.IsNullOrWhiteSpace(mtbCode.Text))
                {
                    mtbCode.Text = "";
                }

                ClsCountry country = new ClsCountry();

                FillCountry(country);

                try
                {
                    if (ClsCRUD_Operations.AddCountry(country) == true)
                        MessageBox.Show("Country Adding Successfully.");
                    else
                        MessageBox.Show("Country Adding Faild.");
                }
                catch
                {
                    MessageBox.Show("Something went wrong.\nPlease contact us.");
                }
            }
            else
            {
                MessageBox.Show("Enter The Requirements.");
            }
        }


        //=========================
        
        ClsCountry _Country = null;
        int _CountryID = -1;

        //=========================

        void SetCountryInfo(ClsCountry Country)
        {
            tbResultUpdateCountryName.Text = Country.CountryName;
            mtbResultUpdatePhoneCode.Text = Country.PhoneCode;
            mtbResultUpdateCode.Text = Country.Code;
        }

        private void btSearchToUpdateCountry_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbUpdateCountryNameSearch.Text))
            {
                try
                {
                    _Country = ClsCRUD_Operations.FindCountry(tbUpdateCountryNameSearch.Text, ref _CountryID);

                    if (_Country != null && _CountryID > 0)
                    {
                        SetCountryInfo(_Country);
                    }
                    else
                    {
                        MessageBox.Show("Country Not Found.");
                    }
                }
                catch
                {
                    MessageBox.Show("Something went wrong.\nPlease contact us.");
                }
            }
            else
            {
                MessageBox.Show("Enter The Requirements.");
            }
        }

        void UpdateCountry(ClsCountry Country)
        {
            Country.CountryName = tbResultUpdateCountryName.Text;
            Country.PhoneCode = mtbResultUpdatePhoneCode.Text;
            Country.Code = mtbResultUpdateCode.Text;
        }

        private void btUpdateCountry_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbUpdateCountryNameSearch.Text))
            {
                try
                {
                    if (_Country != null && _CountryID > 0)
                    {
                        UpdateCountry(_Country);

                        if (ClsCRUD_Operations.UpdateCountry(_Country, _CountryID))
                        {
                            MessageBox.Show("Country Updated Successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Country Updated Faild.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Find Country First.");
                    }
                }
                catch
                {
                    MessageBox.Show("Something went wrong.\nPlease contact us.");
                }
                finally
                {
                    _Country = null;
                    _CountryID = -1;
                }
            }
            else
            {
                MessageBox.Show("Enter The Requirements.");
            }
        }

        void ShowCountry(ClsCountry Country, int CountryID)
        {
            lFindResultCountryID.Text = CountryID.ToString();
            lFindResultCountryName.Text = Country.CountryName;
            lFindResultCode.Text = Country.Code;
            lFindResultPhoneCode.Text = Country.PhoneCode;
        }

        private void btSearchCountry_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbCountryNameFind.Text))
            {
                int CountryID = -1;

                try
                {
                    ClsCountry Country = ClsCRUD_Operations.FindCountry(tbCountryNameFind.Text, ref CountryID);

                    if (Country != null && CountryID > 0)
                    {
                        ShowCountry(Country, CountryID);
                    }
                    else
                    {
                        MessageBox.Show("Country Not Found.");
                    }
                }
                catch
                {
                    MessageBox.Show("Something went wrong.\nPlease contact us.");
                }
            }
            else
            {
                MessageBox.Show("Enter The Requirements.");
            }
        }

        private void btIsExistCountry_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbCountryNameExist.Text))
            {
                try
                {
                    if (ClsCRUD_Operations.IsCountryExist(tbCountryNameExist.Text))
                    {
                       MessageBox.Show("Country Exist.");
                    }
                    else
                    {
                        MessageBox.Show("Country Not Exist.");
                    }
                }
                catch
                {
                    MessageBox.Show("Something went wrong.\nPlease contact us.");
                }
            }
            else
            {
                MessageBox.Show("Enter The Requirements.");
            }
        }

        private void btShowAllCountries_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dataTable = ClsCRUD_Operations.GetAllCountries();

                if (dataTable.Rows.Count > 0)
                {
                    lvCountries.Items.Clear();

                    foreach (DataRow row in dataTable.Rows)
                    {
                        ListViewItem item = new ListViewItem(row["CountryID"].ToString());

                        item.SubItems.Add(row["CountryName"].ToString());
                        item.SubItems.Add(row["PhoneCode"].ToString());
                        item.SubItems.Add(row["Code"].ToString());

                        lvCountries.Items.Add(item);
                    }
                }
                else
                {
                    MessageBox.Show("Country Not Found.");
                }
            }
            catch
            {
                MessageBox.Show("Something went wrong.\nPlease contact us.");
            }
        }

        private void btSetImage_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image =  Image.FromFile(openFileDialog1.FileName);

                if (_Contact == null)
                {
                    _Contact = new ClsContact();

                    _Contact.ImagePath = openFileDialog1.FileName;
                }
            }
        }

        private void btChangeImage_Click(object sender, EventArgs e)
        {
            if (_Contact != null)
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    pictureBox2.Image = Image.FromFile(openFileDialog1.FileName);

                    _Contact.ImagePath = openFileDialog1.FileName;
                }
                else
                {
                    MessageBox.Show("Image Not Changed.");
                }
            }
            else
            {
                MessageBox.Show("Find Contact First.");
            }
        }

        private void btDeleteImage_Click(object sender, EventArgs e)
        {
            if (_Contact != null)
            {
                pictureBox2.Image = null;

                _Contact.ImagePath = "";
            }
            else
            {
                MessageBox.Show("Find Contact First.");
            }
        }














    }
}
