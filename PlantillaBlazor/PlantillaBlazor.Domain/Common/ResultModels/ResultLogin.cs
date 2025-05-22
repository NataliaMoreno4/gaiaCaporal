using PlantillaBlazor.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Domain.Common.ResultModels
{
    public record class ResultLogin<T>
    {
        public T Value { get; set; }
        public bool IsSuccess { get; set; }
        public string Error { get; set; }
        public TipoRespuestaLogin RespuestaLogin { get; set; }

        private ResultLogin(T value, bool isSuccess, string error, TipoRespuestaLogin respuestaLogin)
        {
            Value = value;
            IsSuccess = isSuccess;
            Error = error;
            RespuestaLogin = respuestaLogin;
        }


        public static ResultLogin<T> Success(T value, TipoRespuestaLogin respuestaLogin) => new ResultLogin<T>(value, true, null, respuestaLogin);
        public static ResultLogin<T> Failure(string error) => new ResultLogin<T>(default, false, error, default);
    }
}
