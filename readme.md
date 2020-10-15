# roslyndumper
A half-baked .NET Standard/C# roslyn exporter for objects.

| Name | Resources |
| ------ | ------ |
| brent | https://gist.github.com/brentmaxwell/21d1f91f70c048593e57 |
| thomas| https://github.com/thomasgalliker/ObjectDumper |

#### A half-baked implementation:
This was originally created for the replacement of another abandoned object exporter. It was critical to export to vb.net/c# with the same export. This was never realized yet has the foundation for you to review and to get a few ideas. 

Since then Thomas created a great object dumper which is highly recommended if you’re looking for a complete solution.

#### Other issues and additional information:
+ The source is not issue/bug free.
+ Only a few of the functional tests pass (based on thomas's repo). 
+ VB.NET source can be created by implementing the VB.NET workspace or by leveraging [Code Converter](https://github.com/icsharpcode/CodeConverter "code converter").

#### Getting Started:
```
using System;
using RoslynDumper;

namespace RoslynDumper.App
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            var person = PersonFactory.GetPersonThomas();

            var roslynDumper = new RoslynDumper();
            var dump = roslynDumper.Dump(person);
        }
    }
}
```
