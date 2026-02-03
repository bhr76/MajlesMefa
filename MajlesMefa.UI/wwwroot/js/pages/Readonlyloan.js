$(document).ready(function () {

    $("#PasokhStateDropDown").kendoDropDownList({
        dataSource: [
            { text: "در دست اقدام", value: 0 },
            { text: "پرداخت شده", value: 1 },
            { text: "رد درخواست", value: 2 },
            { text: "در دست شعبه", value: 3 }
        ],
        dataTextField: "text",
        dataValueField: "value",
        valuePrimitive: true,
        optionLabel: "همه"
    });

    $("#LoanTypeDropDown").kendoDropDownList({
        dataSource: [
            { text: "مرابحه", value: 2 },
            { text: "قرض الحسنه", value: 1 }
        ],
        dataTextField: "text",
        dataValueField: "value",
        valuePrimitive: true,
        optionLabel: "همه"
    });

    $("#LoanTypeDropDown").on("change", function (e) {
        var filters = dataSource._filter?.filters ?? [];
        if (e.target.value) {
            filters = filters.filter(x => x.field != "loanType");
            filters.push({ field: "loanType", value: e.target.value, operator: "eq" });
            dataSource.filter(filters).read();
        }
        else {
            dataSource.filter(filters.filter(x => x.field != "loanType")).read();
        }
    });

    $("#PasokhStateDropDown").on("change", function (e) {
        var filters = dataSource._filter?.filters ?? [];
        if (e.target.value) {
            filters = filters.filter(x => x.field != "pasokhState");
            filters.push({ field: "pasokhState", value: e.target.value, operator: "eq" });
            dataSource.filter(filters).read();
        } else {
            dataSource.filter(filters.filter(x => x.field != "pasokhState")).read();
        }
    });

    window.record = 0;
    const dataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: url_get_loan,
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

    var grid = $("#report-grid-loan").kendoGrid({
        dataSource: dataSource,
        autoBind: true,
        scrollable: true,
        resizable: true,

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
                },
                enums: {
                    eq: "Equal to",
                    neq: "Not equal to"
                }
            }
        },
        toolbar: [
            'excel'
        ],
        excel: {
            fileName: "readonly-loans.xlsx",
            filterable: true,
            allPages: true,
        },
        excelExport: function (e) {
            var workbook = e.workbook;
            var sheet = workbook.sheets[0];

            workbook.rtl = true;
            
            // Set right alignment for all cells
            for (var i = 0; i < sheet.rows.length; i++) {
                for (var ci = 0; ci < sheet.rows[i].cells.length; ci++) {
                    sheet.rows[i].cells[ci].hAlign = "right";
                }
            }
            
            // Format header row
            if (sheet.rows.length > 0) {
                for (var ci = 0; ci < sheet.rows[0].cells.length; ci++) {
                    sheet.rows[0].cells[ci].bold = true;
                    sheet.rows[0].cells[ci].background = "#7a7a7a";
                    sheet.rows[0].cells[ci].color = "#ffffff";
                }
            }
        },
        reorderable: true,
        resizeable: true,
        dataBound: onDataBound,
        groupable: true,
        columnMenu: true,
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        },
        columns: [
            {
                title: "ردیف",
                template: "#: ++record #",
                width: 50,
                attributes: { class: "text-center" },
                media: "(min-width: 200px)",
                filterable: false,
                sortable: false,
                exportable: false
            },
            {
                field: "id",
                hidden: true,
                exportable: false
            },
            {
                field: "trackingCode",
                title: "شناسه یکتا",
                media: "(min-width: 200px)",
                width: "105px",
                filterable: {
                    cell: {
                        operator: "contains",
                        showOperators: false
                    }
                }
            },
            {
                field: "senatorName",
                title: "نام نماینده",
                media: "(min-width: 200px)",
                width: "120px",
                filterable: {
                    cell: {
                        operator: "contains",
                        showOperators: false
                    }
                }
            },
            {
                field: "senatorCity",
                title: "استان نماینده",
                media: "(min-width: 200px)",
                width: "120px",
                filterable: {
                    cell: {
                        operator: "contains",
                        showOperators: false
                    }
                }
            },
            {
                field: "senatorHozeEntekhabi",
                title: "حوزه انتخابی",
                media: "(min-width: 200px)",
                width: "120px",
                filterable: {
                    cell: {
                        operator: "contains",
                        showOperators: false
                    }
                }
            },
            {
                field: "loanOwnerFullName",
                title: "نام متقاضی",
                media: "(min-width: 400px)",
                width: "120px",
                filterable: {
                    cell: {
                        operator: "contains",
                        showOperators: false
                    }
                }
            },
            {
                field: "loanOwnerMobile",
                title: "موبایل متقاضی",
                media: "(min-width: 400px)",
                width: "120px",
                filterable: {
                    cell: {
                        operator: "contains",
                        showOperators: false
                    }
                }
            },
            {
                field: "loanOwnerNationalCode",
                title: "کدملی متقاضی",
                media: "(min-width: 400px)",
                width: "120px",
                filterable: {
                    cell: {
                        operator: "contains",
                        showOperators: false
                    }
                }
            },
            {
                field: "persianCreatedDate",
                title: "تاریخ درخواست",
                media: "(min-width: 200px)",
                width: "120px",
                filterable: {
                    cell: {
                        operator: "contains",
                        showOperators: false
                    }
                }
            },
            {
                field: "actionRefrenceDate",
                title: "تاریخ ارجا به شعبه",
                media: "(min-width: 200px)",
                width: "130px",
                filterable: {
                    cell: {
                        operator: "contains",
                        showOperators: false
                    }
                }
            },
            {
                field: "amount",
                title: "مبلغ تسهیلات(تومان)",
                media: "(min-width: 200px)",
                width: "150px",
                filterable: {
                    cell: {
                        operator: "contains",
                        showOperators: false
                    }
                }
            },
            {
                field: "loanTypeDesc",
                title: "نوع تسهیلات",
                media: "(min-width: 400px)",
                width: "120px",
                filterable: false,
                sortable: false
            },
            {
                field: "pasokhStateDesc",
                title: "وضعیت",
                media: "(min-width: 400px)",
                width: "120px",
                filterable: false,
                sortable: false
            },
            {
                field: "canAccessActionRefrence",
                hidden: true,
                exportable: false
            },
            {
                field: "pasokhState",
                title: "وضعیت",
                hidden: true,
                width: "105px",
                exportable: false
            },
            {
                field: "suggestedBankName",
                title: "بانک عامل",
                media: "(min-width: 200px)",
                width: "120px",
                filterable: {
                    cell: {
                        operator: "contains",
                        showOperators: false
                    }
                }
            }   
        ]
    }).data("kendoGrid");

    grid.table.on("click", ".checkbox", selectRow);

    function onDataBound(e) {
        var grid = $("#report-grid-loan").data("kendoGrid");
        var data = grid.dataSource.data();

        $.each(data, function (i, row) {
            if (row.pasokhState == 1) {
                var element = $('tr[data-uid="' + row.uid + '"] ');
                $(element).addClass("bg-green");
            } else if (row.pasokhState == 2) {
                var element = $('tr[data-uid="' + row.uid + '"] ');
                $(element).addClass("bg-error");
            }
        });
    }

});

function reportTypeChanged(e) {
    var grid = $('#report-grid-loan');
    grid.data('kendoGrid').dataSource.read();
    grid.data('kendoGrid').refresh();
}