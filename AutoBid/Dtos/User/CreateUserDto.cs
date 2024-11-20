namespace AutoBid.Dtos.User
{
    public class CreateUserDto
    {
        public int UserId { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
