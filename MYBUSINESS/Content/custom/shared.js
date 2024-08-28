//$('.storeids').click(function () {
//    alert('Hi');
//    $('#storeOpeningPopup').modal('show');
//});
var storeId = 0;
if (storeId != null) {

}
$(document).on('click', '#closeStore', function (e) {
    debugger;
    e.preventDefault();
    $('#storeClosingPopup').modal('show');
    $('#storeClosingPopup1').modal('show');
});
//$('#closeStore').click(function () {
//    alert('Hi');
//    $('#storeOpeningPopup').modal('show');
//});
$(document).on('click', '#closeShop', function () {
    //if (confirm("You input 0, are you sure?")) {
    var totalAmountVndClose = document.getElementById('totalVndCountClose').value;
    if (totalAmountVndClose == '0') totalAmountVndClose = 0;
    if (confirm(`You input '${totalAmountVndClose}', are you sure?`)) {
        debugger;
        // Select the element
        //const element = document.querySelector('.storeids');
        //// Get the value of data-storeid
        //const storeId = element.getAttribute('data-storeid');
        //alert(storeId);
        var formattedString = "";
        if (closeCurrencyDetalInVnd.length > 0) {
            debugger;
            formattedString = closeCurrencyDetalInVnd.map(item => {
                // Set the fixed denomination value
                let denominationValue = 0;

                switch (item.denomination) {
                    case 'oneThsndVndClose':
                        denominationValue = 1000;
                        break;
                    case 'twoThsndVndClose':
                        denominationValue = 2000;
                        break;
                    case 'fiveThsndVndClose':
                        denominationValue = 5000;
                        break;
                    case 'tenThsndVndClose':
                        denominationValue = 10000;
                        break;
                    case 'twentyThsndVndClose':
                        denominationValue = 20000;
                        break;
                    case 'fiftyThsndVndClose':
                        denominationValue = 50000;
                        break;
                    case 'oneLacVndClose':
                        denominationValue = 100000;
                        break;
                    case 'twoLacVndClose':
                        denominationValue = 200000;
                        break;
                    case 'fiveLacVndClose':
                        denominationValue = 500000;
                        break;
                    default:
                        denominationValue = 0;
                }

                // Use the fixed denomination value in the formatted string
                return `${denominationValue}@${item.count}`;
            }).join(':');
        }
        var selectedBlance = 0;
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
            vndBalance = 0;
            dollarBalance = 0;
            jpyBalance = 0;
            //alert('No balance available');
        }

        var storeViewModel = {
            ClosingBalance: selectedBlance, // Assuming you have an input field with id="openingBalance"
            ClosingCurrencyDetail: formattedString
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
                    //alert(response.Message);
                    window.location.href = '/Stores/StoreDashboard';
                    localStorage.removeItem('storeId');
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
        $('#storeClosingPopup').modal('hide');
        $('#storeClosingPopup').find('input, textarea, select').val('0');
    }
});
function calculateTotalClose(inputId, denominationValue, outputId) {
    debugger;
    const count = parseInt(document.getElementById(inputId).value) || 0;
    const total = count * denominationValue;
    document.getElementById(outputId).value = total;

    // Update the overall totals
    updateOverallTotalsClose();
}
var closeCurrencyDetalInVnd = [];
function updateOverallTotalsClose() {
    debugger;
    const inputIds = ['oneThsndVndClose', 'twoThsndVndClose', 'fiveThsndVndClose', 'tenThsndVndClose', 'twentyThsndVndClose', 'fiftyThsndVndClose', 'oneLacVndClose', 'twoLacVndClose', 'fiveLacVndClose'];
    const outputIds = ['totalOneThsndVndClose', 'totalTwoThsndVndClose', 'totalFiveThsndVndClose', 'totalTenThsndVndClose', 'totalTwentyThsndVndClose', 'totalFiftyThsndVndClose', 'totalOneLacVndClose', 'totalTwoLacVndClose', 'totalFiveLacVndClose'];

    let totalNotes = 0;
    let totalValue = 0;
    closeCurrencyDetalInVnd = [];
    inputIds.forEach((id, index) => {
        const count = parseInt(document.getElementById(id).value) || 0;
        totalNotes += count;

        // Get the corresponding total value
        const value = parseInt(document.getElementById(outputIds[index]).value) || 0;
        totalValue += value;

        // Push the note count and value to the array
        if (count > 0) {
            closeCurrencyDetalInVnd.push({
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

    document.getElementById('totalVndClose').value = totalNotes;
    document.getElementById('totalVndCountClose').value = totalValue;
}
function logout() {
    debugger
    // Clear all items from localStorage
    localStorage.clear();

    // Redirect to logout action on the server
    window.location.href = '/UserManagement/Logout';
}