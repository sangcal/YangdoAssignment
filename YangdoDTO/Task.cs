using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace YangdoDTO
{
    [Table("Tasks", Schema = "dbo")]
    public class Task
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskId { get; set; }

        [Required, Column(TypeName = "varchar(100)")]
        public string TaskName { get; set; }

        [Column(TypeName = "varchar(400)")]
        public string TaskDesc { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? RegisterDate { get; set; }

        #region Navigation Properties

        public IEnumerable<TimeSheet> TimeSheets { get; set; }

        #endregion
    }
}
