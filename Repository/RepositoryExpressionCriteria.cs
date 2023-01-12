using Castle.DynamicProxy.Contributors;
using Contracts;
using DynamicLinq;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryExpressionCriteria<T> 
    {
        class RepositoryExpressionCriterion
        {
            public RepositoryExpressionCriterion(
                string propertyName,
                object value,
                ExpressionType op,
                string andOr = "And")
            {
                AndOr = andOr;
                PropertyName = propertyName;
                Value = value;
                Operator = op;
                validateProperty(typeof(T), propertyName);
            }

            PropertyInfo validateProperty(Type type, string propertyName)
            {
                string[] parts = propertyName.Split('.');
                //var Test = type.GetProperty(propertyName);

                var info = (parts.Length > 1)
                ? validateProperty(
                    type.GetProperty(
                        parts[0]).PropertyType,
                        parts.Skip(1).Aggregate((a, i) =>
                            a + "." + i)) : type.GetProperty(propertyName);
                if (info == null)
                    throw new ArgumentException(propertyName,
                            //$"Property {propertyName}
                            //is not a member of
                            //{type.Name}");
                            $"Hej Kurt {propertyName} {type.Name}");
                return info;
            }
            public string PropertyName { get; }
            public object Value { get; }
            public ExpressionType Operator { get; }
            public string AndOr { get; }
        }

        private string _andOr = "And";

        List<RepositoryExpressionCriterion> _expressionCriterion = new
            List<RepositoryExpressionCriterion>();

        public RepositoryExpressionCriteria<T> And()
        {
            this._andOr = "And";
            return this;
        }
        public RepositoryExpressionCriteria<T> Or() 
        {
            this._andOr = "Or";
            return this;
        }

        public RepositoryExpressionCriteria<T> Add(string propertyName,
                                                   object value, ExpressionType op)
        {
            var Test = value.GetType();

            var newCriterion = new RepositoryExpressionCriterion(propertyName, value,
                op, _andOr);
            this._expressionCriterion.Add(newCriterion);
            return this;
        }

        Expression GetExpression(ParameterExpression parameter,
                                 RepositoryExpressionCriterion ExpressionCriteria)
        {
            Expression expression = parameter;
            foreach (var member in ExpressionCriteria.PropertyName.Split('.'))
            {
                expression = Expression.PropertyOrField(expression, member);
            }
            return (Expression.MakeBinary(ExpressionCriteria.Operator,
                    expression, Expression.Constant(ExpressionCriteria.Value)));
        }

        public Expression<Func<T, bool>>? GetLambda()
        {
            Expression? expression = null;
            var parameterExpression = Expression.Parameter(typeof(T),
                                                           typeof(T).Name.ToLower());
            foreach (var item in _expressionCriterion)
            {
                if (expression == null)
                {
                    expression = GetExpression(parameterExpression, item);
                }
                else
                {
                    expression = item.AndOr == "And" ?
                        Expression.And(expression,
                            GetExpression(parameterExpression, item)) :
                        Expression.Or(expression,
                            GetExpression(parameterExpression, item));
                }
            }

            return expression != null ?
                Expression.Lambda<Func<T, bool>>(expression, parameterExpression) : null;
        }
    }
}
