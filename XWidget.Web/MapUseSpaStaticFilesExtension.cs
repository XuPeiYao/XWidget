﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace XWidget.Web {
    public static class MapUseSpaStaticFilesExtension {
        /// <summary>
        /// 在指定路由使用指定檔案來源的SPA
        /// </summary>
        /// <param name="app">應用程式建構器</param>
        /// <param name="pathMatch">子路由</param>
        /// <param name="options">靜態檔案選項</param>
        /// <param name="configuration">子路由之應用程式建構流程</param>
        /// <returns>應用程式建構器</returns>
        public static IApplicationBuilder MapUseSpaStaticFiles(
            this IApplicationBuilder app,
            PathString pathMatch,
            StaticFileOptions options,
            Action<IApplicationBuilder> configuration = null
            ) {
            return app.Map(pathMatch, app2 => {
                configuration?.Invoke(app2);

                // 優先至實體檔案尋找指定路徑
                app2.UseStaticFiles(options);

                // 找不到則跳轉到SPA處理
                app2.UseSpa(spa => {
                    spa.Options.DefaultPageStaticFileOptions = options;
                });
            });
        }

        /// <summary>
        /// 在指令路由使用指定檔案來源的SPA且自動修正index.html中的Base元素
        /// </summary>
        /// <param name="app">應用程式建構器</param>
        /// <param name="pathMatch">子路由</param>
        /// <param name="options">靜態檔案選項</param>
        /// <param name="configuration">子路由之應用程式建構流程</param>
        /// <returns>應用程式建構器</returns>
        public static IApplicationBuilder MapUseSpaStaticFilesWithFixBaseHref(
            this IApplicationBuilder app,
            PathString pathMatch,
            StaticFileOptions options,
            Action<IApplicationBuilder> configuration = null
            ) {
            return app.MapUseSpaStaticFiles(
                pathMatch,
                options,
                app2 => {
                    app2.UseHtmlHandler(async (context, html) => {
                        // 剖析HTML
                        HtmlDocument htmlDoc = new HtmlDocument();
                        htmlDoc.LoadHtml(html);

                        // 取得BaseElement並設定href
                        var baseNode = htmlDoc.DocumentNode.SelectSingleNode("//base");
                        if (baseNode != null) {
                            baseNode.SetAttributeValue("href", pathMatch + "/");
                        }

                        return htmlDoc.DocumentNode.OuterHtml;
                    });

                    configuration?.Invoke(app2);
                });
        }
    }
}
