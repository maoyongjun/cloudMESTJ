layui.config({
    base: '../../lib/'
}).extend({
    common: 'common',
    bootstrapTable: 'bootstrap-table/bootstrap-table',
    jQueryUI: 'Plugins/jQueryUI',
    TouchPunch: 'Plugins/TouchPunch',
    bootstrap: 'Bootstrap/js/bootstrap',
    htmlClean: 'Plugins/htmlClean',
    echarts: 'echarts/echarts'
}).define(['common', 'jquery', 'form', 'layer', 'jQueryUI', 'TouchPunch', 'bootstrap', 'bootstrapTable', 'htmlClean', 'echarts', 'ChartDefaultOption'], function (exports) {
    var $ = layui.$;
    var currentDocument = null;
    var timerSave = 2e3;
    var demoHtml = $(".demo").html();
    var charts = {};

    function OpenWindow(option) {
        var options = {
            type: 1,
            area: ['70vw', '80vh'],
            offset: ['10vh', '15vw'],
            content: ''
        };
        for (var o in option) {
            options[o] = option[o];
        }
        if (options.type === 1) {
            $.ajax({
                type: 'get',
                url: options.url,
                async: false,
                success: function (data) {
                    options.content = data;
                },
                error: function (e) {
                    var page = '';
                    switch (e.status) {
                        case 404:
                            page = '404.html';
                            break;
                        case 500:
                            page = '500.html';
                            break;
                        default:
                            options.content = "打开窗口失败";
                    }
                    $.ajax({
                        type: 'get',
                        url: '../error/' + page,
                        async: false,
                        success: function (data) {
                            options.content = data;
                        },
                        error: function () {
                            layer.close(load);
                        }
                    });
                }
            });
        }
        else {
            options.content = options.url;
        }
        delete options['url'];
        layui.layer.open(options);
    }
    function handleSaveLayout() {
        var e = $(".demo").html();
        if (e !== window.demoHtml) {
            //saveLayout();
            window.demoHtml = e
        }
    }

    function randomNumber() {
        return randomFromInterval(1, 1e6)
    }
    function randomFromInterval(e, t) {
        return Math.floor(Math.random() * (t - e + 1) + e)
    }
    function gridSystemGenerator() {
        $(".lyrow .preview input").bind("keyup",
            function () {
                var e = 0;
                var t = "";
                var n = false;
                var r = $(this).val().split(" ", 12);
                $.each(r,
                    function (r, i) {
                        if (!n) {
                            if (parseInt(i) <= 0) n = true;
                            e = e + parseInt(i);
                            t += '<div class="col-md-' + i + ' column"></div>'
                        }
                    });
                if (e === 12 && !n) {
                    $(this).parent().next().children().html(t);
                    $(this).parent().prev().show()
                } else {
                    $(this).parent().prev().hide()
                }
            })
    }
    function configurationElm(e, t) {
        $(".demo").delegate(".configuration > a", "click",
            function (e) {
                e.preventDefault();
                var t = $(this).parent().next().next().children();
                $(this).toggleClass("active");
                t.toggleClass($(this).attr("rel"))
            });
        $(".demo").delegate(".configuration .dropdown-menu a", "click",
            function (e) {
                e.preventDefault();
                var t = $(this).parent().parent();
                var n = t.parent().parent().next().children();
                t.find("li").removeClass("active");
                $(this).parent().addClass("active");
                var r = "";
                t.find("a").each(function () {
                    r += $(this).attr("rel") + " "
                });
                t.parent().removeClass("open");
                n.removeClass(r);
                n.addClass($(this).attr("rel"))
            })
    }
    function OpenConfigWindow() {
        $(".dsource").delegate(".config", "click",
            function (e) {
                var elid = $(this).parent().attr('elid');
                OpenWindow({
                    type: 2,
                    title: 'DataSourceConfig',
                    url: 'template/DataBaseConfig.html?elid=' + elid + '&type=Data',
                    btn: ['Save', 'Cancel'],
                    yes: function (index, layero) {
                        var body = layui.layer.getChildFrame('body', index);
                        var id = body.find('[lay-filter=ConfigObjectID]').val();
                        var name = body.find('[lay-filter=SourceName]').val();
                        var url = body.find('[lay-filter=SourceURL]').val();
                        $('#' + id).find('.sourcename').text(name);
                        $('#' + id).attr('sourceURL', url);
                        layui.layer.close(index);
                    },
                    btn2: function (index, layero) {
                        //return false 开启该代码可禁止点击该按钮关闭
                    }
                });
            });
        $(".demo").delegate(".config", "click",
            function (e) {
                var elid = $(this).parent().attr('elid');
                var type = $(this).parent().attr('rel');
                var url = 'template/';
                var title = '';
                if (type === 'Table') {
                    title = 'TableConfig';
                    url += 'TableConfig.html?elid' + elid + '&type=' + type;
                }
                else if (type === 'Echarts') {
                    title = 'Echart Config';
                    url += 'EchartConfig.html?elid=' + elid + '&type=' + type;
                }
                OpenWindow({
                    type: 2,
                    title: title,
                    url: url,
                    btn: ['Save', 'Cancel'],
                    success: function (layero, index) {
                        if (type === 'Echarts') {
                            localStorage.setItem(elid, JSON.stringify(charts[elid].option))
                        }
                    },
                    yes: function (index, layero) {
                        var body = layui.layer.getChildFrame('body', index);
                        var id = body.find('[lay-filter=ConfigObjectID]').val();
                        var type = body.find('[lay-filter=ConfigObjectType]').val();
                        if (type) {
                            var name = body.find('[lay-filter=DataSource]').val();
                            var url = body.find('[lay-filter=xAxisType]').val();
                        } else {
                            var tttt = body.find('[lay-filter=xAxisType]').val();
                        }
                        layui.layer.close(index);
                    },
                    btn2: function (index, layero) {
                    },
                    end: function () {
                        localStorage.removeItem(elid)
                    }
                });
            });
    }
    function removeElm() {
        $(".demo,.dsource").delegate(".remove", "click",
            function (e) {
                e.preventDefault();

                $(this).parent().remove();
                if (!$(".dsource .data").length > 0) {
                    clearSource()
                }
                if (!$(".demo .column").length > 0) {
                    clearDemo()
                }
            })
    }
    function clearDemo() {
        $(".demo").empty()
    }
    function clearSource() {
        $(".dsource").empty()
    }
    function removeMenuClasses() {
        $("#menu-layoutit li button").removeClass("active")
    }
    function changeStructure(e, t) {
        $("#download-layout ." + e).removeClass(e).addClass(t)
    }
    function cleanHtml(e) {
        $(e).parent().append($(e).children().html())
    }
    function downloadLayoutSrc() {
        var e = "";
        $("#download-layout").children().html($(".demo").html());
        var t = $("#download-layout").children();
        t.find(".preview, .configuration, .drag, .remove").remove();
        t.find(".lyrow").addClass("removeClean");
        t.find(".box-element").addClass("removeClean");
        t.find(".lyrow .lyrow .lyrow .lyrow .lyrow .removeClean").each(function () {
            cleanHtml(this)
        });
        t.find(".lyrow .lyrow .lyrow .lyrow .removeClean").each(function () {
            cleanHtml(this)
        });
        t.find(".lyrow .lyrow .lyrow .removeClean").each(function () {
            cleanHtml(this)
        });
        t.find(".lyrow .lyrow .removeClean").each(function () {
            cleanHtml(this)
        });
        t.find(".lyrow .removeClean").each(function () {
            cleanHtml(this)
        });
        t.find(".removeClean").each(function () {
            cleanHtml(this)
        });
        t.find(".removeClean").remove();
        $("#download-layout .column").removeClass("ui-sortable");
        $("#download-layout .row-fluid").removeClass("clearfix").children().removeClass("column");
        if ($("#download-layout .container").length > 0) {
            changeStructure("row-fluid", "row")
        }
        formatSrc = $.htmlClean($("#download-layout").html(), {
            format: true,
            allowedAttributes: [["id"], ["class"], ["data-toggle"], ["data-target"], ["data-parent"], ["role"], ["data-dismiss"], ["aria-labelledby"], ["aria-hidden"], ["data-slide-to"], ["data-slide"]]
        });
        $("#download-layout").html(formatSrc);
        //$("#downloadModal textarea").empty();
        //$("#downloadModal textarea").val(formatSrc)
    }
    function ChartsRezise() {
        for (var item in charts) {
            charts[item].charts.resize({ width: charts[item].charts._dom.parentElement.offsetWidth, height: (charts[item].charts._dom.parentElement.offsetHeight - 30) });
        }
    };

    $(window).resize(function () {
        $("body").css("min-height", $(window).height() - 90);
        $(".demo").css("min-height", $(window).height() - 160)
    });
    $(document).ready(function () {
        $("body").css("min-height", $(window).height() - 90);
        $(".demo").css("min-height", $(window).height() - 220);
        $(".demo, .demo .column").sortable({
            connectWith: ".column",
            opacity: .35,
            handle: ".drag"
        });
        $(".dsource").sortable({
            connectWith: ".data",
            opacity: .35,
            handle: ".drag",
            deactivate: function (e, t) {
                var id = 'data' + randomNumber();
                t.item.attr('id', id);
                t.item.attr('elid', id);
                OpenWindow({
                    type: 2,
                    title: 'DataSource Config',
                    url: 'template/DataBaseConfig.html?elid=' + id + '&type=Data',
                    closeBtn: 0,
                    btn: ['Save', 'Cancel'],
                    yes: function (index, layero) {
                        var body = layui.layer.getChildFrame('body', index);
                        var id = body.find('[lay-filter=ConfigObjectID]').val();
                        var name = body.find('[lay-filter=SourceName]').val();
                        var url = body.find('[lay-filter=SourceURL]').val();
                        t.item.append($('<div class="sourcename">' + name + '</div>'));
                        t.item.attr('sourceURL', url);
                        layui.layer.close(index);
                    },
                    btn2: function (index, layero) {
                        t.item.remove();
                        //return false
                    }
                });
            }
        });
        $(".sidebar-nav .lyrow").draggable({
            connectToSortable: ".demo",
            helper: "clone",
            handle: ".drag",
            drag: function (e, t) {
                t.helper.width(400)
            },
            stop: function (e, t) {
                $(".demo .column").sortable({
                    opacity: .35,
                    connectWith: ".column"
                })
            }
        });
        $(".sidebar-nav .box").draggable({
            connectToSortable: ".column",
            helper: "clone",
            handle: ".drag",
            start: function (e, t) {
                var id = '';
                if (t.helper.context.outerHTML.indexOf('echartDemo') > 0) {
                    id = 'Charts' + randomNumber();
                    t.helper.id = id
                    t.helper.context.children[4].children[0].setAttribute('id', id);
                } else {
                    id = 'Table' + randomNumber();
                    t.helper.id = id
                    t.helper.context.children[4].children[0].setAttribute('id', id);
                }
                t.helper.context.setAttribute('elid', id);
            },
            drag: function (e, t) {
                t.helper.width(400)
            },
            stop: function (e, t) {
                var id = t.helper.id + "";
                $('.sidebar-nav #' + id).attr('id', '');
                if (id.startsWith('Charts')) {
                    var selector = '.demo #' + id;
                    var container = $(selector);
                    if (container.length > 0) {
                        var width = container.parent().width();
                        var option = layui.ChartDefaultOption.OptionPublic;
                        for (var item in layui.ChartDefaultOption.OptionLine) {
                            option[item] = layui.ChartDefaultOption.OptionLine[item];
                        }
                        charts[id] = {};
                        charts[id]['charts'] = layui.echarts.init(container[0], layui.EchartsTheme, { width: width, height: 400 });
                        charts[id]['option'] = option;
                        charts[id].charts.clear();
                        charts[id].charts.resize();
                        charts[id].charts.setOption(option);
                    }
                }
            }
        });
        $(".sidebar-nav .data").draggable({
            connectToSortable: ".dsource",
            helper: "clone",
            handle: ".drag",
            drag: function (e, t) {
                t.helper.width(100)
            },
            stop: function (e, t) {
                //handleJsIds()                
            }
        });
        $("#button-download").click(function (e) {
            //e.preventDefault();
            //downloadLayoutSrc();
            OpenWindow({
                title: '查看/编辑源代码',
                rul: 'template/download.html'
            });
        });
        $("#download").click(function () {
            //downloadLayout();
            //return false
        });
        $("#downloadhtml").click(function () {
            //downloadHtmlLayout();
            //return false
        });
        $("#edit").click(function () {
            $("body").removeClass("devpreview sourcepreview");
            $("body").addClass("edit");
            removeMenuClasses();
            $(this).addClass("active");
            return false
        });
        $("#clear").click(function (e) {
            e.preventDefault();
            clearDemo()
        });
        $("#devpreview").click(function () {
            $("body").removeClass("edit sourcepreview");
            $("body").addClass("devpreview");
            removeMenuClasses();
            $(this).addClass("active");
            return false
        });
        $("#sourcepreview").click(function () {
            $("body").removeClass("edit");
            $("body").addClass("devpreview sourcepreview");
            removeMenuClasses();
            $(this).addClass("active");
            return false
        });
        $(".nav-header").click(function () {
            $(".sidebar-nav .boxes, .sidebar-nav .rows").hide();
            $(this).next().slideDown()
        });
        window.onresize = ChartsRezise;
        removeElm();
        OpenConfigWindow();
        configurationElm();
        gridSystemGenerator();
        setInterval(function () {
            handleSaveLayout()
        },
            timerSave)
    });

    exports('index', {});
}).addcss('../../../view/Admin/css/layoutit.css', 'layoutit');
