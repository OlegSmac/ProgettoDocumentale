function loadProjectsHierarchy() {
    $.getJSON("/CedacriOperator/GetProjectsHierarchy", function (items) {
        var html = `
            <ul class="list-unstyled mb-0">
                <li class="mb-2">
                    <a href="#"
                        class="text-decoration-none text-dark fw-semibold all-projects-filter">
                        <i class="bi bi-list-ul me-2"></i>
                        All projects
                    </a>
                </li>
                <li><hr class="my-2"></li>
        `;

        items.forEach(function (inst) {
            var instCollapseId = 'inst-' + inst.InstitutionId;

            html += `
                <li class="mb-2">
                    <div class="d-flex align-items-center gap-2">

                        <button type="button"
                                class="btn btn-link p-0 text-decoration-none fw-semibold text-dark"
                                data-bs-toggle="collapse"
                                data-bs-target="#${instCollapseId}",
                                aria-expanded="false">
                            <i class="bi bi-plus-square hierarchy-toggle-icon"></i>                        
                        </button>

                        <a href="#" class="text-decoration-none text-dark fw-semibold hierarchy-inst-filter"
                                data-institution-id="${inst.InstitutionId}">
                            ${inst.InstitutionName}
                        </a>

                    </div>

                    <div class="collapse mt-1" id="${instCollapseId}">
                        <ul class="list-unstyled ps-4 mb-0">
                            ${(inst.Years || []).map(y => `
                                <li class="mb-1">
                                    <a href="#" class="text-decoration-none text-dark year-filter"
                                            data-institution-id="${inst.InstitutionId}"
                                            data-year="${y}">
                                        ${y}
                                    </a>
                                </li>
                            `).join('')}
                        </ul>
                    </div>
                </li>`;
        });

        html += '</ul>';
        $('#projectsHierarchy').html(html);
    });
}

$(document).on('click', '.year-filter', function (e) {
    e.preventDefault();

    selectedInstitutionId = parseInt($(this).data('institution-id'), 10);
    selectedYear = parseInt($(this).data('year'), 10);

    projectsTable.ajax.reload();
});


