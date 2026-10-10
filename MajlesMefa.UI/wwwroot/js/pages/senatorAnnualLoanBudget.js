$(function () {
    var readUrl = annualBudgetReadUrl;
    if (annualBudgetSenatorId && annualBudgetSenatorId !== '00000000-0000-0000-0000-000000000000')
        readUrl += (readUrl.indexOf('?') >= 0 ? '&' : '?') + 'senatorId=' + encodeURIComponent(annualBudgetSenatorId);

    var ds = new kendo.data.DataSource({
        transport: {
            read: {
                url: readUrl,
                type: 'POST',
                dataType: 'json'
            },
            parameterMap: function (options) { return { models: kendo.stringify(options) }; }
        },
        schema: { data: 'data', total: 'total' },
        pageSize: 15,
        serverPaging: true,
        serverFiltering: true,
        serverSorting: true
    });

    var searchTimer;
    $('#annual-budget-name-search').on('input', function () {
        clearTimeout(searchTimer);
        var value = this.value;
        searchTimer = setTimeout(function () {
            ds.filter(value ? { field: 'senatorName', operator: 'contains', value: value } : {});
        }, 250);
    });

    $('#annual-loan-budget-grid').kendoGrid({
        dataSource: ds,
        pageable: { refresh: true, pageSizes: [10, 15, 25, 50] },
        sortable: true,
        filterable: { extra: false, operators: { string: { contains: 'شامل' } } },
        noRecords: { template: '<div class="p-3 text-muted">سقفی ثبت نشده است.</div>' },
        columns: [
            { field: 'senatorName', title: 'نماینده', width: 190 },
            { field: 'year', title: 'سال شمسی', width: 100 },
            { field: 'gharzolHasaneLimit', title: 'سقف قرض‌الحسنه (تومان)', template: "#= kendo.toString(gharzolHasaneLimit, 'n0') #" },
            { field: 'gharzolHasaneUsed', title: 'مصرف قرض‌الحسنه (تومان)', template: "#= kendo.toString(gharzolHasaneUsed, 'n0') #" },
            { field: 'morabeheLimit', title: 'سقف مرابحه (تومان)', template: "#= kendo.toString(morabeheLimit, 'n0') #" },
            { field: 'morabeheUsed', title: 'مصرف مرابحه (تومان)', template: "#= kendo.toString(morabeheUsed, 'n0') #" },
            { title: 'عملیات', width: 170, template: function (item) {
                return '<a class="k-button" href="' + annualBudgetEditUrl + '?id=' + item.id + '">ویرایش</a> ' +
                    '<button class="k-button js-delete-annual-budget" data-id="' + item.id + '">حذف</button>';
            }}
        ]
    });

    function submitBulkForm(formSelector) {
        $(formSelector).on('submit', function (e) {
            e.preventDefault();
            var form = this;
            var confirmText = formSelector === '#bulk-create-form'
                ? 'برای همهٔ نماینده‌های فاقد رکورد در این سال سقف ثبت شود؟'
                : 'تغییر سقف‌های همهٔ نماینده‌هایی که در این سال رکورد دارند اعمال شود؟';
            if (!window.confirm(confirmText)) return;
            $.ajax({ url: form.action, type: 'POST', data: $(form).serialize() })
                .done(function (result) {
                    if (window.toastr) toastr.success(result.message);
                    else alert(result.message);
                    $('#annual-loan-budget-grid').data('kendoGrid').dataSource.read();
                    form.reset();
                    $(form).find('input[name="Year"]').val(annualBudgetDefaultYear);
                })
                .fail(function (xhr) {
                    var message = xhr.responseJSON && xhr.responseJSON.message ? xhr.responseJSON.message : 'عملیات گروهی انجام نشد.';
                    if (window.toastr) toastr.error(message);
                    else alert(message);
                });
        });
    }
    submitBulkForm('#bulk-create-form');
    submitBulkForm('#bulk-adjust-form');

    $(document).on('click', '.js-delete-annual-budget', function () {
        var id = $(this).data('id');
        if (!confirm('این سقف سالانه حذف شود؟')) return;
        $.ajax({
            url: annualBudgetDeleteUrl,
            type: 'POST',
            data: { id: id, __RequestVerificationToken: annualBudgetToken },
            success: function () { $('#annual-loan-budget-grid').data('kendoGrid').dataSource.read(); },
            error: function (xhr) { alert(xhr.responseText || 'حذف سقف ناموفق بود.'); }
        });
    });
});
