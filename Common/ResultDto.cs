using System;

namespace Common
{
    public class ResultDto
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
    }

    public class ResultDto<T>
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public T data { get; set; }
    }
}
