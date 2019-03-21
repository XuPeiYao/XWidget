using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using Xunit;
using XWidget.Utilities;
using XWidget.Web.Jwt.Test.Models;

namespace XWidget.Web.Jwt.Test {
    public class TokenTest {
        [Fact]
        public void TokenSignAndVerify() {
            var token = new TestToken() {
                Header = new DefaultJwtHeader() {
                    Algorithm = SecurityAlgorithms.HmacSha256
                },
                Payload = new CommonPayload() {
                    Actor = "TestUser",
                    Audience = "TestAudience",
                    Issuer = "TestIssuer",
                    Subject = "TestTokens",
                    IssuedAt = DateTime.Now,
                    Expires = DateTime.Now
                }
            };

            var testKey = new SymmetricSecurityKey("TestKey".ToHash<MD5>());

            var tokenString = token.Sign(testKey);

            var isVaild = JwtTokenConvert.Verify<CommonPayload>(
                tokenString, testKey, out var verifyToken);

            Assert.True(isVaild);

            Assert.Equal(JObject.FromObject(token), JObject.FromObject(verifyToken));
        }

        [Fact]
        public void TokenSignAndVerify2() {
            var token = new TestToken() {
                Header = new DefaultJwtHeader() {
                    Algorithm = SecurityAlgorithms.HmacSha256
                },
                Payload = new CommonPayload() {
                    Actor = "TestUser",
                    Audience = "TestAudience",
                    Issuer = "TestIssuer",
                    Subject = "TestTokens",
                    IssuedAt = DateTime.Now,
                    Expires = DateTime.Now
                }
            };

            var testKey = new SymmetricSecurityKey("TestKey".ToHash<MD5>());

            var tokenString = token.Sign(testKey);

            Thread.Sleep(5000);

            var isVaild = JwtTokenConvert.Verify<TestToken, DefaultJwtHeader, CommonPayload>(tokenString,
                new TokenValidationParameters() {
                    IssuerSigningKey = testKey,
                    ValidIssuer = "TestIssuer", // ���Ҫ��o���
                    ValidAudience = "TestAudience", // ���Ҫ�TOKEN������

                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true, // �ˬdTOKEN�o���
                    ValidateAudience = true, // �ˬd��TOKEN�O�_�o�����A��
                    ValidateLifetime = true, // �ˬdTOKEN�O�_����
                    ClockSkew = TimeSpan.Zero
                },
                out TestToken tokenOut,
                out Exception e);

            Assert.False(isVaild);
            Assert.NotNull(e);
        }
    }
}
