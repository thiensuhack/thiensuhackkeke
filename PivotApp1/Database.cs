// --------------------------------
// <copyright file="Database.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

using System.Collections.Generic;
using Wintellect.Sterling.Database;

namespace EPubReader
{
    internal class Database : BaseDatabaseInstance
    {
        public override string Name
        {
            get { return "EPubReader Database"; }
        }

        protected override List<ITableDefinition> RegisterTables()
        {
            return new List<ITableDefinition>
            {
                CreateTableDefinition<Book,string>(b => b.Metadata.Identifier.Value)
            };
        }
    }
}
