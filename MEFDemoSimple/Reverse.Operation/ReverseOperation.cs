using Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reverse.Operation
{
    [Export(typeof(IOperation))]
    public class ReverseOperation: IOperation
    {
        public string Name { get { return "Reverse"; } }

        public string Execute(string text)
        {
            char[] arr = text.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }
    }
}
