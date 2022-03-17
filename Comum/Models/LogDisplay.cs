using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Comum.Utils;

namespace Comum.Models
{
    public class LogDisplay
    {
        public DateTime Time { get; set; }
        public StatusLog Status { get; set; }
        public string Message { get; set; }

        public LogDisplay(StatusLog status, string message, DateTime time)
        {
            this.Time = time;
            this.Status = status;
            this.Message = message;
        }
    }
}