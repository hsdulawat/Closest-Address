$(document).ready(function () {
    $("#tblAddressList").hide();
    $('#btnSearch').click(function () {
        if ($('#txtAddress').val() != null && $('#txtAddress').val() !== "") {
            $.ajax({
                type: "GET",
                url: "/api/DistanceCalculator",
                contentType: "application/json; charset=utf-8",
                datatype: "json",
                data: { originAddress: $('#txtAddress').val() },
                async: "true",
                success: function (data) {
                    if (data.length > 0) {
                        $("#tblAddressList").show();
                        $.each(data,
                            function(key, item) {
                                $('#tblAddressResult')
                                    .append('<tr><td>' + item.Name + '</td><td>' + item.KM + '</td></tr>');
                            });
                    } else {
                        $('#errorMsg').show();
                    }
                },
                error: function (response) {
                    console.log("response is not coming");
                }
            });
        } else {
            alert("Please enter address");
        }
    });
});