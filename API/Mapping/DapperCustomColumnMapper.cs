﻿using API.Models;
using Dapper;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Mapping
{
    public class DapperCustomColumnMapper
    {
        public static void SetCustomColumnMapper()
        {
            var modelTypes = new List<Type>
            {
                typeof(UserBaseInfo),
                typeof(UserAccount)
            };

            foreach(var modelType in modelTypes)
            {
                SqlMapper.SetTypeMap(
                    modelType,
                    new CustomPropertyTypeMap(
                        modelType,
                        (type, columnName) =>
                            type.GetProperties().FirstOrDefault(prop =>
                                prop.GetCustomAttributes(false)
                                    .OfType<ColumnAttribute>()
                                    .Any(attr => attr.Name == columnName))));
            }
        }
    }
}
