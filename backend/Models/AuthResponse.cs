using System.Collections.Generic;

namespace backend.Models
{
    public class AuthResponse
    {
        public UserProfile Profile { get; set; } = new();
        public List<MenuItem> Menu { get; set; } = new();
    }
}
