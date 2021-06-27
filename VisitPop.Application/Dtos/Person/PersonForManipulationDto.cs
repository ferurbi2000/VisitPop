namespace VisitPop.Application.Dtos.Person
{
    public abstract class PersonForManipulationDto
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DocId { get; set; }

        public string PhoneNumber { get; set; }

        public int PersonTypeId { get; set; }

        public int CompanyId { get; set; }

        public string EmailAddress { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
