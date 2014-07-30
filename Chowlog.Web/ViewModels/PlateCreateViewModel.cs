using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Chowlog.Web.ViewModels
{
    public class PlateCreateViewModel
    {
        [Key]
        public Guid Id { get; set; }
        public String Title { get; set; }

        public DateTime? TimeEaten { get; set; }

        [Required]
        public HttpPostedFileBase[] files { get; set; }
    }
}