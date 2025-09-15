$(document).ready(function () {

    window.record = 0;
    const dataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: url_get_molaghat,
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
    var grid = $("#report-grid-molaghat").kendoGrid({
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
        dataBound: onDataBound,
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
        toolbar: fromReportPage ? []:[
            {
                template:
                    `<div class="row">
                        <div style="margin-right: 20px;margin-top:6px;">#if('${url_create}' != ''){#<a class="k-button" href="${url_create}" >ایجاد ملاقات</a>#}#</div>
                    </div>`
            },
            'excel'
        ],
        excel: {
            fileName: "molaghat.xlsx",
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
        groupable: true,
        columnMenu: true,
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        },
        columns: [
            { title: "ردیف", template: "#: ++record #", width: 50, attributes: { class: "text-center" }, media: "(min-width: 200px)" },
            { field: "id", hidden: true },
            { field: "dataEntryType", hidden: true },
            { field: "creatorUserName", title: "ثبت کننده", media: "(min-width: 200px)" },
            { field: "senatorName", title: "نام نماینده", media: "(min-width: 200px)" },
            { field: "senatorCity", title: "استان نماینده", media: "(min-width: 200px)" },
            { field: "senatorHozeEntekhabi", title: "حوزه انتخابی نماینده", media: "(min-width: 300px)" },
            { field: "commissionTitle", title: "کمیسیون", media: "(min-width: 300px)", filterable: false },
            { field: "myData.tarikhStr", title: "تاریخ ملاقات", media: "(min-width: 80px)", filterable: false },
            { field: "myData.mahalDesc.name", title: "محل ملاقات", media: "(min-width: 80px)", filterable: false },
           
            {
                title: "عملیات",
                width: fromReportPage ? 120 : 340,
                attributes: {
                    "class": "text-center",
                    style: "text-align: center"
                }
                , template: fromReportPage ?
                    `<button class="k-button" data-link="${url_details_molaghat}/?molaghatId=#:id#">جزییات</button>`
                    : btns

            }
        ]
    }).data("kendoGrid");
    grid.table.on("click", ".checkbox", selectRow);

    function onDataBound(e) {
        var grid = $("#report-grid-molaghat").data("kendoGrid");
        var data = grid.dataSource.data();

        $.each(data, function (i, row) {
            if (row.myData.pasokhNo != null) {
                var element = $('tr[data-uid="' + row.uid + '"] ');
                $(element).addClass("bg-green");
            }
        });
    }
});

function reportTypeChanged(e) {
    var grid = $('#report-grid-molaghat');
    grid.data('kendoGrid').dataSource.read();
    grid.data('kendoGrid').refresh();
}

