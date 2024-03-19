using Indo.Departments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Indo.Skills
{
    public class SkillUpdateDto
    {

        [Required]
        [StringLength(SkillConsts.MaxNameLength)]
        public string Name { get; set; }
    }
}
