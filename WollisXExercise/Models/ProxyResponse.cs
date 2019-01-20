using System;
namespace WollisXExercise.Models
{
    public class ProxyResponse<T>
    {
        public T Content { get; set; }

        public string ErrorMessage { get; set; }
    }
}
