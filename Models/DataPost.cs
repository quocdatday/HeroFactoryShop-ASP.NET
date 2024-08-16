namespace ShopOnline.Models
{
    public class DataPost
    {
        public int ID { get; set; }

        //User
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Role { get; set; }
        //Category
        //public string? Name { get; set; }
        //Product 
        //public int CATID { get; set; }
        //public IFormFile? Image { get; set; }
        //public string? Title { get; set; }
        //public string? Subtitle { get; set; }
        //public decimal Price { get; set; }
        //Setting
        public string? Value { get; set; }
    }
}
