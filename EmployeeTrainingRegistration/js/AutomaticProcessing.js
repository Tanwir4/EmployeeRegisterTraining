$(document).ready(function () {
    $.ajax({
        url: '/AutomaticProcessing/GetEnrolledEmployeesForExport',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data && data.enrolledEmployees) {
                $.each(data.enrolledEmployees, function (index, employee) {
                    var row = '<tr>' +
                        '<td>' + employee.TrainingTitle + '</td>' +
                        '<td>' + employee.FirstName + '</td>' +
                        '<td>' + employee.LastName + '</td>' +
                        '</tr>';
                    $('#enrolledEmployeesTable tbody').append(row);
                });
            } else {
                console.error('Invalid or empty data returned from the server.');
            }
        },
        error: function (error) {
            console.error('Error fetching data from the server: ' + error.statusText);
        }
    });
});