function handleCreateUpdateUser(html) {
    const $modal = $(".modal.show");
    $modal.find(".modal-content").replaceWith(html);

    const $content = $modal.find(".modal-content").first();

    $('.selectpicker').selectpicker();

    if ($content.attr("data-success") === "1") {
        const modal = bootstrap.Modal.getInstance($modal[0]) || new bootstrap.Modal($modal[0]);
        modal.hide();

        userTable?.ajax.reload(null, false);
    }
}

function handleResetPassword(response) {
    $('#resetPasswordError')
        .addClass('d-none')
        .text('');

    if (!response || response.success === false) {
        if (response.errors) {
            $('.validation-message').text('');
            $('.is-invalid').removeClass('is-invalid');

            Object.keys(response.errors).forEach(function (key) {
                $('span[data-valmsg-for="' + key + '"]').text(response.errors[key]);
                $('[name="' + key + '"]').addClass('is-invalid');
            });
            return;
        }

        if (response.message) {
            $('#resetPasswordError')
                .removeClass('d-none')
                .text(response.message);
        }

        return;
    }

    bootstrap.Modal.getInstance(
        document.getElementById('resetPasswordModal')
    ).hide();
}
