function openModal(options) {
    $('#appModalContent').empty().html('<div class="text-muted">Loading...</div>');

    $.ajax({
        url: options.url,
        type: 'GET',
        cache: false,
        data: options.data != null ? { id: options.data } : {},
    })
    .done(function (html) {
        $('#appModalContent').empty().html(html);
        if ($.fn.selectpicker) {
            $('#appModalContent .selectpicker').selectpicker();
        }

        $('#appModal').modal('show');
    })
    .fail(function () {
        $('#appModalContent').empty();
        alert("Failed to load modal content.");
    });
}