using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace YangdoDTO
{
    [Table("TimeSheets", Schema = "dbo")]
    public class TimeSheet
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TimeSheetId { get; set; }

        [ForeignKey("Person")]
        public int PersonId { get; set; }

        [ForeignKey("Task")]
        public int TaskId { get; set; }

        [Required, Column(TypeName = "datetime")]
        public DateTime TimeFrom { get; set; }

        [Required, Column(TypeName = "datetime")]
        public DateTime TimeTo { get; set; }

        [Required, Column(TypeName = "float")]
        public double WorkedHours { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? RegisterDate { get; set; }


        #region Navigation Properties

        [NotMapped]
        public Person Person { get; set; }

        [NotMapped]
        public Task Task { get; set; }

        #endregion   
    }
}
