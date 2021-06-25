namespace VisitPop.Application.Dtos.Employee
{
    public abstract class EmployeeForManipulationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DocId { get; set; }
        public string PhoneNumber { get; set; }
        public int EmployeeDepartmentId { get; set; }
        public string EmailAddress { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
