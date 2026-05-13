



namespace ContactsApp_ModulesLayer
{
    public class ClsCountry
    {
        public string CountryName { get; set; }
        public string PhoneCode { get; set; }
        public string Code { get; set; }

        public bool IsFull()
        {
            return !string.IsNullOrWhiteSpace(CountryName);
        }





    }
}
