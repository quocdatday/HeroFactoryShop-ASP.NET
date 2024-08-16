$(document).ready(function () {
    var table = new DataTable('#example');
    //DELETE
    $("#btnDlt").on("click", function () {
        var x = $("#inputData").val();
        var token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            url: `/admin/${x}?handler=Dlt${x}`,
            type: 'DELETE',
            contentType: 'application/json',
            headers: {
                RequestVerificationToken: token
            },
            data: JSON.stringify({
                ID: $("#inputDlt").val()
            }),
            dataType: 'json',
            success: function (res) {
                table.row($(`tr[data-id="${res.id}"]`)).remove().draw(false);
                $("#Delete").removeClass("fixed-top top-50 translate-middle-y container").addClass("fixed-top top-100 container");
                location.reload();
            },
            error: function (xhr, status, error) {
                console.log('Error:', error); console.log('Status:', status); console.log('Response:', xhr);
            }
        });
    });
});

