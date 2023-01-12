using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DynamicLinq
{
    public class WebApiDynamicCommunication
    {
        public string? FieldName { get; set; }
        public dynamic? Value { get; set; }
        public ExpressionType Expression { get; set; }
    }
}
