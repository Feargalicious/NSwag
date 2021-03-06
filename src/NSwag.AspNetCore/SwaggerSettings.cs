//-----------------------------------------------------------------------
// <copyright file="SwaggerSettings.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/NSwag/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using System;
using Newtonsoft.Json;
using NSwag.SwaggerGeneration;
using NSwag.SwaggerGeneration.WebApi;

#if AspNetOwin
using Microsoft.Owin;

namespace NSwag.AspNet.Owin
#else
namespace NSwag.AspNetCore
#endif
{
    // TODO: Remove this class in v13, only used for legacy Web API middlewares

    /// <summary>The settings for UseSwagger.</summary>
    public class SwaggerSettings<T>
        where T : SwaggerGeneratorSettings, new()
    {
        /// <summary>Initializes a new instance of the <see cref="SwaggerSettings{T}"/> class.</summary>
        public SwaggerSettings()
        {
            GeneratorSettings = new T();

#if !AspNetOwin
            if (GeneratorSettings is WebApiToSwaggerGeneratorSettings)
                ((WebApiToSwaggerGeneratorSettings)(object)GeneratorSettings).IsAspNetCore = true;
#endif
        }

        /// <summary>Gets the generator settings.</summary>
#if !AspNetOwin
        [Obsolete("This property is ignored when used without OpenAPI/Swagger generator and will be removed eventually, change config in UseSwagger().")]
#endif
        public T GeneratorSettings { get; }

        /// <summary>Gets or sets the OWIN base path (when mapped via app.MapOwinPath()) (must start with '/').</summary>
#if !AspNetOwin
        [Obsolete("This property is ignored when using AspNetCoreToSwaggerGenerator and will be removed eventually.")]
#endif
        public string MiddlewareBasePath { get; set; }

#if !AspNetOwin
        /// <summary>Gets or sets the Swagger document route (must start with '/', default: '/swagger/{documentName}/swagger.json').</summary>
        /// <remarks>May contain '{documentName}' placeholder to register multiple routes.</remarks>
        public string DocumentPath { get; set; } = "/swagger/{documentName}/swagger.json";
#else
        /// <summary>Gets or sets the Swagger document route (must start with '/', default: '/swagger/v1/swagger.json').</summary>
        public string DocumentPath { get; set; } = "/swagger/v1/swagger.json";
#endif

        /// <summary>Gets or sets the Swagger post process action.</summary>
#if !AspNetOwin
        [Obsolete("This property is ignored when using AspNetCoreToSwaggerGenerator and will be removed eventually.")]
#endif
        public Action<SwaggerDocument> PostProcess { get; set; }

        /// <summary>Gets or sets for how long a <see cref="Exception"/> caught during schema generation is cached.</summary>
#if !AspNetOwin
        [Obsolete("This property is ignored when using AspNetCoreToSwaggerGenerator and will be removed eventually.")]
#endif
        public TimeSpan ExceptionCacheTime { get; set; } = TimeSpan.FromSeconds(10);

        internal virtual string ActualSwaggerDocumentPath => DocumentPath.Substring(MiddlewareBasePath?.Length ?? 0);

        internal T CreateGeneratorSettings(JsonSerializerSettings serializerSettings, object mvcOptions)
        {
            GeneratorSettings.ApplySettings(serializerSettings, mvcOptions);
            return GeneratorSettings;
        }
    }
}