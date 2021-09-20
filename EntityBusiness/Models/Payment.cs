using System;
using System.Collections.Generic;

#nullable disable

namespace EntityBusiness.Models
{
    public partial class Payment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Money { get; set; }
        public double Time { get; set; }
    }
}
