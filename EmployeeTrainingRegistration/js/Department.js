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
                console.error('Error: ', error);
            }
        });
    }

    function populateDepartmentDropdown(options, dropdown) {
        var $dropdown = $(dropdown); 
        if ($dropdown.length) {
            $dropdown.empty(); 

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
