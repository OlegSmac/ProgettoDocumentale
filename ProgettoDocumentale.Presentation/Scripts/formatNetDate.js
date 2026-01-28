function formatNetDate(v) {
    if (!v) return "";
    // v like "/Date(1768255200000)/"
    var ms = parseInt(v.substr(6), 10);
    var d = new Date(ms);
    return d.toISOString().slice(0, 10); // "yyyy-MM-dd"
}
