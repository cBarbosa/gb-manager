using System;

namespace gb_manager.Domain.Shared.Command
{
    public class CreatePersonCommand
    {
        public Guid? RecordId { get; set; }
        public string Document { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        // Endereço
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string FederativeUnit { get; set; }
        public string Complement { get; set; }

        public string Profile { get; set; }
        public bool Active { get; set; }
    }
}