var data1 = [
    ['product', '2015', '2016', '2017'],
    ['Tablet', 43.3, 85.8, 93.7],
    ['phone', 83.1, 73.4, 55.1],
    ['Switch', 86.4, 65.2, 82.5],
    ['Leptop', 80.4, 60.2, 90.5]
];

var data2 = [
    { value: 335, name: 'Tablet' },
    { value: 310, name: 'phone' },
    { value: 274, name: 'Switch' },
    { value: 235, name: 'Leptop' },
    { value: 400, name: 'Monitor' }
];

var data3 = [
    [161.2, 51.6], [167.5, 59.0], [159.5, 49.2], [157.0, 63.0], [155.8, 53.6],
    [170.0, 59.0], [159.1, 47.6], [166.0, 69.8], [176.2, 66.8], [160.2, 75.2],
    [172.5, 55.2], [170.9, 54.2], [172.9, 62.5], [153.4, 42.0], [160.0, 50.0],
    [147.2, 49.8], [168.2, 49.2], [175.0, 73.2], [157.0, 47.8], [167.6, 68.8],
    [159.5, 50.6], [175.0, 82.5], [166.8, 57.2], [176.5, 87.8], [170.2, 72.8],
    [174.0, 54.5], [173.0, 59.8], [179.9, 67.3], [170.5, 67.8], [160.0, 47.0],
    [154.4, 46.2], [162.0, 55.0], [176.5, 83.0], [160.0, 54.4], [152.0, 45.8],
    [162.1, 53.6], [170.0, 73.2], [160.2, 52.1], [161.3, 67.9], [166.4, 56.6],
    [168.9, 62.3], [163.8, 58.5], [167.6, 54.5], [160.0, 50.2], [161.3, 60.3],
    [167.6, 58.3], [165.1, 56.2], [160.0, 50.2], [170.0, 72.9], [157.5, 59.8],
    [167.6, 61.0], [160.7, 69.1], [163.2, 55.9], [152.4, 46.5], [157.5, 54.3],
    [168.3, 54.8], [180.3, 60.7], [165.5, 60.0], [165.0, 62.0], [164.5, 60.3],
    [156.0, 52.7], [160.0, 74.3], [163.0, 62.0]
];

var OptionData = {
    AxisType: ['category', 'value', 'time', 'log'],
    ChartsType: [
        {
            label: '直線圖',
            value: 'line'
        },
        {
            label: '柱狀圖',
            value: 'bar'
        },
        {
            label: '元餅圖',
            value: 'pie'
        },
        {
            label: '散點圖',
            value: 'scatter'
        },
        {
            label: '雷達圖',
            value: 'radar'
        },
        {
            label: '樹形圖',
            value: 'tree'
        },
        {
            label: '樹形圖',
            value: 'tree'
        },
        {
            label: '曲線圖',
            value: 'graph'
        },
        {
            label: '漏斗圖',
            value: 'funnel'
        },
        {
            label: '儀錶圖',
            value: 'gauge'
        }
    ],
    seriesLayoutBy: ['row', 'column']
};

layui.define(function (exports) {
    exports('ChartDefaultOption', {
        OptionPublic: {
            title: {
                text: 'DemoCharts',
                left: 'center',
                top: 10,
                textStyle: {
                    color: '#000'
                }
            },
            xAxis: {
                type: 'category'
            },
            yAxis: {
                type: 'value'
            },
            grid: {
                top: '10%',
                right: '10%',
                bottom: '10%',
                left:'10%'
            },
        },
        OptionLine: {
            dataset: {
                source: data1
            },
            series: [
                { id: 'serial123456',name: '', type: 'line', seriesLayoutBy: 'row' },
                { id: 'serial123457',name: '', type: 'line', seriesLayoutBy: 'row' },
                { id: 'serial123458',name: '', type: 'line', seriesLayoutBy: 'row' },
            ]
        },
        OptionBar: {
            dataset: {
                source: data1
            },
            series: [
                { id: 'serial123456',name:'',type: 'bar', seriesLayoutBy: 'row' },
                { id: 'serial123457', name: '',type: 'bar', seriesLayoutBy: 'row' },
                { id: 'serial123458', name: '',type: 'bar', seriesLayoutBy: 'row' },
            ]
        },
        OptionPie: {
            dataset: {
                source: data2
            },
        },
        OptionScatter: {
            dataset: {
                source: data3
            },
            series: [
                { type: 'scatter', symbolSize: 2 }
            ]
        },
        OptionGauge: {
            dataset: {
                source: data2[0]
            },
            series: [
                {
                    type: 'gauge',
                    detail: { formatter: '{value}%' }
                }
            ]
        }
    });
});