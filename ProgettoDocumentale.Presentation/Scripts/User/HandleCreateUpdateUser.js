function handleCreateUpdateUser(html) {
    $('#appModalContent').empty().html(html);

    var success = $('#appModalContent').find('[data-success]').first().attr('data-success');
    $('#appModalContent .selectpicker').selectpicker();

    if (success === '1') {
        $('#appModal').modal('hide');        

        usersTable?.ajax.reload(null, false);
    }
}
