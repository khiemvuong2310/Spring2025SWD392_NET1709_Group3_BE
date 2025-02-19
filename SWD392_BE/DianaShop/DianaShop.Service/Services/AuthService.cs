using DianaShop.Data.Entities;
using DianaShop.Repository.UnitOfWork;
using DianaShop.Repository.Utils;
using DianaShop.Service.Interfaces;
using DianaShop.Service.ReponseModel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DianaShop.Service.RequestModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace DianaShop.Service.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public AuthServices(IConfiguration configuration, IUserService userService, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponseForLogin<LoginResponseModel>> AuthenticateAsync(string email, string password)
        {
            var user = await _userService.GetUserByEmailAsync(email);

            if (user != null)
            {
                var userWithRole = await _userService.GetUserByUsernameAsync(user.Username);
                var roleNames = string.Join(",", userWithRole.UserRoles.Select(ur => ur.Roles));
                string token = GenerateJwtToken(user.Username, roleNames, user.Id);

                if (user.Status == false)
                {
                    return new BaseResponseForLogin<LoginResponseModel>()
                    {
                        Code = 404,
                        Message = "Your Account has been banned. Check email for reason",
                        Data = new LoginResponseModel()
                        {
                            User = new UserResponseModel()
                            {
                                Id = user.Id,
                                Username = user.Username,
                                Email = user.Email,
                                Location = user.Location,
                                Phone = user.Phone,
                            },
                        },
                        IsBanned = true,
                    };
                }

                return new BaseResponseForLogin<LoginResponseModel>()
                {
                    Code = 200,
                    Message = "",
                    Data = new LoginResponseModel()
                    {
                        Token = token,
                        User = new UserResponseModel()
                        {
                            Id = user.Id,
                            Username = user.Username,
                            Email = user.Email,
                            Location = user.Location,
                            Phone = user.Phone,
                        },
                    },
                    IsBanned = false
                };
            }
            return new BaseResponseForLogin<LoginResponseModel>()
            {
                Code = 404,
                Message = "Username or Password incorrect",
                Data = null,
                IsBanned = false
            };
        }

        public string GenerateJwtToken(string username, string roleName, int userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, roleName),
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<BaseResponse<TokenModel>> RegisterAsync(RegisterModel registerModel)
        {
            var existingUser = await _unitOfWork.Repository<User>().FindAsync(u => u.Email == registerModel.Email);

            if (existingUser != null)
            {
                return new BaseResponse<TokenModel>
                {
                    Code = 409,
                    Message = "Email already exists",
                };
            }

            var user = new User()
            {
                Location = registerModel.Location,
                Username = registerModel.Username,
                Email = registerModel.Email,
                Password = PasswordTools.HashPassword(registerModel.Password),
                Phone = registerModel.Phone,
                Status = true,
            };

            await _unitOfWork.Repository<User>().InsertAsync(user);
            await _unitOfWork.CommitAsync();

            var userWithRole = await _userService.GetUserByUsernameAsync(user.Username);
            var roleNames = string.Join(",", userWithRole.UserRoles.Select(ur => ur.Roles));
            string token = GenerateJwtToken(user.Username, roleNames, user.Id);

            return new BaseResponse<TokenModel>
            {
                Code = 201,
                Message = "Register successfully",
                Data = new TokenModel
                {
                    Token = token
                }
            };
        }

        public async Task<BaseResponse<TokenModel>> AdminGenAcc(AdminCreateAccountModel adminCreateAccountModel)
        {
            var existingUser = await _unitOfWork.Repository<User>().FindAsync(u => u.Email == adminCreateAccountModel.Email);

            if (existingUser != null)
            {
                return new BaseResponse<TokenModel>
                {
                    Code = 409,
                    Message = "Email already exists",
                };
            }

            var user = new User()
            {
                Location = adminCreateAccountModel.Location,
                Username = adminCreateAccountModel.Username,
                Email = adminCreateAccountModel.Email,
                Password = "01234",
                Phone = adminCreateAccountModel.Phone,
                Status = true,
            };

            await _unitOfWork.Repository<User>().InsertAsync(user);
            await _unitOfWork.CommitAsync();

            await SendAccount(user.Id);

            var userWithRole = await _userService.GetUserByUsernameAsync(user.Username);

            var roleNames = string.Join(",", userWithRole.UserRoles.Select(ur => ur.Roles)); //Note sửa


            string token = GenerateJwtToken(user.Username, roleNames, user.Id);

            return new BaseResponse<TokenModel>
            {
                Code = 201,
                Message = "Register successfully",
                Data = new TokenModel
                {
                    Token = token
                }
            };
        }

        public async Task<BaseResponse> SendAccount(int userId)
        {
            try
            {
                var user = await _unitOfWork.Repository<User>().GetById(userId);
                var providePassword = GeneratePassword();
                user.Password = PasswordTools.HashPassword(providePassword);
                await _unitOfWork.Repository<User>().Update(user, user.Id);
                await _unitOfWork.CommitAsync();

                var smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("studentexchangeweb@gmail.com", "fwpl wpkw zhqe peyh");

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("studentexchangeweb@gmail.com");
                mailMessage.To.Add(user.Email);
                mailMessage.Subject = "YOUR ENTRY ACCOUNT";

                mailMessage.Body = @"
<html>
<head>
  <style>
    body {
      font-family: Arial, sans-serif;
      line-height: 1.6;
    }
    .container {
      padding: 20px;
      background-color: #f4f4f4;
      border: 1px solid #ddd;
      border-radius: 5px;
      max-width: 600px;
      margin: 0 auto;
    }
    .header {
      font-size: 20px;
      font-weight: bold;
      text-align: center;
      margin-bottom: 20px;
    }
    .content {
      font-size: 16px;
      color: #333;
    }
    .footer {
      font-size: 12px;
      color: #888;
      text-align: center;
      margin-top: 20px;
    }
    .highlight {
      color: #007BFF;
      font-weight: bold;
    }
  </style>
</head>
<body>
  <div class='container'>
    <div class='header'>Welcome to our Exchange Web!</div>
    <div class='content'>
      <p>Email: <span class='highlight'>" + user.Email + @"</span></p>
      <p>Password: <span class='highlight'>" + providePassword + @"</span></p>
      <p>This is a temporary password. Please change your password after logging in.</p>
    </div>
    <div class='footer'>
      &copy; 2024 Exchange Web. All rights reserved.
    </div>
  </div>
</body>
</html>";

                mailMessage.IsBodyHtml = true;

                await smtpClient.SendMailAsync(mailMessage);

                return new BaseResponse
                {
                    Code = 200,
                    Message = "Send succeed."
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Code = 400,
                    Message = "An error occurred: " + ex.Message
                };
            }
        }

        public async Task<BaseResponse> ForgotPassword(RequestModel.ForgotPasswordRequest request)
        {
            try
            {
                var query = await _unitOfWork.Repository<User>()
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Email == request.Email);

                if (query == null || query.Email != request.Email)
                {
                    return new BaseResponse
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Message = "Email is not matched"
                    };
                }

                var providePassword = GeneratePassword();
                query.Password = PasswordTools.HashPassword(providePassword);

                var smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("studentexchangeweb@gmail.com", "fwpl wpkw zhqe peyh");

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("studentexchangeweb@gmail.com");
                mailMessage.To.Add(query.Email);
                mailMessage.Subject = "WEB EXCHANGE: YOUR RESET PASSWORD";

                mailMessage.Body = @"
<html>
<head>
  <style>
    body {
      font-family: Arial, sans-serif;
      line-height: 1.6;
    }
    .container {
      padding: 20px;
      background-color: #f4f4f4;
      border: 1px solid #ddd;
      border-radius: 5px;
      max-width: 600px;
      margin: 0 auto;
    }
    .header {
      font-size: 20px;
      font-weight: bold;
      text-align: center;
      margin-bottom: 20px;
    }
    .content {
      font-size: 16px;
      color: #333;
    }
    .footer {
      font-size: 12px;
      color: #888;
      text-align: center;
      margin-top: 20px;
    }
    .highlight {
      color: #007BFF;
      font-weight: bold;
    }
  </style>
</head>
<body>
  <div class='container'>
    <div class='header'>Password Reset Request</div>
    <div class='content'>
      <p>Hello <span class='highlight'>" + query.Username + @"</span>,</p>
      <p>Your reset password is: <span class='highlight'>" + providePassword + @"</span></p>
      <p>This is a temporary password. Please change your password after logging in.</p>
    </div>
    <div class='footer'>
      &copy; 2024 Exchange Web. All rights reserved.
    </div>
  </div>
</body>
</html>";

                mailMessage.IsBodyHtml = true;

                await smtpClient.SendMailAsync(mailMessage);
                await _unitOfWork.Repository<User>().Update(query, query.Id);
                await _unitOfWork.CommitAsync();

                return new BaseResponse
                {
                    Code = StatusCodes.Status200OK,
                    Message = "Your reset password has been sent."
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred: " + ex.Message
                };
            }
        }

        public string GeneratePassword()
        {
            string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";
            var bytes = new byte[8];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            var password = new string(bytes.Select(b => characters[b % characters.Length]).ToArray());
            return password;
        }
    }
}
