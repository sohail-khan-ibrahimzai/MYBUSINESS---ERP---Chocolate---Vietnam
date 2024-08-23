//$('.storeids').click(function () {
//    alert('Hi');
//    $('#storeOpeningPopup').modal('show');
//});
var storeId = 0;
$(document).on('click', '#closeStore', function () {
    debugger;
    $('#storeClosingPopup').modal('show');
});
//$('#closeStore').click(function () {
//    alert('Hi');
//    $('#storeOpeningPopup').modal('show');
//});
$(document).on('click', '#closeShop', function () {
    if (confirm("You input 0, are you sure?")) {
        debugger;
        // Select the element
        //const element = document.querySelector('.storeids');
        //// Get the value of data-storeid
        //const storeId = element.getAttribute('data-storeid');
        //alert(storeId);

        var selectedBlance;
        var vndBalance = parseFloat($('#totalVndCountClose').val());
        var dollarBalance = parseFloat($('#totalDollarsCountClose').val());
        var jpyBalance = parseFloat($('#totalYensCountClose').val());

        if (!isNaN(vndBalance) && vndBalance !== 0) {
            selectedBlance = vndBalance;
        } else if (!isNaN(dollarBalance) && dollarBalance !== 0) {
            selectedBlance = dollarBalance;
        } else if (!isNaN(jpyBalance) && jpyBalance !== 0) {
            selectedBlance = jpyBalance;
        } else {
            alert('No balance available');
        }

        var storeViewModel = {
            ClosingBalance: selectedBlance, // Assuming you have an input field with id="openingBalance"
        };

        // Make the AJAX POST request to server-side
        $.ajax({
            url: '/Stores/CloseShop',  // Replace 'ControllerName' with your actual controller name
            type: 'POST',
            data: JSON.stringify(storeViewModel),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            //headers: {
            //    'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val() // For anti-forgery token
            //},
            success: function (response) {
                if (response.Success) {
                    alert(response.Message);
                } else {
                    alert('Error: ' + response.Message);
                }
            },
            error: function (xhr, status, error) {
                alert('An error occurred: ' + error);
            }
        });
    } else {
        alert("Operation cancelled.");
    }
});