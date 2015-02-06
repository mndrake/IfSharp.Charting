namespace IfSharp.Charting
open System
open System.Collections.Generic
open Newtonsoft.Json

module ChartTypes =

    /// data point for line and scatter plots
    type Point = { x: float; y: float }

    type Series = Point seq

    /// base class for all chart types
    type ChartBase() =
        let options = new Dictionary<string, obj>()
        let guid = String.Format("N{0}", Guid.NewGuid().ToString("N"))
        let mutable template = """
            <div id="{GUID}">
                <div class="legend small"></div>
                <svg></svg>
            </div>
            <link href="//cdnjs.cloudflare.com/ajax/libs/metrics-graphics/2.1.0/metricsgraphics.min.css" rel="stylesheet" type="text/css">
            <script type="text/javascript">
                require.config({
                    paths: {jquery: "//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min",
                            d3: "//cdnjs.cloudflare.com/ajax/libs/d3/3.5.3/d3.min",
                            MG: "//cdnjs.cloudflare.com/ajax/libs/metrics-graphics/2.1.0/metricsgraphics.min"}});
                require(["jquery", "d3", "MG"], function(jquery, d3, MG) {        
                    MG.data_graphic({OPTIONS});
                });  
            </script>"""
        do
            options.["width"] <- 600
            options.["height"] <- 400
            options.["y_extended_ticks"] <- true
            options.["target"] <- String.Format("#{0}", guid)

        member internal this.Template with get() = template and set value = template <- value

        member this.ToHtml() =
            // FUNC can be used to wrap a javascript function passed to the data_graphic method
            template.Replace("{OPTIONS}", JsonConvert.SerializeObject(options))
                    .Replace("{GUID}", guid)
                    .Replace("\"FUNC", "").Replace("FUNC\"", "")


        member this.Item with get(key) = options.[key] and set key (value: obj) = options.[key] <- value

    type LineChart() =
        inherit ChartBase()
        do
            base.["area"] <- false
            base.["x_accessor"] <- "x"
            base.["y_accessor"] <- "y"
            base.["data"] <- Seq.empty

        member this.addSeries (values: (float*float) seq) = 
            let data : Series seq = this.["data"] |> unbox
            this.["data"] <- Seq.append data (seq [values |> Seq.map (fun p -> {x = fst p; y = snd p})])

    type HistogramChart() =
        inherit ChartBase()
        do
            base.["chart_type"] <- "histogram"
            base.["right"] <- 60
            base.["mouseover"] <- @"FUNCfunction(d, i){$('{GUID} svg .mg-active-datapoint').text('Value: ' + d3.round(d.x,2) + '   Count: ' + d.y)}FUNC"

        member this.Bins with get() : int = this.["bins"] |> unbox and set (value:int) = this.["bins"] <- value
        member this.BarMargin with get() : int = this.["bar_margin"] |> unbox and set (value:int) = this.["bar_margin"] <- value
        member this.Data with get() : seq<float> = this.["data"] |> unbox and set (value:seq<float>) = this.["data"] <- value


type MGChart =

    static member Line (values:seq<float*float>) =
        let chart = new ChartTypes.LineChart()
        chart.addSeries(values)
        chart

    static member Histogram (values: seq<float>) =
        let chart = new ChartTypes.HistogramChart()
        chart.Data <- values
        chart
