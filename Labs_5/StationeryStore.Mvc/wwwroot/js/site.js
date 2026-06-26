document.addEventListener('DOMContentLoaded', function () {
    const displayInput = document.getElementById('displayMinPrice');
    const hiddenInput = document.getElementById('actualMinPrice');

    if(!displayInput || !hiddenInput) return;
    if (hiddenInput.value) { 
        let initialValue = parseInt(hiddenInput.value, 10);
        if (!isNaN(initialValue)) {
            displayInput.value = initialValue.toLocaleString('en-US'); 
        }
    }

    displayInput.addEventListener('input', function (e) {
        // Strip out everything except pure numbers
        let rawString = e.target.value.replace(/\D/g, '');

        // If they deleted everything, clear both boxes
        if (rawString === "") {
            displayInput.value = "";
            hiddenInput.value = "";
            return;
        }

        let cleanNumber = parseInt(rawString, 10);

        // Show the formatted version with commas to the user
        displayInput.value = cleanNumber.toLocaleString('en-US');

        // Save the pure, unformatted number for the C# backend
        hiddenInput.value = cleanNumber;
    });
});