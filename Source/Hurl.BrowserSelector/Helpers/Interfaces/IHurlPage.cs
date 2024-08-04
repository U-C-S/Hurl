using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hurl.BrowserSelector.Helpers.Interfaces
{
    public interface IHurlPage
    {
        public string HeaderTitle { get; }

        public bool IsBackButtonVisible { get; }

    }
}
