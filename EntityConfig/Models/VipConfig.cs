using System;
using System.Collections.Generic;

#nullable disable

namespace EntityConfig.Models
{
    public partial class VipConfig
    {
        public int Id { get; set; }
        public string VipName { get; set; }
        public int RequireVip { get; set; }
    }
}
