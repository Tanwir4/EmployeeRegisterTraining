﻿$(document).ready(function () {
    $('#department').on('change', function () {
        var selectedDepartment = $(this).val();
        $.ajax({
            type: 'GET',
            url: '/Register/ManagersByDepartment',
            data: { department: selectedDepartment },
            success: function (data) {
                console.log(data);
                // Clear existing options
                $('#manager').empty();

                // Populate manager dropdown with new options
                $.each(data.managers, function (index, manager) {
                    $('#manager').append($('<option>').text(manager).attr('value', manager));
                });
            },

            error: function (error) {
                console.error('Error fetching managers:', error);
            }
        });
    });
});