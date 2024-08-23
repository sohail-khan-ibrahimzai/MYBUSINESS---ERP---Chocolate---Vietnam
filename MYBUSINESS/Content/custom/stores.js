$('.storeids').click(function () {
    //alert('Hi');
    $('#storeOpeningPopup').modal('show');
});

$('#openShop').click(function () {
    if (confirm("You input 0, are you sure?")) {
        debugger;
        // Select the element
        const element = document.querySelector('.storeids');
        // Get the value of data-storeid
        const storeIds = element.getAttribute('data-storeid');

        //storeId from layout/shared.js
        storeId = storeIds;
        if (storeIds != null || storeIds != undefined) { localStorage.setItem('storeId', storeId); }

        var selectedBlance;
        var vndBalance = parseFloat($('#totalVndCount').val());
        var dollarBalance = parseFloat($('#totalDollarsCount').val());
        var jpyBalance = parseFloat($('#totalYensCount').val());

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
            OpeningBalance: selectedBlance, // Assuming you have an input field with id="openingBalance"
        };

        // Make the AJAX POST request to server-side
        $.ajax({
            url: '/Stores/OpenShop',  // Replace 'ControllerName' with your actual controller name
            type: 'POST',
            data: JSON.stringify(storeViewModel),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            //headers: {
            //    'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val() // For anti-forgery token
            //},
            success: function (response) {
                if (response.Success) {
                    //alert(response.Message);
                    window.location.href('/SOSR/Create?IsReturn=false')
                    $('#storeOpeningPopup').modal('hide');
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