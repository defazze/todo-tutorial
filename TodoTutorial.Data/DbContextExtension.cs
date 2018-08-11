using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace TodoTutorial.Data
{
    public static class DbContextExtension
    {
        /// <summary>
        /// Добавление Map названий таблиц и столбцов (Конвертация любого имени из PascalCase нотации в under_scope нотацию)
        /// </summary>
        public static void AddPostgresNameMap(this DbContext context, ModelBuilder modelBuilder)
        {
            //Получаем информацию о свойствах, являющихся таблицами
            var tablePropertyInfos = context.GetType().GetProperties().Where(x => x.PropertyType.Name == "DbSet`1").ToList();

            //Проходим по всем таблицам
            foreach (var tablePropertyInfo in tablePropertyInfos)
            {
                //Получаем тип таблицы и ее имя
                var tableType = tablePropertyInfo.PropertyType.GenericTypeArguments[0];
                var porsgresTableName = GetPostgresName(tablePropertyInfo.Name);

                //Делаем мапинг таблицы на новое имя
                var entity = modelBuilder.Entity(tableType);
                entity.ToTable(porsgresTableName);

                //Далее проходим по свойствам таблицы и мапим их на соответствующие имена
                //Выборку производим по публичным свойствам, имеющим и get и set
                var columnPropertyInfos = tableType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(x => x.GetCustomAttributes(typeof(NotMappedAttribute), true).Length == 0 && x.CanRead && x.CanWrite && IsMapProperty(x))
                    .ToList();
                foreach (var columnPropertyInfo in columnPropertyInfos)
                {
                    var porsgresClomunName = GetPostgresName(columnPropertyInfo.Name);
                    entity.Property(columnPropertyInfo.PropertyType, columnPropertyInfo.Name).HasColumnName(porsgresClomunName);
                }
            }
        }

        /// <summary>
        /// Проверки нужно ли мапить свойство
        /// </summary>
        private static bool IsMapProperty(PropertyInfo propertyInfo)
        {
            //Проверяем Get методы свойств и исключаем виртуальные свойства
            //Для того чтоб подтягивались интерфейсные свойства, то оставляем те что помечены как Final (не обозначены ключевым словом virtual)
            var getMethodInfo = propertyInfo.GetGetMethod();
            return !getMethodInfo.IsVirtual || getMethodInfo.IsVirtual && getMethodInfo.IsFinal;
        }

        /// <summary>
        /// Ковертация любого имени из PascalCase нотации в under_score нотацию (удобную для работы с postgres)
        /// </summary>
        /// <param name="entityName">Любой текст использующий PascalCase</param>
        private static string GetPostgresName(string entityName)
        {
            var replaceRegex = new Regex(@"((?<UpperSymbol>)[A-Z])", RegexOptions.Singleline);
            var result = replaceRegex.Replace(entityName, @"_$1").TrimStart('_').ToLower();
            return result;
        }
    }
}
