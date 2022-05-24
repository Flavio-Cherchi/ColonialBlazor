using Colonial.Shared.SharedModels.Generic;

namespace Colonial.Shared.SharedModels.Account
{
    public class User 
    {
        public int? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime? DateCreation { get; set; }
        public DateTime? DateModification { get; set; }
        public UserRole? UserRole { get; set; }
        public Image? Image { get; set; }
    }
}