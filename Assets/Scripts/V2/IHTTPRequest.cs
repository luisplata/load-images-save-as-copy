using System;
using System.Collections.Generic;

namespace V2
{
    public interface IHttpRequest
    {
        void CanInit(Action ok, Action error);

        void ImagineRequest(byte[] imageInBytes, string style, string profession, Action<List<string>> ok,
            Action error);
    }
}