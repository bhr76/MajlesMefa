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
        groupable: true,
        resizable: false,
        reorderable: false,
        filterable: {
            extra: false,
            operators: {
                string: {
                    startswith: "شروع میشود با ",
                    contains: "شامل میشود",
                    eq: "برابر",
                }
            }
        },
        //toolbar: [
        //    {
        //        template:
        //            `<a class="k-button" href="${url_create}" >ایجاد تذکر کتبی</a>`
        //    }],
        columnMenu: true,
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        },
        columns: [
            { title: "ردیف", template: "#: ++record #", width: 50, attributes: { class: "text-center" }, media: "(min-width: 50px)" },
            { field: "id", hidden: true, menu: false },
            //{ field: "dataEntryType", hidden: true, menu: false },
            //{ field: "actionerName", title: "نام اقدام کننده", media: "(min-width: 200px)", width: 100 },
            { field: "description", title: "توضیح", media: "(min-width: 200px)", width: 100 },
            //{ field: "actionDesc", title: "نوع اقدام", media: "(min-width: 200px)", width: 100 },
            //{ field: "actionerParentOrgName", title: "سازمان اقدام کننده", media: "(min-width: 200px)", width: 200 },
            //{ field: "actionerOrgName", title: "شرکت اقدام کننده", media: "(min-width: 200px)", width: 150 },
            //{ field: "created", title: "تاریخ", media: "(min-width: 200px)", width: 150 },
            //{
            //    title: "عملیات",
            //    width: 270,
            //    attributes: {
            //        "class": "text-center",
            //        style: "text-align: center"
            //    }
            //    , template: btns

            //}

        ]
    }).data("kendoGrid");
    grid.table.on("click", ".checkbox", selectRow);
});

function reportTypeChanged(e) {
    var grid = $('#report-grid');
    grid.data('kendoGrid').dataSource.read();
    grid.data('kendoGrid').refresh();
}




