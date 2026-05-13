using System.Data;
using ContactsApp_DataAccessLayer;
using ContactsApp_ModulesLayer;



namespace ContactsApp_BusinessLayer
{
    public class ClsCRUD_Operations
    {
        static public bool AddContact(ClsContact contact)
        {
            if (contact.IsFull())
            {
                return ClsDataHandling.AddNewContact(contact);
            }

            return false;
        }

        static public ClsContact Find(string FirstName, string LastName, ref int ID)
        {
            return ClsDataHandling.FindContact(FirstName, LastName, ref ID);
        }

        static public bool Update(ClsContact Contact, int ID)
        {
            if (Contact.IsFull() && ID > 0)
            {
                return ClsDataHandling.UpdateContact(Contact, ID);
            }

            return false;
        }

        static public bool Delete(int ID)
        {
            if (ID > 0)
            {
                return ClsDataHandling.DeleteContact(ID);
            }

            return false;
        }

        static public DataTable GetAllContacts()
        {
            return ClsDataHandling.GetAllContacts();
        }

        static public bool IsExist(string FirstName, string LastName)
        {
            return ClsDataHandling.IsContactExist(FirstName, LastName);
        }

        static public ClsCountry FindCountry(string CountryName, ref int CountryID)
        {
            return ClsDataHandling.FindCountry(CountryName, ref CountryID);
        }

        static public bool IsCountryExist(string CountryName)
        {
            return ClsDataHandling.IsCountryExist(CountryName);
        }

        static public bool AddCountry(ClsCountry Country)
        {
            if (Country.IsFull())
            {
                return ClsDataHandling.AddNewCountry(Country);
            }

            return false;
        }

        static public DataTable GetAllCountries()
        {
            return ClsDataHandling.GetAllCountries();
        }

        static public bool UpdateCountry(ClsCountry Country, int CountryID)
        {
            if (Country.IsFull() && CountryID > 0)
            {
                return ClsDataHandling.UpdateCountry(Country, CountryID);
            }

            return false;
        }






    }
}
