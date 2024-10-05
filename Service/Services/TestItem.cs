using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class TestItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        [Range(0, 100)]
        public int Quantity { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public bool IsActive { get; set; }

        public TestItem()
        {
            CreatedDate = DateTime.UtcNow;
            IsActive = true;
        }
    }
}
