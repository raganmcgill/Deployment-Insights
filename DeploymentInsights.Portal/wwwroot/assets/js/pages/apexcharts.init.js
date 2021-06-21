Apex.grid = {
    padding: {
        right: 0,
        left: 0
    }
}, Apex.dataLabels = {
    enabled: !1
};

colors = ["green", "red"];
(dataColors = $("#chart-deployment-frq").data("colors")) && (colors = dataColors.split(","));


var deployment_frequency = [];
deployment_frequency.push(["2014-01-01", 12])
deployment_frequency.push(["2014-02-01", 45])
deployment_frequency.push(["2014-03-01", 23])
deployment_frequency.push(["2014-04-01", 11])
deployment_frequency.push(["2014-04-15", 24])
deployment_frequency.push(["2014-05-01", 17])

var deployment_frequency2 = [];
deployment_frequency2.push(["2014-02-01", 45])
deployment_frequency2.push(["2014-05-01", 17])

var options = {
    series: [
        {
            name: 'Successful',
            data: deployment_frequency
        },
        {
            name: 'Failures',
            type: 'scatter',
            data: deployment_frequency2
        }
    ],
    colors: colors,
    chart: {
        type: 'area',
        stacked: false,
        height: 250,
        zoom: {
            type: 'x',
            enabled: true,
            autoScaleYaxis: true
        },
        toolbar: {
            autoSelected: 'zoom'
        }
    },
    dataLabels: {
        enabled: true
    },
    markers: {
        size: 6,
    },
    title: {
        text: 'Deployment Frequency',
        align: 'left'
    },
    //fill: {
    //    type: 'gradient',
    //    gradient: {
    //        shadeIntensity: 1,
    //        inverseColors: false,
    //        opacityFrom: 0.5,
    //        opacityTo: 0,
    //        stops: [0, 90, 100]
    //    },
    //},
    yaxis: {
        labels: {
            formatter: function (val) {
                return (val).toFixed(0);
            },
        },
        title: {
            text: 'Price'
        },
    },
    xaxis: {
        type: 'datetime',
    },
    tooltip: {
        shared: false,
        y: {
            formatter: function (val) {
                return (val).toFixed(0)
            }
        }
    }
};

var chart = new ApexCharts(document.querySelector("#chart-deployment-frq"), options);
chart.render();



var lead_time = [];
lead_time.push(["2014-01-01", 2])
lead_time.push(["2014-02-01", 5])
lead_time.push(["2014-03-01", 3])
lead_time.push(["2014-04-01", 7])
lead_time.push(["2014-04-15", 2])
lead_time.push(["2014-05-01", 5])
var options = {
    series: [{
        name: '',
        data: lead_time
    }],
    colors: colors,
    chart: {
        type: 'area',
        stacked: false,
        height: 250,
        zoom: {
            type: 'x',
            enabled: true,
            autoScaleYaxis: true
        },
        toolbar: {
            autoSelected: 'zoom'
        }
    },
    dataLabels: {
        enabled: false
    },
    markers: {
        size: 0,
    },
    title: {
        text: 'Deployment Frequency',
        align: 'left'
    },
    fill: {
        type: 'gradient',
        gradient: {
            shadeIntensity: 1,
            inverseColors: false,
            opacityFrom: 0.5,
            opacityTo: 0,
            stops: [0, 90, 100]
        },
    },
    yaxis: {
        labels: {
            formatter: function (val) {
                return (val).toFixed(0);
            },
        },
        title: {
            text: 'Price'
        },
    },
    xaxis: {
        type: 'datetime',
    },
    tooltip: {
        shared: false,
        y: {
            formatter: function (val) {
                return (val).toFixed(0)
            }
        }
    }
};

var chart = new ApexCharts(document.querySelector("#chart-lead-time"), options);
chart.render();




var change_fail_rate = [];
change_fail_rate.push(["2014-02-01", 20])
change_fail_rate.push(["2014-04-01", 15])
change_fail_rate.push(["2014-04-15", 10])
change_fail_rate.push(["2014-05-01", 5])
var options = {
    series: [{
        name: '',
        data: change_fail_rate
    }],
    colors: colors,
    chart: {
        type: 'area',
        stacked: false,
        height: 250,
        zoom: {
            type: 'x',
            enabled: true,
            autoScaleYaxis: true
        },
        toolbar: {
            autoSelected: 'zoom'
        }
    },
    dataLabels: {
        enabled: false
    },
    markers: {
        size: 0,
    },
    title: {
        text: 'Change Fail Rate',
        align: 'left'
    },
    fill: {
        type: 'gradient',
        gradient: {
            shadeIntensity: 1,
            inverseColors: false,
            opacityFrom: 0.5,
            opacityTo: 0,
            stops: [0, 90, 100]
        },
    },
    yaxis: {
        labels: {
            formatter: function (val) {
                return (val).toFixed(0);
            },
        },
        title: {
            text: 'Price'
        },
    },
    xaxis: {
        type: 'datetime',
    },
    tooltip: {
        shared: false,
        y: {
            formatter: function (val) {
                return (val).toFixed(0)
            }
        }
    }
};

var chart = new ApexCharts(document.querySelector("#chart-change-fail"), options);
chart.render();



var mttr = [];
mttr.push(["2014-02-01", 13])
mttr.push(["2014-04-01", 13])
mttr.push(["2014-04-15", 14])
mttr.push(["2014-05-01", 25])
var options = {
    series: [{
        name: '',
        data: mttr
    }],
    colors: colors,
    chart: {
        type: 'area',
        stacked: false,
        height: 250,
        zoom: {
            type: 'x',
            enabled: true,
            autoScaleYaxis: true
        },
        toolbar: {
            autoSelected: 'zoom'
        }
    },
    dataLabels: {
        enabled: false
    },
    markers: {
        size: 0,
    },
    title: {
        text: 'MTTR Trend',
        align: 'left'
    },
    fill: {
        type: 'gradient',
        gradient: {
            shadeIntensity: 1,
            inverseColors: false,
            opacityFrom: 0.5,
            opacityTo: 0,
            stops: [0, 90, 100]
        },
    },
    yaxis: {
        labels: {
            formatter: function (val) {
                return (val).toFixed(0);
            },
        },
        title: {
            text: 'Price'
        },
    },
    xaxis: {
        type: 'datetime',
    },
    tooltip: {
        shared: false,
        y: {
            formatter: function (val) {
                return (val).toFixed(0)
            }
        }
    }
};

var chart = new ApexCharts(document.querySelector("#chart-mttr"), options);
chart.render();