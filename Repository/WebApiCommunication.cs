using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class WebApiCommunication
    {
        public string FieldName { get; set; }
        public object Value { get; set; }
        public ExpressionType Expression { get; set; }
    }
}
