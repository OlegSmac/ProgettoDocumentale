$(document).on('shown.bs.modal', '#appModal', function () {
    DocumentForm.bindOnce();
    DocumentForm.init();
});
