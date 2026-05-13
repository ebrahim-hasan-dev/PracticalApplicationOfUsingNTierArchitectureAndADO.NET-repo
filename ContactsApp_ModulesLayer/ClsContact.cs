using System;



namespace ContactsApp_ModulesLayer
{
    public class ClsContact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Country { get; set; }
        public string ImagePath { get; set; }

        public bool IsFull()
        {
            return !string.IsNullOrWhiteSpace(this.FirstName) && !string.IsNullOrWhiteSpace(this.LastName) && !string.IsNullOrWhiteSpace(this.Email) &&
                !string.IsNullOrWhiteSpace(this.Phone) && !string.IsNullOrWhiteSpace(this.Address) && !string.IsNullOrWhiteSpace(this.Country) &&
                this.DateOfBirth != default(DateTime) ;
        }







    }
}
