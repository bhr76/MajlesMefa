$(document).ready(function () {

    window.record = 0;
    const dataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: url_get,
                dataType: "json",
                type: 'Post'
            },
            parameterMap: function (options) {
                return `models= ${kendo.stringify(options)}`;
            }
        },
        schema: {
            data: "data",
            total: "total"
        },
        error: function (e) {
            alert(e.errorThrown);
        },
        pageSize: 10,
        sort: { field: "id", dir: "desc" },
        serverPaging: true,
        serverFiltering: true,
        serverSorting: true
    });
    var grid = $("#report-grid").kendoGrid({
        //toolbar: ["excel"],
        //excel: {
        //    fileName: "LogData_Export.xlsx",
        //    //proxyURL: "https://demos.telerik.com/kendo-ui/service/export",
        //    filterable: true
        //},
        dataSource: dataSource,
            //[
            //{
            //    id: "0",
            //    title: "salam",
            //    graspNumber: "255"
            //}],
        autoBind: true,
        scrollable: true,
        noRecords: {
            template: `<div class="mt-4 mb-4"><p class="text-danger">داده ای یافت نشد</p></div>`
        },
        pageable: {
            buttonCount: 5,
            refresh: true,
            input: true,
            pageSizes: [5, 10, 15, 20, 25],
            info: true
        },
        sortable: true,
        filterable: {
            extra: false,
            operators: {
                string: {
                    contains: "شامل میشود",
                    startswith: "شروع میشود با ",
                    eq: "برابر",
                }
            },
            columns: false,
        },
        toolbar: [
            //{
            //    template:
            //        `<a class="k-button" href="${url_create}" >ایجاد صفحه</a>`
            //}
        ],
        reorderable: true,
        columnMenu: true,
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        },
        columns: [
            { title: "ردیف", template: "#: ++record #", width: 50, attributes: { class: "text-center" }, media: "(min-width: 200px)" },
            { field: "id", hidden: true },
            { field: "title", title: "عنوان", media: "(min-width: 200px)" ,width: 150 },
            { field: "controller", title: "Controller", media: "(min-width: 200px)", width: 100 },
            { field: "action", title: "Action", media: "(min-width: 200px)", width: 100 },
            { field: "url", title: "URL", media: "(min-width: 200px)", width: 100 },
            { field: "rolesStr", title: "نقش", media: "(min-width: 200px)", width: 250 },
            {
                title: "عملیات",
                width: 160,
                attributes: {
                    "class": "text-center",
                    style: "text-align: center"
                },
                template: btns

            }
        ]
    }).data("kendoGrid");
    grid.table.on("click", ".checkbox", selectRow);
});

function reportTypeChanged(e) {
    var grid = $('#report-grid');
    grid.data('kendoGrid').dataSource.read();
    grid.data('kendoGrid').refresh();
}

