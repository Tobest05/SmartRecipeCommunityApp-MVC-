using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    public class BaseResponse<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; } = default!;
        public T? Data { get; set; }

        private BaseResponse() { }
        public static BaseResponse<T> Success(string message, T data)
        {
            return new BaseResponse<T>
            {
                Status = true,
                Message = message,
                Data = data
            };

        }

        public static BaseResponse<T> Success(string message)
        {
            return new BaseResponse<T>
            {
                Status = true,
                Message = message
            };

        }

        public static BaseResponse<T> Failure(string message)
        {
            return new BaseResponse<T>
            {
                Status = false,
                Message = message,
            };

        }

    }

}

