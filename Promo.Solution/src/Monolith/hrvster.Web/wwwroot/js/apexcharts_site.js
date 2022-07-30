window.Apex = {
    chart: {
        foreColor: '#fff',
        toolbar: {
            show: false
        },
    },
    colors: ['#FCCF31', '#17ead9', '#f02fc2'],
    stroke: {
        width: 3
    },
    dataLabels: {
        enabled: false
    },
    grid: {
        borderColor: "#40475D",
    },
    xaxis: {
        axisTicks: {
            color: '#333'
        },
        axisBorder: {
            color: "#333"
        }
    },
    fill: {
        type: 'gradient',
        gradient: {
            gradientToColors: ['#F55555', '#6078ea', '#6094ea']
        },
    },
    tooltip: {
        theme: 'dark',
        x: {
            formatter: function (val) {
                return moment(new Date(val)).format("HH:mm:ss")
            }
        }
    },
    yaxis: {
        decimalsInFloat: 2,
        opposite: true,
        labels: {
            offsetX: -10
        }
    }
};

var options = {
    chart: {
        height: 350,
        type: 'area',
        stacked: false,
        animations: {
            enabled: true,
            easing: 'linear',
            dynamicAnimation: {
                speed: 1000
            }
        },
        dropShadow: {
            enabled: true,
            opacity: 0.3,
            blur: 5,
            left: -7,
            top: 22
        },
    },
    markers: {
        size: 2,
    },
    stroke: {
        curve: 'smooth'
    },
    dataLabels: {
        enabled: true
    },
    series: [],
    noData: {
        text: 'Loading...'
    },
    xaxis: {
        type: 'datetime',
    },
    plotOptions: {
        bar: {
            columnWidth: "20%"
        }
    },
    title: {
        text: 'Sweeps and Harvested Tweets'
    },
    fill: {
        type: 'gradient'
    }
}

var entriesOptions = {
    chart: {
        height: 350,
        type: 'area',
        stacked: false,
        animations: {
            enabled: true,
            easing: 'linear',
            dynamicAnimation: {
                speed: 1000
            }
        },
        dropShadow: {
            enabled: true,
            opacity: 0.3,
            blur: 5,
            left: -7,
            top: 22
        },
    },
    markers: {
        size: 2,
    },
    stroke: {
        curve: 'smooth'
    },
    dataLabels: {
        enabled: true
    },
    series: [],
    noData: {
        text: 'Loading...'
    },
    xaxis: {
        type: 'datetime',
    },
    plotOptions: {
        bar: {
            columnWidth: "20%"
        }
    },
    title: {
        text: 'Sweepstake Entries'
    },
    fill: {
        type: 'gradient'
    }
}


var trigoStrength = 3

function generateMinuteWiseTimeSeries(baseval, count, yrange) {
    var i = 0;
    var series = [];
    while (i < count) {
        var x = baseval;
        var y = ((Math.sin(i / trigoStrength) * (i / trigoStrength) + i / trigoStrength + 1) * (trigoStrength * 2))

        series.push([x, y]);
        baseval += 300000;
        i++;
    }
    return series;
}

function getSweepTweets(campaignUid) {
    var url = '/dashboard/gettweets?campaignUid=' + campaignUid;
    $.getJSON(url, function (response) {
        
        chart.updateSeries(response);
    });
}

function getSweepEntries(campaignUid) {
    var url = '/dashboard/getsweepentries?campaignUid=' + campaignUid;
    $.getJSON(url, function (response) {

        entriesChart.updateSeries(response);
    });
}

var chart = new ApexCharts(document.querySelector("#sweepstakesTweetsLineChart"), options);
chart.render();

var entriesChart = new ApexCharts(document.querySelector("#sweepstakesEntriesLineChart"), entriesOptions);
entriesChart.render();

//Window function to get details or update the data
window.setInterval(function () {

    //chart.updateSeries(getTweets())

}, 5000);

$(document).ready(function () {
    $('#promotionForm').change(function () {
        var campaignUid = "";
        $('#promotionForm option:selected').each(function () {
            campaignUid = $(this).val();
        });

        getSweepTweets(campaignUid);
        getSweepEntries(campaignUid);
    });
});