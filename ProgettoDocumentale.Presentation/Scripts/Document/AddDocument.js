(() => {
    const $macro = () => $('#MacroTypeId');
    const $micro = () => $('#MicroTypeId');
    const $proj = () => $('#ProjectId');

    const setHiddenTypeId = () => {
        const code = $macro().find(':selected').data('code');
        const val = !code ? '' : (code === 'SLA_REPORT' ? $macro().val() : $micro().val());
        $('#TypeId').val(val || '');
    };

    const toggleRow = (rowId, on) => $(rowId).toggle(!!on);
    const disableSelect = ($s, placeholder) => {
        $s.prop('disabled', true).html(`<option value="">${placeholder}</option>`).val('');
    };

    const loadMicro = (macroId) => {
        const $m = $micro();
        if (!macroId) return disableSelect($m, 'Select micro type');

        disableSelect($m, 'Loading...');
        $.getJSON('/CedacriOperator/GetMicroTypes', { macroTypeId: macroId })
            .done(items => {
                $m.prop('disabled', false).html('<option value="">Select micro type</option>');
                (items || []).forEach(it => $m.append(new Option(it.Name, it.Id)));
            })
            .fail(() => disableSelect($m, 'Failed to load micro types'));
    };

    const applyRules = () => {
        const macroId = $macro().val();
        const code = $macro().find(':selected').data('code');

        if (!macroId) {
            toggleRow('#microRow', false);
            toggleRow('#projectRow', false);
            disableSelect($micro(), 'Select micro type');
            $proj().val('').prop('disabled', true);
            setHiddenTypeId();
            return;
        }

        if (code === 'SLA_REPORT') {
            toggleRow('#microRow', false);
            toggleRow('#projectRow', false);
            disableSelect($micro(), 'Select micro type');
            $proj().val('').prop('disabled', true);
            setHiddenTypeId();
            return;
        }

        toggleRow('#microRow', true);
        loadMicro(macroId);

        const needsProject = (code === 'PROGETTAZIONE');
        toggleRow('#projectRow', needsProject);
        $proj().prop('disabled', !needsProject);
        if (!needsProject) $proj().val('');

        setHiddenTypeId();
    };
    
    $(document)
        .off('change.addDocument', '#MacroTypeId')
        .on('change.addDocument', '#MacroTypeId', () => applyRules())
        .off('change.addDocument', '#MicroTypeId')
        .on('change.addDocument', '#MicroTypeId', () => setHiddenTypeId())
        .off('submit.addDocument', '#documentForm')
        .on('submit.addDocument', '#documentForm', function (e) {
            e.preventDefault();
            const fd = new FormData(this);

            $.ajax({
                url: this.action,
                type: 'POST',
                data: fd,
                processData: false,
                contentType: false,
                success: function (html) {
                    handleCreateUpdateDocument(html);
                },
                error: function () {
                    alert('Failed to submit document.');
                }
            })
        });  

    $(applyRules);
})();
