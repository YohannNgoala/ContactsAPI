using ContactsApi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ContactsAPI.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly DatabaseContext _context;
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            DatabaseContext context)
            : base(options, logger, encoder, clock)
        {
            _context = context;
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Authorization header was not found");
            try
            {
                var authentificationHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var bytes = Convert.FromBase64String(AddPadding(authentificationHeaderValue.Parameter));
                string[] credentials = Encoding.UTF8.GetString(bytes).Split(":");
                string username = credentials[0];
                string pwd = credentials[1];

                UserModel user = _context.Users.Where(user => user.UserName == username && user.Password == pwd).FirstOrDefault();
                if (user == null)
                    AuthenticateResult.Fail("Invalid username or password");
                else
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, user.UserName) };
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);

                    return AuthenticateResult.Success(ticket);
                }
            }
            catch (Exception)
            {
                return AuthenticateResult.Fail("");
            }

            return AuthenticateResult.Fail("");
        }
        //Add Padding to have a valid 64 string
        private string  AddPadding(string s)
        {           
            s = s.PadRight(s.Length + (s.Length * 3) % 4, '=');  // add padding
            return s;
        }
    }
}
