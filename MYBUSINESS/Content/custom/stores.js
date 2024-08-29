$('.storeids').click(function () {
    //alert('Hi');
    $('#storeOpeningPopup').modal('show');
});

////////////////
var formattedString = "";
var storeName = "";
document.querySelectorAll('.storeids').forEach(function (element) {
    element.addEventListener('click', function () {
        debugger;
        // Get the value of data-storeid from the clicked element
        const storeIds = this.getAttribute('data-storeid');
        storeName = this.getAttribute('data-storename');
        // Set this storeId on the openShop button's data attribute for later use
        $('#openShop').data('storeid', storeIds);
    });
});
$('#openShop').click(function () {
    debugger;
    const storeIds = $(this).data('storeid');
    if (!storeIds) {
        alert('No store selected. Please select a store first.');
        return;
    }
    var totalAmountVnd = document.getElementById('totalVndCount').value;
    if (totalAmountVnd == '0') totalAmountVnd = 0;
    if (confirm(`You input '${totalAmountVnd}', are you sure?`)) {
        debugger;
        // Select the element
        //const element = document.querySelector('.storeids');
        //// Get the value of data-storeid
        //const storeIds = element.getAttribute('data-storeid');

        if (openingCurrencyDetalInVnd.length > 0) {
            debugger;
            formattedString = openingCurrencyDetalInVnd.map(item => {
                // Set the fixed denomination value
                let denominationValue = 0;

                switch (item.denomination) {
                    case 'oneThsndVnd':
                        denominationValue = 1000;
                        break;
                    case 'twoThsndVnd':
                        denominationValue = 2000;
                        break;
                    case 'fiveThsndVnd':
                        denominationValue = 5000;
                        break;
                    case 'tenThsndVnd':
                        denominationValue = 10000;
                        break;
                    case 'twentyThsndVnd':
                        denominationValue = 20000;
                        break;
                    case 'fiftyThsndVnd':
                        denominationValue = 50000;
                        break;
                    case 'oneLacVnd':
                        denominationValue = 100000;
                        break;
                    case 'twoLacVnd':
                        denominationValue = 200000;
                        break;
                    case 'fiveLacVnd':
                        denominationValue = 500000;
                        break;
                    default:
                        denominationValue = 0;
                }

                // Use the fixed denomination value in the formatted string
                return `${denominationValue}@${item.count}`;
            }).join(':');
        }
        // Set storeId in localStorage and session
        storeId = storeIds;
        if (storeIds != null || storeIds != undefined) {
            localStorage.setItem('storeId', storeId);
            localStorage.setItem('storeName', storeName);
            setSession(); // Call setSession function to make an AJAX request
        }
    } else {
        alert("Operation cancelled.");
        $('#storeOpeningPopup').modal('hide');
        $('#storeOpeningPopup').find('input, textarea, select').val('0');
    }
});

//$('#openShop').click(function () {
//    if (confirm("You input 0, are you sure?")) {
//        debugger;
//        // Select the element
//        const element = document.querySelector('.storeids');
//        // Get the value of data-storeid
//        const storeIds = element.getAttribute('data-storeid');
//        var formattedString = "";
//        if (openingCurrencyDetalInVnd.length > 0) {
//            debugger;
//            formattedString = openingCurrencyDetalInVnd.map(item => {
//                // Set the fixed denomination value
//                let denominationValue = 0;

//                switch (item.denomination) {
//                    case 'oneThsndVnd':
//                        denominationValue = 1000;
//                        break;
//                    case 'twoThsndVnd':
//                        denominationValue = 2000;
//                        break;
//                    case 'fiveThsndVnd':
//                        denominationValue = 5000;
//                        break;
//                    case 'tenThsndVnd':
//                        denominationValue = 10000;
//                        break;
//                    case 'twentyThsndVnd':
//                        denominationValue = 20000;
//                        break;
//                    case 'fiftyThsndVnd':
//                        denominationValue = 50000;
//                        break;
//                    case 'oneLacVnd':
//                        denominationValue = 100000;
//                        break;
//                    case 'twoLacVnd':
//                        denominationValue = 200000;
//                        break;
//                    case 'fiveLacVnd':
//                        denominationValue = 500000;
//                        break;
//                    default:
//                        denominationValue = 0;
//                }

//                // Use the fixed denomination value in the formatted string
//                return `${denominationValue}@${item.count}`;
//            }).join(':');
//        }
//        //formattedString = openingCurrencyDetalInVnd.map(item => `${item.totalValue}@${item.count}`).join(':');
//        //storeId from layout/shared.js
//        storeId = storeIds;
//        if (storeIds != null || storeIds != undefined) { localStorage.setItem('storeId', storeId); /*document.getElementById('closeStore').style.display = 'block';*/ /*document.getElementById("StorageItem").value = storeId;*/ setSession(); }

//        var selectedBlance;
//        var vndBalance = parseFloat($('#totalVndCount').val());
//        var dollarBalance = parseFloat($('#totalDollarsCount').val());
//        var jpyBalance = parseFloat($('#totalYensCount').val());

//        if (!isNaN(vndBalance) && vndBalance !== 0) {
//            selectedBlance = vndBalance;
//        } else if (!isNaN(dollarBalance) && dollarBalance !== 0) {
//            selectedBlance = dollarBalance;
//        } else if (!isNaN(jpyBalance) && jpyBalance !== 0) {
//            selectedBlance = jpyBalance;
//        } else {
//            alert('No balance available');
//        }

//        var storeViewModel = {
//            OpeningBalance: selectedBlance, // Assuming you have an input field with id="openingBalance"
//            OpeningCurrencyDetail: formattedString
//        };

//        // Make the AJAX POST request to server-side
//        $.ajax({
//            url: '/Stores/OpenShop',  // Replace 'ControllerName' with your actual controller name
//            type: 'POST',
//            data: JSON.stringify(storeViewModel),
//            contentType: 'application/json; charset=utf-8',
//            dataType: 'json',
//            //headers: {
//            //    'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val() // For anti-forgery token
//            //},
//            success: function (response) {
//                if (response.Success) {
//                    //alert(response.Message);
//                    window.location.href = '/SOSR/Index';
//                    $('#storeOpeningPopup').modal('hide');
//                } else {
//                    alert('Error: ' + response.Message);
//                }
//            },
//            error: function (xhr, status, error) {
//                alert('An error occurred: ' + error);
//            }
//        });
//    } else {
//        alert("Operation cancelled.");
//    }
//});
function calculateTotal(inputId, denominationValue, outputId) {
    debugger;
    const count = parseInt(document.getElementById(inputId).value) || 0;
    const total = count * denominationValue;
    document.getElementById(outputId).value = total;

    // Update the overall totals
    updateOverallTotals();
}
var openingCurrencyDetalInVnd = [];
function updateOverallTotals() {
    debugger;
    const inputIds = ['oneThsndVnd', 'twoThsndVnd', 'fiveThsndVnd', 'tenThsndVnd', 'twentyThsndVnd', 'fiftyThsndVnd', 'oneLacVnd', 'twoLacVnd', 'fiveLacVnd'];
    const outputIds = ['totalOneThsndVnd', 'totalTwoThsndVnd', 'totalFiveThsndVnd', 'totalTenThsndVnd', 'totalTwentyThsndVnd', 'totalFiftyThsndVnd', 'totalOneLacVnd', 'totalTwoLacVnd', 'totalFiveLacVnd'];

    let totalNotes = 0;
    let totalValue = 0;
    openingCurrencyDetalInVnd = [];
    inputIds.forEach((id, index) => {
        const count = parseInt(document.getElementById(id).value) || 0;
        totalNotes += count;

        // Get the corresponding total value
        const value = parseInt(document.getElementById(outputIds[index]).value) || 0;
        totalValue += value;

        // Push the note count and value to the array
        if (count > 0) {
            openingCurrencyDetalInVnd.push({
                denomination: id,  // Identifier for the denomination (e.g., 'oneThsndVnd')
                count: count,
                totalValue: value
            });
        }
    });
    //inputIds.forEach((id, index) => {
    //    const count = parseInt(document.getElementById(id).value) || 0;
    //    totalNotes += count;
    //});

    //outputIds.forEach(id => {
    //    const value = parseInt(document.getElementById(id).value) || 0;
    //    totalValue += value;
    //});

    document.getElementById('totalVnd').value = totalNotes;
    document.getElementById('totalVndCount').value = totalValue;
}
//function setSession() {
//    debugger;
//        // Retrieve the value from localStorage
//        var storeIds = localStorage.getItem('storeId');
//        if (storeIds !== null && storeIds !== undefined) {
//            // Set display style and value
//            //document.getElementById('closeStore').style.display = 'block';
//            document.getElementById('storeIdInput').value = storeId;
//            // Submit the form
//            document.getElementById('hiddenForm').submit();
//        }
//}
function setSession() {
    debugger;
    var storeIds = localStorage.getItem('storeId');
    var storesName = localStorage.getItem('storeName');

    if (storeIds !== null && storeIds !== undefined) {
        // Perform an AJAX call to set the session value on the server
        $.ajax({
            url: '/UserManagement/StoreValue',  // Replace with your actual controller/action path
            type: 'POST',
            data: { storeId: storeIds, storeName: storesName },
            success: function (response) {
                if (response.Success) {
                    // Proceed to open the shop after successfully setting the session
                    openShop();
                } else {
                    alert('Failed to set session: ' + response.Message);
                }
            },
            error: function (xhr, status, error) {
                alert('An error occurred while setting the session: ' + error);
            }
        });
    }
}
function openShop() {
    debugger;

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
        vndBalance = 0;
        dollarBalance = 0;
        jpyBalance = 0;
        //alert('No balance available');
        //return;
    }

    var storeViewModel = {
        OpeningBalance: selectedBlance,
        OpeningCurrencyDetail: formattedString
    };

    // Make the AJAX POST request to server-side
    $.ajax({
        url: '/Stores/OpenShop',
        type: 'POST',
        data: JSON.stringify(storeViewModel),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (response) {
            if (response.Success) {
                debugger;
                window.location.href = '/SOSR/Create?IsReturn=false';
                $('#storeOpeningPopup').modal('hide');
            } else {
                alert('Error: ' + response.Message);
            }
        },
        error: function (xhr, status, error) {
            alert('An error occurred: ' + error);
        }
    });
}