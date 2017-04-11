using System;
using System.Net;

namespace ControliD
{
    public class cidException : Exception 
    {
        public string jsonRequest { get; private set; }
        public string jsonResult { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }
        public ErroCodes ErroCode { get; private set; }

        public cidException(ErroCodes erro, string cErro)
            :base(cErro)
        {
            ErroCode = erro;
        }

        public cidException(ErroCodes erro, Exception inner, string req, string res, HttpStatusCode code)
            : base(erro.ToString() +": " + inner.Message, inner)
        {
            ErroCode = erro;
            jsonRequest = req ?? "?";
            jsonResult = res ?? "?";
            StatusCode = code;
        }

        public cidException(string cErro, ErroCodes erro, Exception inner, string req, string res, HttpStatusCode code)
            : base(cErro, inner)
        {
            ErroCode = erro;
            jsonRequest = req ?? "?";
            jsonResult = res ?? "?";
            StatusCode = code;
        }
    }
}
