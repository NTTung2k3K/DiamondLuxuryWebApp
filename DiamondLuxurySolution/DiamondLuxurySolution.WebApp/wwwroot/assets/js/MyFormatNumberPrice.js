
$(document).ready(function () {

    // Select all inputs with the class "PriceFormat"
    const $inputs = $(".PriceFormatFinal");

    $inputs.each(function () {

        // Ensure initial value is correctly formatted
        if ($(this).val()) {
            let value = $(this).val().replace(/\./g, "");
            $(this).val(new Intl.NumberFormat('vi-VN').format(parseInt(value)));
        }

        $(this).on("input", function () {
            let value = $(this).val();

            // Remove all non-digit characters
            value = value.replace(/[^\d]/g, "");

            // Limit to a maximum of 16 digits
            if (value.length > 16) {
                value = value.slice(0, 16);
            }

            // If value is not empty, format it with Vietnamese thousand separators
            if (value) {
                $(this).val(new Intl.NumberFormat('vi-VN').format(parseInt(value)));
            } else {
                $(this).val("");
            }
        });
    });

    $("#submitFormFormatFinal").on("submit", function (e) {
        e.preventDefault();

        $inputs.each(function () {
            // Log value before removing dots

            // Remove dots before submitting
            const formattedValue = $(this).val().replace(/\./g, "");
            $(this).val(formattedValue);

            // Log value after removing dots
        });

        this.submit();
    });
});
