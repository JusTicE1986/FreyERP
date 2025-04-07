using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreyERP.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Vorname { get; set; }
        [Required]
        public string Nachname { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string Staße { get; set; }
        public int Hausnummer { get; set; }
        public int PLZ { get; set; }
        public string Ort { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public string FullName => $"{Vorname} {Nachname}";
    }
}
