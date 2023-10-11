namespace paf.api.Dtos.Identity_Dtos
{
    public record UserRegisterDto(string UserName, string Email,string Password, int Age, string Gender, string Phone, string PostalCode,string Role);
}
