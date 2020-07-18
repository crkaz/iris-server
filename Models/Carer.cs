using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace iris_server.Models
{
    public class Carer
    {
        // DB fields.
        [Key]
        public string Email { get; set; } // Primary Key
        public virtual User User { get; set; } // Foreign Key.
        public IList<string> AssignedPatientIds { get; set; }

        public Carer() { }
    }
}
