﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.Container.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class ImportConstructorAttribute : Attribute
    {
    }
}
