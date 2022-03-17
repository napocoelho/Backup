using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comum.Models
{
    public class FieldException : Exception 
    {
        public string PropertyName { get; set; }

        public FieldException(string message, string propertyName)
            : base(message)
        {
            this.PropertyName = PropertyName;
        }
    }
}
