function setTypeId(val) {
    $('#TypeId').val(val || '');
}

function clearAndDisable($sel, placeholder) {
    $sel.val('');
    $sel.prop('disabled', true);
    $sel.find('option').remove();
    $sel.append($('<option>', { value: '', text: placeholder || 'Select' }));
}

function loadMicroTypes(macroId, selectedMicroId) {
    const $micro = $('#MicroTypeId');

    if (!macroId) {
        clearAndDisable($micro, 'Select micro type');
        return;
    }

    $micro.prop('disabled', true).empty().append($('<option>', { value: '', text: 'Loading...' }));

    $.getJSON('/CedacriOperator/GetMicroTypes', { macroTypeId: macroId })
        .done(function (items) {
            $micro.empty().append($('<option>', { value: '', text: 'Select micro type' }));
            (items || []).forEach(function (it) {
                $micro.append($('<option>', { value: it.Id, text: it.Name }));
            });
            if (selectedMicroId) {
                $micro.val(String(selectedMicroId));
            }

            $micro.prop('disabled', false);
            setTypeId($micro.val());
        })
        .fail(function () {
            clearAndDisable($micro, 'Failed to load micro types');
        });
}

function applyMacroRules() {
    const $macro = $('#MacroTypeId');
    const macroId = $macro.val();
    const code = $macro.find(':selected').attr('data-code');

    const $microRow = $('#microRow');
    const $micro = $('#MicroTypeId');

    const $projectRow = $('#projectRow');
    const $project = $('#ProjectId');

    if (!macroId) {
        $microRow.hide();
        $projectRow.hide();
        clearAndDisable($micro, 'Select micro type');
        $project.val('').prop('disabled', true);
        setTypeId('');
        return;
    }

    if (code === 'SLA_REPORT') {
        $microRow.hide();
        $projectRow.hide();

        clearAndDisable($micro, 'Select micro type');
        $micro.val('');
        $project.val('').prop('disabled', true);

        setTypeId(macroId);
        return;
    }

    $microRow.show();
    const selectedMicroId = $('#MicroTypeId').val();
    loadMicroTypes(macroId, selectedMicroId);

    if (code === 'PROGETTAZIONE') {
        $projectRow.show();
        $project.prop('disabled', false);
    } else {
        $projectRow.hide();
        $project.val('').prop('disabled', true);
    }
}

$(document).off('change.addDocument', '#MacroTypeId');
$(document).on('change.addDocument', '#MacroTypeId', function () {
    $('#MicroTypeId').val('');
    setTypeId('');
    applyMacroRules();
});

$(document).off('change.addDocument', '#MicroTypeId');
$(document).on('change.addDocument', '#MicroTypeId', function () {
    setTypeId($(this).val());
});

$(document).off('submit.addDocument', '#documentForm');
$(document).on('submit.addDocument', '#documentForm', function (e) {
    e.preventDefault();

    const form = this;
    const fd = new FormData(form);

    $.ajax({
        url: form.action,
        type: 'POST',
        data: fd,
        processData: false,
        contentType: false,
        success: function (html) {
            handleCreateUpdateDocument(html);
            bindDocumentFormAjax();
        },
        error: function () {
            alert('Failed to submit document.');
        }
    });
});

$(function () {
    applyMacroRules();
    const microSelected = $('#MicroTypeId').val();
    if (microSelected) setTypeId(microSelected);
});