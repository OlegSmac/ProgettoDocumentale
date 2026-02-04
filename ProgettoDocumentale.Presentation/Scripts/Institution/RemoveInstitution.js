function toggleRemoveInstitution(id) {
    if (!confirm('Are you sure you want to remove this institution?')) {
        return;
    }

    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: '/Admin/RemoveInstitution',
        type: 'POST',
        data: { id: id, __RequestVerificationToken: token },
        success: function (res) {
            if (res && res.success) {
                if (institutionsTable) institutionsTable.ajax.reload(null, false);
            } else {
                alert(res.message || 'Failed to remove institution');
            }
        },
        error: function () {
            alert('Server error while changing status');
        }
    });
}
