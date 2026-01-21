function handleResetPassword(html) {
    const $modal = $(".modal.show");
    $modal.find(".modal-content").replaceWith(html);

    const $content = $modal.find(".modal-content").first();

    if ($content.attr("data-success") === "1") {
        const modal = bootstrap.Modal.getInstance($modal[0]) || new bootstrap.Modal($modal[0]);
        modal.hide();

        userTable?.ajax.reload(null, false);
    }
}