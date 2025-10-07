$(document).ready(function () {
    // Guard: required globals provided by the Razor view
    if (typeof url_get_senatorbudget === "undefined" || url_get_senatorbudget === '') {
        toastr.error('شما مجوز مشاهده صفحه را ندارید');
        return;
    }

    // Build data source like mokatebe.js
    const dataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: url_get_senatorbudget,
                dataType: "json",
                type: "POST"
            },
            parameterMap: function (options) {
                // Consistent with other pages: send as models=<json>
                return `models=${kendo.stringify(options)}`;
            }
        },
        schema: {
            data: "data",
            total: "total"
        },
        error: function (e) {
            console.error("SenatorBudget Grid error:", e);
            alert(e.errorThrown || 'خطایی رخ داده است');
        },
        pageSize: 15,
        sort: { field: "id", dir: "desc" },
        serverPaging: true,
        serverFiltering: true,
        serverSorting: true
    });

    // Initialize grid
    $("#report-grid-senatorbudget").kendoGrid({
        dataSource: dataSource,
        autoBind: true,
        height: 600,
        scrollable: true,
        resizable: true,
        sortable: true,
        filterable: {
            extra: false
        },
        pageable: {
            refresh: true,
            pageSizes: true,
            buttonCount: 5
        },
        noRecords: {
            template: `<div class="mt-4 mb-4"><p class="text-danger">داده ای یافت نشد</p></div>`
        },
        columns: [
            { field: "senatorName", title: "نماینده", width: "150px" },
            { field: "userName", title: "کاربر", width: "150px" },
            {
                field: "amount",
                title: "مبلغ بودجه",
                width: "150px",
                template: "#= kendo.toString(amount, 'n0') #"
            },
            { field: "execDate", title: "تاریخ اجرا", width: "120px" },
            { field: "loanTypeName", title: "نوع وام", width: "120px" },
            {
                template: btns,
                title: "عملیات",
                width: "200px"
            }
        ],
        toolbar: (typeof url_create !== "undefined" && url_create !== '')
            ? [{ name: "create", text: "ایجاد جدید" }]
            : []
    });

    // Create button redirects (if present)
    if (typeof url_create !== "undefined" && url_create !== '') {
        $(document).on("click", ".k-grid-add", function (e) {
            e.preventDefault();
            window.location.href = url_create;
        });
    }

    // Handle action buttons (edit/delete) via data-link
    $(document).on('click', '[data-link]', function (e) {
        e.preventDefault();
        var link = $(this).attr('data-link');
        window.location.href = link;
    });

    // Sanity checks
    if (typeof $.fn.kendoGrid !== "function") {
        console.error('kendoGrid plugin is not loaded.');
    }
    if (!$("#report-grid-senatorbudget").data("kendoGrid")) {
        console.error('#report-grid-senatorbudget was not initialized as a Kendo Grid.');
    }
});