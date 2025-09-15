$(document).ready(function () {

    window.record = 0;
    const dataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: url_get_tarh,
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
    var grid = $("#report-grid-tarh").kendoGrid({
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
        toolbar: fromReportPage ? [
            {
                template:
                    `<div class="row">
                        <div class="col-md-3 " style="margin-right: 20px;margin-top:6px;">
                                <div class="input-group">
                                    <input style="height: 40px;margin-top:0px;" type="text" class="form-control" id='FieldFilter' placeholder="جستجو...">
                                </div>
                            </div>
                    </div>`
            },
        ] : [
            {
                template:
                    `<div class="row">
                        <div style="margin-right: 20px;margin-top:6px;">#if('${url_create}' != ''){#<a class="k-button" href="${url_create}" >ایجاد طرح</a>#}#</div>
                        <div class="col-md-3 ">
                                <div class="input-group">
                                    <input style="height: 40px;margin-top:0px;" type="text" class="form-control" id='FieldFilter' placeholder="جستجو...">
                                </div>
                            </div>
                    </div>`
            },
            'excel'
        ],
        excel: {
            fileName: "tarh.xlsx",
            filterable: true,
            allPages: true,
        },
        excelExport: function (e) {
            var workbook = e.workbook;
            var sheet = workbook.sheets[0];

            workbook.rtl = true;
            for (var i = 0; i < sheet.rows.length; i++) {
                for (var ci = 0; ci < sheet.rows[i].cells.length; ci++) {
                    sheet.rows[i].cells[ci].hAlign = "right";
                }
            }
        },
        reorderable: true,
        columnMenu: true,
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        },
        columns:  [
            { title: "ردیف", template: "#: ++record #", width: 50, attributes: { class: "text-center" }, media: "(min-width: 200px)" },
            { field: "id", hidden: true },
            { field: "creatorUserName", title: "ثبت کننده", media: "(min-width: 200px)" },
            { field: "title", title: "عنوان", media: "(min-width: 200px)", width: 150 },
            { field: "myData.shenase", title: "شناسه طرح", media: "(min-width: 200px)", width: 100 },
            { field: "senatorName", title: "نام نماینده", media: "(min-width: 200px)" },
            { field: "senatorCity", title: "استان نماینده", media: "(min-width: 200px)" },
            { field: "senatorHozeEntekhabi", title: "حوزه انتخابی نماینده", media: "(min-width: 300px)" },
            {
                title: "عملیات",
                width: fromReportPage ? 120 : 270,
                attributes: {
                    "class": "text-center",
                    style: "text-align: center"
                }
                , template: fromReportPage ?
                    `<button class="k-button" data-link="${url_details_tarh}/?tarhId=#:id#">جزییات</button>`
                    : btns
            }
        ]
    }).data("kendoGrid");
    grid.table.on("click", ".checkbox", selectRow);

    $("#FieldFilter").keyup(function () {

        var value = $("#FieldFilter").val();
        grid = $("#report-grid-tarh").data("kendoGrid");
        if (value) {

            grid.dataSource.filter({
                logic: "or",
                filters: [{
                    field: "title",
                    operator: "contains",
                    value: value
                },
                ]

            });
        } else {
            grid.dataSource.filter({});
        }
    });
});

function reportTypeChanged(e) {
    var grid = $('#report-grid-tarh');
    grid.data('kendoGrid').dataSource.read();
    grid.data('kendoGrid').refresh();
}





