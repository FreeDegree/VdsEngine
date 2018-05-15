using System;
using System.Collections.Generic;
using System.Text;

namespace VdsEngine
{
    public interface IPropertyUIInterface
    {

        string PropertyToString();

        void PropertyFromString(string s);
    }
}