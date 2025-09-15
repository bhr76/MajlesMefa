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
        //sortable: true,
        resizable: false,
        reorderable: false,
        filterable: {
            extra: false,
            operators: {
                string: {
                    contains: "شامل میشود",
                    startswith: "شروع میشود با ",
                    eq: "برابر",
                }
            }
        },
        toolbar: [
            {
                template:
                    `<div class="row">
                        <div style="margin-right: 20px;margin-top:3px;"><button class="k-button mt-1" id="create-btn" data-link="${url_create}"> افزودن </button></div>
                         <div class="col-md-3 ">
                                <div class="input-group">
                                    <input style="height: 40px;margin-top:0px;" type="text" class="form-control" id='FieldFilter' placeholder="جستجو...">
                                </div>
                            </div>
                    </div>`
            }        ],
        columnMenu: true,
        dataBinding: function () {
            record = (this.dataSource.page() - 1) *  this.dataSource.pageSize();
        },
        columns: [
            { title: "ردیف", template: "#: ++record #", width: 50, attributes: { class: "text-center" }, media: "(min-width: 200px)" },
            { field: "id", hidden: true, menu: false },
            { field: "name", title: "عنوان", media: "(min-width: 200px)" },
            { field: "isCentralOfficeStr", hidden: hasCentralOffice? false : true, title: "زیرمجموعه ادارات کل استان‌ها", media: "(min-width: 200px)", filterable: false },
            { field: "isCentralOffice", hidden: true, menu: false},
            {
                title: "عملیات",
                width: 300,
                attributes: {
                    "class": "text-center",
                    style: "text-align: center"
                },
                template: btns

            }
        ]
    }).data("kendoGrid");
    grid.table.on("click", ".checkbox", selectRow);

    $("#FieldFilter").keyup(function () {

        var value = $("#FieldFilter").val();
        grid = $("#report-grid").data("kendoGrid");
        if (value) {

            grid.dataSource.filter({
                logic: "or",
                filters: [{
                    field: "name",
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
    var grid = $('#report-grid');
    grid.data('kendoGrid').dataSource.read();
    grid.data('kendoGrid').refresh();
}

