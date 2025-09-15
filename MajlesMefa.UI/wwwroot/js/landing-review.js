$(function () {
    $("#wizard").steps({
        headerTag: "h2",
        bodyTag: "section",
        transitionEffect: "fade",
        enableAllSteps: false,
        transitionEffectSpeed: 500,
        enablePagination: false,
        titleTemplate: "#title#",
        cssClass: "tabcontrol",
        //onStepChanging: function (event, currentIndex, newIndex) {
        //    //Allways allow user to move backwards.
        //    if (currentIndex > newIndex) {
        //        return true;
        //    }
        //    // Remove the validation errors from the next step, incase user has previously visited it.
        //    var form = $('#frmStepOne');
        //    if (currentIndex < newIndex) {
        //        // remove error styles
        //        $(".body:eq(" + newIndex + ") label.error", form).remove();
        //        $(".body:eq(" + newIndex + ") .error", form).removeClass("error");
        //    }
        //    //disable validation on fields that are disabled or hidden.
        //    form.validate().settings.ignore = ":disabled,:hidden";
        //    return form.valid();
        //}
    });
});