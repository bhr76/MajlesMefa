$(document).ready(function () {

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
                    startswith: "شروع میشود با",
                    eq: "برابر",
                },
                enums: {
                    eq: "برابر",
                    neq: "نابرابر"
                }
            }
        },
        toolbar: fromReportPage ? [] : [
            {
                template: `<div class="d-flex w-auto">
                    #if('${url_create}' != ''){#
                        <div style="margin-right: 10px;margin-left:10px;margin-top:7px;">
                            <a class="k-button" href="${url_create}">ایجاد تسهیلات</a>
                        </div>
                    #}#
                </div>`
            },
            'excel'
        ],
        excel: {
            fileName: "loans.xlsx",
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
                sortable: false
            },
            {
                field: "id",
                hidden: true
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
                title: "تاریخ ارجا به بانک",
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
                filterable: {
                    cell: {
                        operator: "contains",
                        showOperators: false
                    }
                }
            },
            {
                field: "description",
                title: "توضیحات",
                media: "(min-width: 400px)",
                width: "150px",
                filterable: {
                    cell: {
                        operator: "contains",
                        showOperators: false
                    }
                }
            },
            {
                field: "pasokhStateDesc",
                title: "وضعیت",
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
                field: "canAccessActionRefrence",
                hidden: true
            },
            {
                field: "pasokhState",
                title: "وضعیت",
                hidden: true,
                width: "105px"
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
            },
            {
                title: "عملیات",
                width: fromReportPage ? 120 : 270,
                attributes: {
                    "class": "text-center",
                    style: "text-align: center"
                },
                template: fromReportPage ?
                    `<button class="k-button" data-link="${url_details_loan}/?loanId=#:id#">جزییات</button>`
                    : btns,
                filterable: false,
                sortable: false
            }
        ]
    }).data("kendoGrid");

    grid.table.on("click", ".checkbox", selectRow);

    function onDataBound(e) {
        var grid = $("#report-grid-loan").data("kendoGrid");
        var data = grid.dataSource.data();

        $.each(data, function (i, row) {
            if (row.pasokhState == 1) { // Accepted
                var element = $('tr[data-uid="' + row.uid + '"] ');
                $(element).addClass("bg-green");
            } else if (row.pasokhState == 2) { // Rejected
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