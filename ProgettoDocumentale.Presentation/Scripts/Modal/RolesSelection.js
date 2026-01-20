function moveSelected($from, $to) {
    $from.find('option:selected').each(function () {
        var value = this.value;
        if ($to.find('option[value="' + value + '"]').length === 0) {
            $(this).prop('selected', true).appendTo($to);
        }
    });
}

$(document).on('click', '#btnAddRole', function () {
    var $form = $(this).closest('form');
    moveSelected($form.find('#AvailableRoles'), $form.find('#SelectedRoles'));
});

$(document).on('click', '#btnRemoveRole', function () {
    var $form = $(this).closest('form');
    moveSelected($form.find('#SelectedRoles'), $form.find('#AvailableRoles'));
});

$(document).on('click', '#userForm button[type="submit"]', function () {
    $(this).closest('form').find('#SelectedRoles option').prop('selected', true);
});
