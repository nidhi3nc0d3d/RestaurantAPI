using System;
using System.Collections.Generic;

namespace RestaurantAPI.Models
{
    public partial class User
    {
        public string UserId { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
