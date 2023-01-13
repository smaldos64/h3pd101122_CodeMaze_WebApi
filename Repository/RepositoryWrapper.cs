using Contracts;
using DynamicLinq;
using Entities;
using Entities.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using Newtonsoft.Json;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private RepositoryContext _repoContext;
        private IOwnerRepository _owner;
        private IAccountRepository _account;
        private ITestDynamicRepository _dynamic;
        public IOwnerRepository Owner
        {
            get
            {
                if (_owner == null)
                {
                    _owner = new OwnerRepository(_repoContext);
                }
                return _owner;
            }
        }

        public IAccountRepository Account
        {
            get
            {
                if (_account == null)
                {
                    _account = new AccountRepository(_repoContext);
                }
                return _account;
            }
        }

        public ITestDynamicRepository Dynamic
        {
            get
            {
                if (_dynamic == null)
                {
                    _dynamic = new TestDynamicRepository(_repoContext);
                }
                return _dynamic;
            }
        }

        public RepositoryWrapper(RepositoryContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }

        public void Save()
        {
            _repoContext.SaveChanges();
        }

        private IQueryable<T> FindAllDynamic<T>() where T : class
        {
#if (ENABLED_FOR_LAZY_LOADING_USAGE)
            return _repoContext.Set<T>();
#else
            return this.RepositoryContext.Set<T>().AsNoTracking();
#endif
        }

        private dynamic RemoveCharacters(dynamic InputValue)
        {
            return InputValue.Trim('"');
        }

        //public static FieldInfo GetFieldOrPropertyType<T>(string FieldName)
        //{
        //    //var cl = new Owner();
        //    //Type t = cl.GetType();
        //    ////var FieldInfoObject = typeof(T).GetField(FieldName);
        //    //var FieldInfoObject = t.GetField(FieldName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Static);

        //    //if (null != FieldInfoObject)
        //    //{
        //    //    return (FieldInfoObject);
        //    //}
        //    //else
        //    //{
        //    //    return (null);
        //    //}

        //    var FieldInfoObject = typeof(T).GetField(FieldName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Static);

        //    if (null == FieldInfoObject)
        //    {

        //    }

        //}

        public IEnumerable<T>? GetOwnersByConditions<T>(List<WebApiDynamicCommunication> WebApiDynamicCommunication_Object_List) where T : class
        {
            RepositoryExpressionCriteria<T> RepositoryExpressionCriteria_Object =
                new RepositoryExpressionCriteria<T>();
            List<WebApiDynamicCommunication> WebApiDynamicCommunication_Object_List_Deserialized = new List<WebApiDynamicCommunication>();

            //for (int Counter = 0; Counter < WebApiDynamicCommunication_Object_List.Count; Counter++)
            //{
            //    WebApiDynamicCommunication WebApiDynamicCommunication_Object = new WebApiDynamicCommunication();
            //    WebApiDynamicCommunication_Object = JsonConvert.DeserializeObject<WebApiDynamicCommunication>(WebApiDynamicCommunication_Object_List[Counter]);
            //    WebApiDynamicCommunication_Object_List_Deserialized.Add(WebApiDynamicCommunication_Object);
            //}   

            for (int Counter = 0; Counter < WebApiDynamicCommunication_Object_List.Count; Counter++)
            {
                switch (WebApiDynamicCommunication_Object_List[Counter].Expression)
                {
                    case ExpressionType.Or:
                        RepositoryExpressionCriteria_Object.Or();
                        break;

                    case ExpressionType.And:
                        RepositoryExpressionCriteria_Object.And();
                        break;

                    default:
                        FieldInfo? FieldInfoObject = null;
                        PropertyInfo? PropertyInfoObject = null;
                        Type? FieldOrPropertyType = null;

                        try
                        {
                            FieldInfoObject = typeof(T).GetField(WebApiDynamicCommunication_Object_List[Counter].FieldName,
                                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                            if (null == FieldInfoObject)
                            {
                                PropertyInfoObject = typeof(T).GetProperty(WebApiDynamicCommunication_Object_List[Counter].FieldName,
                                    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                            }
                        }
                        catch (Exception Error)
                        {
                            return (null);
                        }

                        if (null != FieldInfoObject)
                        {
                            FieldOrPropertyType = FieldInfoObject.FieldType;
                        }
                        else
                        {
                            if (null != PropertyInfoObject)
                            {
                                FieldOrPropertyType = PropertyInfoObject.PropertyType;
                            }
                        }

                        if (null != FieldOrPropertyType)
                        {
                            switch (FieldOrPropertyType.Name)
                            {
                                case nameof(Int32):
                                    WebApiDynamicCommunication_Object_List[Counter].Value =
                                        System.Text.Json.JsonSerializer.Deserialize<int>(WebApiDynamicCommunication_Object_List[Counter].Value);
                                    break;

                                case nameof(String):
                                    WebApiDynamicCommunication_Object_List[Counter].Value =
                                        System.Text.Json.JsonSerializer.Deserialize<string>(WebApiDynamicCommunication_Object_List[Counter].Value);
                                    break;

                                case nameof(Boolean):
                                    WebApiDynamicCommunication_Object_List[Counter].Value =
                                        System.Text.Json.JsonSerializer.Deserialize<bool>(WebApiDynamicCommunication_Object_List[Counter].Value);
                                    break;

                                case nameof(Single):  // float
                                    WebApiDynamicCommunication_Object_List[Counter].Value =
                                        System.Text.Json.JsonSerializer.Deserialize<float>(WebApiDynamicCommunication_Object_List[Counter].Value);
                                    break;

                                case nameof(Double):
                                    WebApiDynamicCommunication_Object_List[Counter].Value =
                                        System.Text.Json.JsonSerializer.Deserialize<double>(WebApiDynamicCommunication_Object_List[Counter].Value);
                                    break;

                                case nameof(DateTime):
                                    WebApiDynamicCommunication_Object_List[Counter].Value =
                                        System.Text.Json.JsonSerializer.Deserialize<DateTime>(WebApiDynamicCommunication_Object_List[Counter].Value);
                                    break;

                                default:
                                    return (null);
                            }

                            RepositoryExpressionCriteria_Object.Add(WebApiDynamicCommunication_Object_List[Counter].FieldName,
                                                                    WebApiDynamicCommunication_Object_List[Counter].Value,
                                                                    WebApiDynamicCommunication_Object_List[Counter].Expression);
                        }
                        else
                        {
                            return (null);
                        }
                        break;
                }
            }

            var Lambda = RepositoryExpressionCriteria_Object.GetLambda();
            if (null != Lambda)
            {
                var LambdaCompile = Lambda.Compile();
                return FindAllDynamic<T>().Where(LambdaCompile);
            }
            else
            {
                return null;
            }
        }
    }
}
