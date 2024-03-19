'use strict';
$(document).ready(function () {


    // [ statistic-line chat ] Start
    indo.dashboards.dashboard.getListPurchaseAndSalesMonthlyQty(5)
        .then(function (data) {

            var chart = AmCharts.makeChart("line-area2", {
                "type": "serial",
                "theme": "light",
                "marginTop": 10,
                "marginRight": 0,
                "dataProvider": data,
                "valueAxes": [{
                    "axisAlpha": 0,
                    "position": "left"
                }],
                "graphs": [{
                    "id": "g1",
                    "balloonText": "[[category]]<br><b><span style='font-size:14px;'>[[value]]</span></b>",
                    "bullet": "round",
                    "lineColor": "#1de9b6",
                    "lineThickness": 3,
                    "title": "Purchase ",
                    "negativeLineColor": "#1de9b6",
                    "valueField": "qtyPurchase"
                }, {
                    "id": "g2",
                    "balloonText": "[[category]]<br><b><span style='font-size:14px;'>[[value]]</span></b>",
                    "bullet": "round",
                    "lineColor": "#10adf5",
                    "lineThickness": 3,
                    "title": "Sales ",
                    "negativeLineColor": "#10adf5",
                    "valueField": "qtySales"
                }],
                "chartCursor": {
                    "cursorAlpha": 0,
                    "valueLineEnabled": true,
                    "valueLineBalloonEnabled": true,
                    "valueLineAlpha": 0.3,
                    "fullWidth": true
                },
                "categoryField": "monthName",
                "categoryAxis": {
                    "minorGridAlpha": 0,
                    "minorGridEnabled": true,
                    "gridAlpha": 0,
                    "axisAlpha": 0,
                    "lineAlpha": 0
                },
                "legend": {
                    "useGraphSettings": true,
                    "position": "top"
                },
            });
        });
    // [ statistic-line chat ] end


});
