namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("IfSharp.Charting")>]
[<assembly: AssemblyProductAttribute("IfSharp.Charting")>]
[<assembly: AssemblyDescriptionAttribute("charting library for IfSharp using MetricsGraphics.js")>]
[<assembly: AssemblyVersionAttribute("0.0.1")>]
[<assembly: AssemblyFileVersionAttribute("0.0.1")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "0.0.1"
