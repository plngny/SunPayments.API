﻿using Microsoft.AspNetCore.Diagnostics;
using SunPayments.API.DTOs;
using System.Text.Json;

namespace SunPayments.API.Exceptions
{
    public static class CustomExceptionHandler
    {
        public static void UseCustomException(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(config =>
            {

                // .run metotu ile bir middleware başka bir middleware e geçmez.
                config.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (errorFeature != null)
                    {
                        var ex = errorFeature.Error;

                        ErrorDto errorDto = null;

                        if (ex is CustomException)
                        {
                            errorDto = new ErrorDto(ex.Message, true);
                        }
                        else
                        {
                            errorDto = new ErrorDto(ex.Message, false);
                        }

                        var response = ExceptionResponseDto<NoDataDto>.Fail(errorDto,500);

                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    }
                });
            });
        }
    }
}
