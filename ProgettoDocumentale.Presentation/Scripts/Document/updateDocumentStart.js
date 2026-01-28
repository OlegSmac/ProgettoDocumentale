$(document).on('shown.bs.modal', '#updateDocumentModal', function () {
    DocumentForm.bindOnce();
    DocumentForm.init();
});
