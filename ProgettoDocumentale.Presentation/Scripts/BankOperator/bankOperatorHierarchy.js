$(document).on('click', 'button[data-bs-toggle="collapse"]', function () {
    var $icon = $(this).find('.hierarchy-toggle-icon');    
    
    if ($icon.hasClass('bi-plus-square')) {
        $icon.removeClass('bi-plus-square').addClass('bi-dash-square');
    } else {
        $icon.removeClass('bi-dash-square').addClass('bi-plus-square');
    }
});

$(document).on('click', '.hierarchy-all-filter', function (e) {    
    var $hierarchy = $(this).closest('[id$="Hierarchy"]');
    if ($hierarchy.length === 0) return;
    
    $hierarchy.find('.collapse.show').each(function () {
        bootstrap.Collapse.getOrCreateInstance(this, { toggle: false }).hide();
    });
    
    $hierarchy.find('.hierarchy-toggle-icon')
        .removeClass('bi-dash-square')
        .addClass('bi-plus-square');
});

function bindReportContextMenu(selector) {
    $.contextMenu({
        selector: selector,
        trigger: 'right',
        callback: function (key, options) {
            var id = options.$trigger.data("id");

            switch (key) {
                case 'Info':
                    openModal({
                        data: id,
                        url: '/BankOperator/GetDocumentDetails'
                    });
                    break;

                case 'Download':
                    window.location = '/BankOperator/DownloadDocument?id=' + encodeURIComponent(id);
                    break;
            }
        },
        items: {
            "Info": { name: "Info" },
            "Download": { name: "Download" }
        }
    });
}

$(document).on('dblclick', '.report-item', function (e) {
    var id = $(this).data("id");
    if (!id) return;

    openModal({
        data: id,
        url: '/BankOperator/GetDocumentDetails'
    });
});
