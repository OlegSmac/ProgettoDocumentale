function openModal(options) {
    $.get(options.url, { id: options.data })
        .done(function (html) {
            $(options.content).empty();
            $(options.content).html(html);
            console.log(html);
            $(options.target).modal('show');

            $('.selectpicker').selectpicker();
        })
        .fail(function () {
            alert("Failed to load modal content.");
        });
}