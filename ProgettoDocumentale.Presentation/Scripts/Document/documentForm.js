window.DocumentForm = (function () {

    function $macro() { return $('#appModalContent  #MacroTypeId'); }
    function $micro() { return $('#appModalContent  #MicroTypeId'); }
    function $proj() { return $('#appModalContent  #ProjectId'); }

    function setHiddenTypeId() {
        const code = $macro().find(':selected').data('code');
        const val = !code ? '' : (code === 'SLA_REPORT' ? $macro().val() : $micro().val());
        $('#appModalContent #TypeId').val(val || '');
    }

    function toggleRow(rowId, on) { $(rowId).toggle(!!on); }

    function disableSelect($s, text) {
        $s.prop('disabled', true).html(`<option value="">${text}</option>`).val('');
    }

    function hasRealMicroOptions() {   
        return $micro().find('option').length > 1;
    }

    function loadMicro(macroId) {
        const $m = $micro();
        if (!macroId) {
            disableSelect($m, 'Select micro type');
            setHiddenTypeId();
            return;
        }

        disableSelect($m, 'Loading...');
        $.getJSON('/CedacriOperator/GetMicroTypes', { macroTypeId: macroId })
            .done(items => {
                $m.prop('disabled', false).html('<option value="">Select micro type</option>');
                (items || []).forEach(it => $m.append(new Option(it.Name, it.Id)));
                setHiddenTypeId();
            })
            .fail(() => {
                disableSelect($m, 'Failed to load micro types');
                setHiddenTypeId();
            });
    }

    function applyRules() {
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

        $('#microRow').show();
        $('#MicroTypeId').prop('disabled', false);
      
        if (!hasRealMicroOptions()) loadMicro(macroId);
        else setHiddenTypeId();

        const needsProject = (code === 'PROGETTAZIONE');
        toggleRow('#projectRow', needsProject);
        $proj().prop('disabled', !needsProject);
        if (!needsProject) $proj().val('');
    }

    function bindOnce() {   
        $(document)
            .off('change.docform', '#MacroTypeId')
            .on('change.docform', '#MacroTypeId', function () {
                $('#MicroTypeId').html('<option value="">Select micro type</option>').val('');
                $micro().val('');
                applyRules();
            })
            .off('change.docform', '#MicroTypeId')
            .on('change.docform', '#MicroTypeId', function () {
                setHiddenTypeId();
            })
            .off('submit.docform', '#documentForm')
            .on('submit.docform', '#documentForm', function (e) {
                e.preventDefault();

                const fd = new FormData(this);

                $.ajax({
                    url: this.action,
                    type: 'POST',
                    data: fd,
                    processData: false,
                    contentType: false
                })
                .done(html => handleCreateUpdateDocument(html))
                .fail(() => alert('Failed to submit document.'));
            });
    }
  
    return {
        bindOnce,
        init: applyRules
    };
})();
