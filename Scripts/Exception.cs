using System;

namespace EzMidi
{
    public class Exception : System.Exception
    {
        public Exception(string message)
                : base(message)
        {
        }
    }
}