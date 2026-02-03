function handleCreateUpdateDocument(html) {
    $('#appModalContent').empty().html(html);    

    var success = $('#appModalContent').find('[data-success]').first().attr('data-success');

    if (success === '1') {
        $('#appModal').modal('hide');        

        documentsTable?.ajax.reload(null, false);
        loadDocumentsHierarchy();       
    }    
}
