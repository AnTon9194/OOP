using System;
using System.Collections.Generic;

namespace lab19v4
{
    public class ValidationException : Exception
    {
        public List<string> Errors { get; }

        public ValidationException() 
            : base("Виникла помилка валідації даних.")
        {
            Errors = new List<string>();
        }

        public ValidationException(string message) 
            : base(message)
        {
            Errors = new List<string> { message };
        }

        public ValidationException(string message, Exception innerException) 
            : base(message, innerException)
        {
            Errors = new List<string> { message };
        }

        public ValidationException(List<string> errors) 
            : base("Об'єкт не пройшов валідацію. Перевірте список помилок.")
        {
            Errors = errors ?? new List<string>();
        }
    }
}
