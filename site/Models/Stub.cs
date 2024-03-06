using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Site.Models
{
    public class Stub
    {
        public Stub() {}
        
        public Stub( int id ,string firstName, string lastName, DateTime birthdate)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Birthdate = birthdate;
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("First name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Last name")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Birthdate")]
        public DateTime Birthdate { get; set; }
        
    } 
}
