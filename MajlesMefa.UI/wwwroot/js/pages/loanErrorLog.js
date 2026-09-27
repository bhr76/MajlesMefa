$(document).ready(function () {
    var record = 0;
    function value(item, name) {
        return item[name] !== undefined ? item[name] : item[name.charAt(0).toUpperCase() + name.substring(1)];
    }
    function text(valueToShow) {
        return valueToShow === null || valueToShow === undefined ? "" : String(valueToShow);
    }

    var dataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: url_get_loan_error_logs,
                type: "POST",
                dataType: "json"
            },
            parameterMap: function (options, operation) {
                if (operation === "read") {
                    return {
                        models: kendo.stringify(options),
                        userName: $("#loan-log-user-name").val()
                    };
                }
                return options;
            }
        },
        schema: { data: "data", total: "total" },
        pageSize: 10,
        serverPaging: true,
        serverSorting: true
    });

    var grid = $("#loan-error-log-grid").kendoGrid({
        dataSource: dataSource,
        autoBind: true,
        sortable: true,
        resizable: true,
        pageable: { refresh: true, input: true, buttonCount: 5, pageSizes: [10, 20, 50, 100], info: true },
        noRecords: { template: '<div class="mt-4 mb-4"><p class="text-danger">لاگی یافت نشد</p></div>' },
        dataBinding: function () { record = (this.dataSource.page() - 1) * this.dataSource.pageSize(); },
        columns: [
            { title: "ردیف", template: function () { return ++record; }, width: 60 },
            { field: "occurredAtUtc", title: "زمان", width: 170, template: function (item) { var d = kendo.parseDate(value(item, "occurredAtUtc")); return d ? kendo.toString(d, "yyyy/MM/dd HH:mm:ss") : ""; } },
            { field: "userName", title: "کد کاربر", width: 140, template: function (item) { return kendo.htmlEncode(text(value(item, "userName"))); } },
            { field: "action", title: "عملیات", width: 180, template: function (item) { return kendo.htmlEncode(text(value(item, "action"))); } },
            { field: "category", title: "دسته خطا", width: 150, template: function (item) { return kendo.htmlEncode(text(value(item, "category"))); } },
            { field: "errorMessage", title: "شرح خطا", width: 320, template: function (item) { return kendo.htmlEncode(text(value(item, "errorMessage"))); } },
            { field: "traceIdentifier", title: "شناسه پیگیری", width: 240, template: function (item) { return kendo.htmlEncode(text(value(item, "traceIdentifier"))); } },
            { field: "inputJson", title: "ورودی", width: 420, template: function (item) { return '<pre style="white-space:pre-wrap;direction:ltr;text-align:left;max-height:120px;overflow:auto;">' + kendo.htmlEncode(text(value(item, "inputJson"))) + '</pre>'; } },
            { field: "exceptionDetails", title: "جزئیات استثنا", width: 420, template: function (item) { return '<pre style="white-space:pre-wrap;direction:ltr;text-align:left;max-height:120px;overflow:auto;">' + kendo.htmlEncode(text(value(item, "exceptionDetails"))) + '</pre>'; } }
        ]
    }).data("kendoGrid");

    $("#loan-log-search").on("click", function () { grid.dataSource.page(1); });
    $("#loan-log-clear").on("click", function () { $("#loan-log-user-name").val(""); grid.dataSource.page(1); });
    $("#loan-log-user-name").on("keydown", function (e) { if (e.key === "Enter") grid.dataSource.page(1); });
});
