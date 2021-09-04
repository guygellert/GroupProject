function create_type_pie_chart(data, wanted_element) {
    var width  = 450,
        height = 450,
        radius = Math.min(width, height) / 2;
    var color = d3.scaleOrdinal(d3.schemeCategory10);

    var arc = d3.arc()
        .outerRadius(radius - 10)
        .innerRadius(0);

    var labelArc = d3.arc()
        .outerRadius(radius - 40)
        .innerRadius(radius - 40);

    var pie = d3.pie()
        .sort(null)
        .value(function (d) { return d.stocks; });

    var svg = d3.select(wanted_element).append("svg")
        .attr("width", width)
        .attr("height", height)
        .append("g")
        .attr("transform", "translate(" + width / 2 + "," + height / 2 + ")");

    var g = svg.selectAll(".arc")
        .data(pie(data))
        .enter().append("g")
        .attr("class", "arc");

    g.append("path")
        .attr("d", arc)
        .style("fill", function (d) { return color(d.data.categoryName); });

    g.append("text")
        .attr("transform", function (d) {                    //set the label's origin to the center of the arc
            //we have to make sure to set these before calling arc.centroid
            d.innerRadius = 0;
            d.outerRadius = radius;
            return "translate(" + arc.centroid(d) + ")";        //this gives us a pair of coordinates like [50, 50]
        })
        .attr("font-size", 14)
        .attr("font-weight", "bold")
        .attr("text-anchor", "middle")                          //center the text on it's origin 
        .attr("fill", "black")
        .text(function (d, i) { return data[i].categoryName + " : " + data[i].stocks; });        //get the label from our original data array
};
function create_maximum_profit_per_category(data, wanted_element) {
    var width = 300,
        height = 300,
        radius = Math.min(width, height) / 2;
    var color = d3.scaleOrdinal(d3.schemeCategory10);

    var arc = d3.arc()
        .outerRadius(radius - 10)
        .innerRadius(0);

    var labelArc = d3.arc()
        .outerRadius(radius - 40)
        .innerRadius(radius - 40);

    var pie = d3.pie()
        .sort(null)
        .value(function (d) { return d.profit; });

    var svg = d3.select(wanted_element).append("svg")
        .attr("width", width)
        .attr("height", height)
        .append("g")
        .attr("transform", "translate(" + width / 2 + "," + height / 2 + ")");

    var g = svg.selectAll(".arc")
        .data(pie(data))
        .enter().append("g")
        .attr("class", "arc");

    g.append("path")
        .attr("d", arc)
        .style("fill", function (d) { return color(d.data.categoryName); });

    g.append("text")
        .attr("transform", function (d) {                    //set the label's origin to the center of the arc
            //we have to make sure to set these before calling arc.centroid
            d.innerRadius = 0;
            d.outerRadius = radius;
            return "translate(" + arc.centroid(d) + ")";        //this gives us a pair of coordinates like [50, 50]
        })
        .attr("font-size", 14)
        .attr("font-weight", "bold")
        .attr("text-anchor", "middle")                          //center the text on it's origin 
        .attr("fill", "black")
        .text(function (d, i) { return data[i].categoryName + " : " + data[i].profit; });        //get the label from our original data array
};