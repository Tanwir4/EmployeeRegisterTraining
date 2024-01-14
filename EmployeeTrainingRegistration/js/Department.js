$(document).ready(function () {
    function GetDepartmentDropdown() {
        $.ajax({
            type: 'GET',
            url: '/Department/GetAllDepartmentName',
            dataType: 'json',
            success: function (data) {

                console.log(data);
                if (data.departments) {
                    populateDepartmentDropdown(data.departments, '#departmentDropdown');
                    populateDepartmentDropdown(data.departments, '#departmentDropdown2');
                    populateDepartmentDropdown(data.departments, '#department');
                } else {
                    console.error('Error: Unexpected response format');
                }
            },
            error: function (error) {
                //console.error('Error: ' + error.responseText);
                console.error('Error: ', error);
            }
        });
    }

    function populateDepartmentDropdown(options, dropdown) {

        // Check if the dropdown element exists
        var $dropdown = $(dropdown); // Convert to jQuery object
        if ($dropdown.length) {
            $dropdown.empty(); // Clear existing options

            options.forEach(function (option) {
                var optionElement = $('<option>', { text: option.DepartmentName });
                $dropdown.append(optionElement);
            });
        } else {
            console.error('Error: Dropdown element not found');
        }
    }


    GetDepartmentDropdown();
});
