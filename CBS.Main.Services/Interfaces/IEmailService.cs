using CBS.Dto.ViewModel;


namespace CBS.Service.Interfaces
{
    public interface IEmailService
    {
        bool SendCredentialToEmail(UserDto user, string emailtemplate);
        bool SendForgorPasswordRequestToEmail(UserDto user, string emailtemplate, string callbackurl);
    }
}
