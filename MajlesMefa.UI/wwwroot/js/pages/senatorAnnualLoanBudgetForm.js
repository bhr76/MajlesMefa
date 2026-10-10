$(function () {
    $('form').on('submit', function () {
        $('.amount-input').each(function () { this.value = (this.value || '').replace(/[,،s]/g, ''); });
    });
});
