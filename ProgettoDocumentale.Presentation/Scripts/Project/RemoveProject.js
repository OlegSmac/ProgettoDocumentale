function toggleRemoveProject(id) {
    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: '/CedacriOperator/RemoveProject',
        type: 'POST',
        data: { id: id, __RequestVerificationToken: token },
        success: function (res) {
            if (res && res.success) {
                if (window.projectsTable) window.projectsTable.ajax.reload(null, false);
                loadProjectsHierarchy();
            } else {
                alert(res.message || 'Failed to remove institution');
            }
        },
        error: function () {
            alert('Server error while changing status');
        }
    });
}
