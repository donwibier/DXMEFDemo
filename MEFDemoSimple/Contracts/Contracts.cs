using System;
using System.Collections.Generic;
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

}
