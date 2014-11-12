// --------------------------------
// <copyright file="DatabaseService.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

using Wintellect.Sterling;
using Wintellect.Sterling.IsolatedStorage;
using System.ComponentModel;
using Wintellect.Sterling.Exceptions;

namespace EPubReader
{
    internal static class DatabaseService
    {
        private static SterlingEngine _engine;
        private static IsolatedStorageDriver _driver;
        private static ISterlingDatabaseInstance _database;

        internal static ISterlingDatabaseInstance Database
        {
            get { return _database; }
        }

        internal static void Activate()
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                _engine = new SterlingEngine();
                try
                {
                    // engine could be already activated (in main application or some referenced dll);
                    _engine.Activate();
                }
                catch (SterlingActivationException ex) 
                { 
                }
                
                _driver = new IsolatedStorageDriver("EPubReader/");

                try
                {
                    _database = _engine.SterlingDatabase.GetDatabase("EPubReader Database");
                }
                catch (SterlingDatabaseNotFoundException ex)
                {
                    _database = _engine.SterlingDatabase.RegisterDatabase<Database>(_driver);
                }
            }
        }

        internal static void Deactivate()
        {
            if (_engine != null)
            {
                _engine.Dispose();
            }

            _engine = null;
            _driver = null;
            _database = null;            
        }
    }
}
