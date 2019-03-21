﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace XWidget.Web.SSO {
    public static class SsoMiddlewareExtension {
        public static IApplicationBuilder UseSSO(
            this IApplicationBuilder app,
            PathString pathMatch,
            Action<ISsoProvider, string, HttpContext> onLogin
            ) {
            return app.Use(async (context, next) => {
                if (!context.Request.Path.StartsWithSegments(pathMatch)) {
                    await next();
                }

                var providers = context.RequestServices.GetService<ISsoProvider[]>();

                foreach (var provider in providers) {
                    if (context.Request.Path.StartsWithSegments(pathMatch + "/" + provider.Name + "/login")) {
                        context.Response.Redirect(await provider.GetLoginUrlAsync(context));
                        return;
                    }
                    if (context.Request.Path.StartsWithSegments(pathMatch + "/" + provider.Name + "/login-callback")) {
                        if (!await provider.VerifyCallbackRequest(context)) {
                            context.Response.StatusCode = 400;
                            return;
                        }

                        var token = await provider.GetLoginCallbackTokenAsync(context);

                        if (token == null) {
                            context.Response.StatusCode = 400;
                            return;
                        }

                        if (await provider.VerifyTokenAsync(token)) {
                            onLogin(provider, token, context);
                            return;
                        } else {
                            context.Response.StatusCode = 400;
                            return;
                        }
                    }
                }

                await next();
            });
        }
    }
}
