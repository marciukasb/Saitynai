fn.dataTable.ext.search.push(
    


    function (settings, data, dataIndex) {
        var min = $('#min').val;
        var max = parseInt($('#max').val(), 10);
        debugger;
        var age = data[3] || 0; // use data for the age column

        if ((min == null) || (min == age)) {
            return true;
        }
        return false;
    }
);

$(document).ready(function () {
    var table = $('#example').DataTable();

    // Event listener to the two range filtering inputs to redraw on input
    $('#min, #max').keyup(function () {
        table.draw();
    });
});