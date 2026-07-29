$(document).ready(function () {
    var selectedIds = [];
    var allPagesSelected = false;

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

    $("#batchStatusDropDown").kendoDropDownList({
        dataSource: [
            { text: "در دست اقدام", value: 0 },
            { text: "پرداخت شده", value: 1 },
            { text: "رد درخواست", value: 2 },
            { text: "در دست شعبه", value: 3 }
        ],
        dataTextField: "text",
        dataValueField: "value",
        valuePrimitive: true,
        optionLabel: "انتخاب وضعیت"
    });

    // مقداردهی دراپ‌داون نوع اقدام بر اساس متغیر سراسری سرور
    $("#batchActionTypeDropDown").kendoDropDownList({
        dataSource: typeof refTypeDataSource !== 'undefined' ? refTypeDataSource : [],
        dataTextField: "Text",
        dataValueField: "Value",
        valuePrimitive: true,
        optionLabel: {
            Text: "انتخاب کنید...",
            Value: ""
        }
    });

    // مقداردهی دراپ‌داون سال
    $("#LoanYearDropDown").kendoDropDownList({
        dataSource: typeof loanYearDataSource !== 'undefined' ? loanYearDataSource : [],
        dataTextField: "Text",
        dataValueField: "Value",
        valuePrimitive: true,
        optionLabel: "همه"
    });

    var batchStatusWindow = $("#batchStatusWindow").kendoWindow({
        width: 420,
        title: "تغییر وضعیت گروهی",
        visible: false,
        modal: true,
        actions: ["Close"],
        resizable: false
    }).data("kendoWindow");

    var batchActionWindow = $("#batchActionWindow").kendoWindow({
        width: 520,
        title: "ثبت اقدام گروهی",
        visible: false,
        modal: true,
        actions: ["Close"],
        resizable: false
    }).data("kendoWindow");

    $("#LoanTypeDropDown").on("change", function (e) {
        var filters = dataSource._filter?.filters ?? [];
        if (e.target.value) {
            filters = filters.filter(x => x.field != "loanType");
            filters.push({ field: "loanType", value: parseInt(e.target.value), operator: "eq" });
            dataSource.filter(filters).read();
        } else {
            dataSource.filter(filters.filter(x => x.field != "loanType")).read();
        }
    });

    $("#PasokhStateDropDown").on("change", function (e) {
        var filters = dataSource._filter?.filters ?? [];
        if (e.target.value) {
            filters = filters.filter(x => x.field != "pasokhState");
            filters.push({ field: "pasokhState", value: parseInt(e.target.value), operator: "eq" });
            dataSource.filter(filters).read();
        } else {
            dataSource.filter(filters.filter(x => x.field != "pasokhState")).read();
        }
    });

    // مدیریت تغییرات دراپ‌داون سال با ساختار کاملاً مشابه بقیه فیلترها
    $("#LoanYearDropDown").on("change", function (e) {
        var filters = dataSource._filter?.filters ?? [];
        if (e.target.value) {
            filters = filters.filter(x => x.field != "year");
            filters.push({ field: "year", value: parseInt(e.target.value), operator: "eq" });
            dataSource.filter(filters).read();
        } else {
            dataSource.filter(filters.filter(x => x.field != "year")).read();
        }
    });

    window.record = 0;
    const dataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: url_get_loan,
                dataType: "json",
                type: "Post"
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
            pageSizes: [5, 10, 15, 20, 100, 200],
            info: true
        },
        sortable: true,
        filterable: {
            extra: false,
            operators: {
                string: {
                    contains: "شامل میشود",
                    startswith: "شروع میشود با ",
                    eq: "برابر"
                },
                enums: {
                    eq: "Equal to",
                    neq: "Not equal to"
                }
            }
        },
        toolbar: fromReportPage ? [] : [
            {
                template: `<div class="d-flex w-auto align-items-center">
                    #if('${url_create}' != ''){#<div style="margin-right: 10px;margin-left:10px;"><a class="k-button" href="${url_create}">ایجاد تسهیلات</a></div>#}#
                </div>`
            },
            "excel"
        ],
        excel: {
            fileName: "loans.xlsx",
            filterable: true,
            allPages: true
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
                width: 50,
                template: "<input type='checkbox' class='row-checkbox' data-id='#:id#' />",
                headerTemplate: "<input type='checkbox' id='header-chb' />",
                filterable: false,
                sortable: false,
                attributes: { style: "text-align: center" }
            },
            {
                title: "ردیف",
                template: "#: ++record #",
                width: 50,
                attributes: { class: "text-center" },
                media: "(min-width: 200px)",
                filterable: false,
                sortable: false
            },
            { field: "id", hidden: true },
            {
                field: "trackingCode",
                title: "شناسه یکتا",
                media: "(min-width: 200px)",
                width: "105px",
                filterable: { cell: { operator: "contains", showOperators: false } }
            },
            {
                field: "senatorName",
                title: "نام نماینده",
                media: "(min-width: 200px)",
                width: "120px",
                filterable: { cell: { operator: "contains", showOperators: false } }
            },
            {
                field: "senatorCity",
                title: "استان نماینده",
                media: "(min-width: 200px)",
                width: "120px",
                filterable: { cell: { operator: "contains", showOperators: false } }
            },
            {
                field: "senatorHozeEntekhabi",
                title: "حوزه انتخابی",
                media: "(min-width: 200px)",
                width: "120px",
                filterable: { cell: { operator: "contains", showOperators: false } }
            },
            {
                field: "loanOwnerFullName",
                title: "نام متقاضی",
                media: "(min-width: 400px)",
                width: "120px",
                filterable: { cell: { operator: "contains", showOperators: false } }
            },
            {
                field: "loanOwnerMobile",
                title: "موبایل متقاضی",
                media: "(min-width: 400px)",
                width: "120px",
                filterable: { cell: { operator: "contains", showOperators: false } }
            },
            {
                field: "loanOwnerNationalCode",
                title: "کدملی متقاضی",
                media: "(min-width: 400px)",
                width: "120px",
                filterable: { cell: { operator: "contains", showOperators: false } }
            },
            {
                field: "persianCreatedDate",
                title: "تاریخ درخواست",
                media: "(min-width: 200px)",
                width: "120px",
                filterable: { cell: { operator: "contains", showOperators: false } }
            },
            {
                field: "actionRefrenceDate",
                title: "تاریخ ارجا به شعبه",
                media: "(min-width: 200px)",
                width: "130px",
                filterable: { cell: { operator: "contains", showOperators: false } }
            },
            {
                field: "amount",
                title: "مبلغ تسهیلات(تومان)",
                media: "(min-width: 200px)",
                width: "150px",
                filterable: { cell: { operator: "contains", showOperators: false } }
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
                field: "description",
                title: "توضیحات",
                media: "(min-width: 400px)",
                width: "150px",
                filterable: { cell: { operator: "contains", showOperators: false } }
            },
            {
                field: "pasokhStateDesc",
                title: "وضعیت",
                media: "(min-width: 400px)",
                width: "120px",
                filterable: false,
                sortable: false
            },
            { field: "canAccessActionRefrence", hidden: true },
            { field: "pasokhState", title: "وضعیت", hidden: true, width: "105px" },
            {
                field: "suggestedBankName",
                title: "بانک عامل",
                media: "(min-width: 200px)",
                width: "120px",
                filterable: { cell: { operator: "contains", showOperators: false } }
            },
            {
                title: "عملیات",
                width: fromReportPage ? 120 : 270,
                attributes: { "class": "text-center", style: "text-align: center" },
                template: fromReportPage
                    ? `<button class="k-button" data-link="${url_details_loan}/?loanId=#:id#">جزییات</button>`
                    : btns,
                filterable: false,
                sortable: false
            }
        ]
    }).data("kendoGrid");

    $("#report-grid-loan").on("change", "#header-chb", function () {
        var checked = $(this).is(":checked");

        if (allPagesSelected && !checked) {
            clearSelection();
            return;
        }

        allPagesSelected = false;
        $(".row-checkbox").each(function () {
            $(this).prop("checked", checked);
            handleRowSelection($(this).data("id"), checked);
        });

        updateBatchUI();
    });

    $("#report-grid-loan").on("change", ".row-checkbox", function () {
        var id = $(this).data("id");
        var checked = $(this).is(":checked");

        if (allPagesSelected && !checked) {
            allPagesSelected = false;
            var pageData = grid.dataSource.view();
            selectedIds = [];
            $.each(pageData, function (i, row) {
                if (row.id !== id) {
                    selectedIds.push(row.id);
                }
            });
        } else {
            handleRowSelection(id, checked);
        }

        var allCheckedOnPage = $(".row-checkbox").length === $(".row-checkbox:checked").length;
        $("#header-chb").prop("checked", allCheckedOnPage);
        updateBatchUI();
    });

    $(document).on("click", "#selectAllPagesBtn", function () {
        allPagesSelected = true;
        selectedIds = [];
        $(".row-checkbox").prop("checked", true);
        $("#header-chb").prop("checked", true);
        updateBatchUI();
    });

    $(document).on("click", "#clearSelectionBtn", function () {
        clearSelection();
    });

    $(document).on("click", "#batchStatusBtn", function () {
        if (!hasSelection()) {
            alert("حداقل یک مورد را انتخاب کنید.");
            return;
        }

        $("#batchStatusDropDown").data("kendoDropDownList").value("");
        batchStatusWindow.center().open();
    });

    $(document).on("click", "#batchRegisterActionBtn", function () {
        if (!hasSelection()) {
            alert("حداقل یک مورد را انتخاب کنید.");
            return;
        }

        $("#batchActionTypeDropDown").data("kendoDropDownList").value("");
        $("#batchActionDescription").val("");
        batchActionWindow.center().open();
    });

    $(document).on("click", "#cancelBatchStatusBtn", function () {
        batchStatusWindow.close();
    });

    $(document).on("click", "#cancelBatchActionBtn", function () {
        batchActionWindow.close();
    });

    $(document).on("click", "#submitBatchStatusBtn", function () {
        var status = $("#batchStatusDropDown").data("kendoDropDownList").value();
        if (status === "" || status === null || status === undefined) {
            alert("وضعیت را انتخاب کنید.");
            return;
        }

        var payload = buildBatchPayload();
        payload.status = parseInt(status);

        $.ajax({
            url: url_batch_change_status,
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(payload),
            success: function () {
                batchStatusWindow.close();
                alert("تغییر وضعیت گروهی با موفقیت انجام شد.");
                clearSelection();
                grid.dataSource.read();
            },
            error: function (xhr) {
                alert("خطا در تغییر وضعیت گروهی: " + xhr.responseText);
            }
        });
    });

    $(document).on("click", "#submitBatchActionBtn", function () {
        var actionType = $("#batchActionTypeDropDown").data("kendoDropDownList").value();
        var description = ($("#batchActionDescription").val() || "").trim();

        if (actionType === "" || actionType === null || actionType === undefined) {
            alert("نوع اقدام را انتخاب کنید.");
            return;
        }

        if (!description) {
            alert("شرح اقدام را وارد کنید.");
            return;
        }

        var payload = buildBatchPayload();
        payload.actionType = parseInt(actionType);
        payload.description = description;

        $.ajax({
            url: url_batch_register_action,
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(payload),
            success: function () {
                batchActionWindow.close();
                alert("اقدام گروهی با موفقیت ثبت شد.");
                clearSelection();
                grid.dataSource.read();
            },
            error: function (xhr) {
                alert("خطا در ثبت اقدام گروهی: " + xhr.responseText);
            }
        });
    });

    function handleRowSelection(id, isSelected) {
        var index = selectedIds.indexOf(id);
        if (isSelected) {
            if (index === -1) {
                selectedIds.push(id);
            }
        } else if (index !== -1) {
            selectedIds.splice(index, 1);
        }
    }

    function hasSelection() {
        return allPagesSelected || selectedIds.length > 0;
    }

    function buildBatchPayload() {
        var options = grid.dataSource.transport.parameterMap({
            filter: grid.dataSource.filter(),
            sort: grid.dataSource.sort(),
            group: grid.dataSource.group()
        });

        var tableRequestModel = null;
        if (options && options.models) {
            var parsed = JSON.parse(options.models.replace("models=", ""));
            tableRequestModel = parsed;
        } else {
            tableRequestModel = {
                filter: grid.dataSource.filter(),
                sort: grid.dataSource.sort()
            };
        }

        var currentSenatorId = typeof senatorId !== 'undefined' ? senatorId : null;

        return {
            allSelected: allPagesSelected,
            selectedIds: selectedIds,
            filter: tableRequestModel,
            senatorId: currentSenatorId
        };
    }

    function clearSelection() {
        selectedIds = [];
        allPagesSelected = false;
        $(".row-checkbox").prop("checked", false);
        $("#header-chb").prop("checked", false);
        updateBatchUI();
    }

    function updateBatchUI() {
        var totalRecords = grid.dataSource.total();
        var count = allPagesSelected ? totalRecords : selectedIds.length;

        if (count > 0) {
            $("#batchActionContainer").attr("style", "display: flex !important;");
            $("#selectedCount").text(count);
        } else {
            $("#batchActionContainer").attr("style", "display: none !important;");
        }
    }

    function onDataBound() {
        var grid = $("#report-grid-loan").data("kendoGrid");
        var data = grid.dataSource.data();

        var allCheckedOnPage = data.length > 0;
        $.each(data, function (i, row) {
            var isSelected = allPagesSelected || selectedIds.indexOf(row.id) !== -1;
            var checkbox = $('tr[data-uid="' + row.uid + '"] .row-checkbox');
            checkbox.prop("checked", isSelected);

            if (!isSelected) {
                allCheckedOnPage = false;
            }

            var element = $('tr[data-uid="' + row.uid + '"]');
            if (row.pasokhState == 1) {
                $(element).addClass("bg-green");
            } else if (row.pasokhState == 2) {
                $(element).addClass("bg-error");
            }
        });

        $("#header-chb").prop("checked", allCheckedOnPage);

        if (allPagesSelected) {
            $("#header-chb").prop("checked", true);
        }
    }
});

function reportTypeChanged() {
    var grid = $("#report-grid-loan");
    grid.data("kendoGrid").dataSource.read();
    grid.data("kendoGrid").refresh();
}
