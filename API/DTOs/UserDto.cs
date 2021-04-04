namespace API.DTOs
{
    public class UserDto
    {
        public string Username { get; set; }   
        public string Token { get; set; }      
    }
}

//This is the object that we going to return when the user logs in or registers.