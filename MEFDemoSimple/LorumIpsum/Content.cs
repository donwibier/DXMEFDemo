using Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LorumIpsum
{
    [Export(typeof(IContent))]
    public class Content : IContent
    {
        public string Name { get { return "Lorum Ipsum"; } }

        public string Get()
        {
            return @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam convallis odio eget sapien rutrum posuere faucibus id nibh. Nullam ullamcorper vehicula mollis. In quis massa ut odio ultrices dignissim. Proin efficitur orci nec libero gravida egestas. Fusce sit amet magna ex. Vivamus id massa finibus, interdum ipsum vel, volutpat mauris. Integer vitae molestie sem. Duis pellentesque dictum sem, quis mollis libero auctor et. Donec id tortor et mi molestie aliquet sit amet sit amet metus. Nulla facilisi.";
        }
    }
}
