using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace YangdoDTO
{
    [Table("People", Schema = "dbo")]
    public class Person
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PersonId { get; set; }

        [Required, Column(TypeName = "varchar(50)")]
        public string FirstName { get; set; }

        [Required, Column(TypeName = "varchar(50)")]
        public string LastName { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return LastName + " " + FirstName;
            }
        }

        [Required, EmailAddress, Column(TypeName = "varchar(50)")]
        public string Email { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime DOB { get; set; }

        [Column(TypeName ="varchar(30)")]
        public string Phone { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? RegisterDate { get; set; }

        #region Navigation Properties

        public IEnumerable<TimeSheet> TimeSheets { get; set; }

        #endregion

    }
}
