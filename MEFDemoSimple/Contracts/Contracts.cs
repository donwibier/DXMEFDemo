using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{ 
    public interface IPlugin
    {
        string Name { get; }
    }
    public interface IContent:IPlugin
    {
        string Get();
    }

    public interface IOperation : IPlugin
    {
        string Execute(string text);
    }
    public interface IDBRepository
    {
        bool Add<T>(T item) where T :class;
        bool Save();
    }

    public interface IDBPluginContext:IPlugin
    {
        void Setup(DbModelBuilder modelBuilder);
    }
}
