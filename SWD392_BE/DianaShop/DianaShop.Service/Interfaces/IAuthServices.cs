using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;

namespace DianaShop.Service.Interfaces
{
    public interface IAuthServices
    {
        Task<BaseResponse<TokenModel>> AdminGenAcc(AdminCreateAccountModel adminCreateAccountModel);
        Task<BaseResponseForLogin<LoginResponseModel>> AuthenticateAsync(string email, string password);
        Task<BaseResponse> ForgotPassword(ForgotPasswordRequest request);
        string GenerateJwtToken(string username, string roleName, int userId);
        string GeneratePassword();
        Task<BaseResponse<TokenModel>> RegisterAsync(RegisterModel registerModel);
        Task<BaseResponse> SendAccount(int userId);
    }
}