$(document).ready(function () {
    toastr.options = {
        closeButton: true,
        progressBar: true,
        positionClass: 'toast-top-right',
        timeOut: 3000
    };
    

    var selectedEmployeesTable = $('#selectedEmployeesTable').DataTable({
        dom: 'Bfrtip',
        buttons: [
            {
                extend: 'excel',
                text: 'Export Excel',
                className: 'btn btn-secondary',
                filename: function () {
                    return 'Export_' + ($('#trainingDetailsModalLabel p').text() || 'NoTrainingTitle');
                }
            }
        ],
        searching: false,
        lengthChange: false,
        info: false,
        paging: false,
        order: []
    });
    $('#automaticProcessingBtn').on('click', function () {
        $.ajax({
            type: 'POST',
            url: '/Training/StartAutomaticProcessing', // Replace with your controller route
            dataType: 'json',
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                } else {
                    console.error(response.message);
                }
            },
            error: function (error) {
                console.error(error.responseText);
            }
        });
    });

    // Fetch training details from the server
    $.ajax({
        url: '/Training/GetAllTrainingForAdmin', // Replace with the actual URL
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            // Populate the table with training details
            var trainingTableBody = $('#trainingTable tbody');
            $.each(data.trainings, function (index, training) {
                var row = '<tr>' +
                    '<td>' + training.Title + '</td>' +
                    '<td>' +
                    '<button class="editButton" data-training-id="' + training.TrainingID + '">Edit</button> ' +
                    '<button class="deleteButton" data-training-id="' + training.TrainingID + '">Delete</button>' +
                    '<button class="selectedEmployeeButton" data-training-id="' + training.TrainingID + '">View Selected Employees</button>' +
                    '</td>' +
                    '</tr>';
                trainingTableBody.append(row);
            });
        
            $('.editButton').on('click', function () {
                var trainingId = $(this).data('training-id');

                // Fetch training details and prerequisites by ID
                $.ajax({
                    url: '/Training/GetTrainingById',
                    type: 'GET',
                    data: { trainingId: trainingId },
                    dataType: 'json',
                    success: function (data) {
                        //console.log(data);
                        // Populate the modal with the selected training details
                        $('#trainingDetailsModal').modal('show');
                        $('#editTitle').val(data.trainings.Title);
                        var startDate = new Date(parseInt(data.trainings.StartDate.substr(6)));
                        var deadline = new Date(parseInt(data.trainings.Deadline.substr(6)));
                        $('#editDate').val(formatDate(startDate));
                        $('#editThreshold').val(data.trainings.Threshold);
                        $('#editDescription').val(data.trainings.Description);
                        $('#editDeadline').val(formatDate(deadline));
                        $('#departmentDropdown').val(data.trainings.DepartmentName);
        


                        // Display prerequisites using checkboxes
                        var prerequisitesHtml = '';
                        if (data.allPreRequisites && data.allPreRequisites.length > 0) {
                            data.allPreRequisites.forEach(function (prerequisite) {
                                var isChecked = data.preRequisites.includes(prerequisite);
                                prerequisitesHtml += '<div class="form-check">';
                                prerequisitesHtml += '<input class="form-check-input" type="checkbox" value="' + prerequisite + '" id="checkbox_' + prerequisite + '"' + (isChecked ? 'checked' : '') + '>';
                                prerequisitesHtml += '<label class="form-check-label" for="checkbox_' + prerequisite + '">' + prerequisite + '</label>';
                                prerequisitesHtml += '</div>';
                            });
                        } else {
                            prerequisitesHtml = '<p>No prerequisites</p>';
                        }

                        $('#prerequisitesCheckboxes').html(prerequisitesHtml);

                        //populateDepartmentDropdown();

                        // Set the trainingId as a data attribute in the form
                        $('#editTrainingForm').data('trainingId', trainingId);
                    },
                    error: function (error) {
                        console.error(error);
                        //alert('Edit button error');
                    }
                });
            });

            $('#saveEdit').on('click', function () {
                var trainingId = $('#editTrainingForm').data('trainingId');
                var editedTitle = $('#editTitle').val();
                var editedStartDate = $('#editDate').val();
                var editedDeadline = $('#editDeadline').val();
                var editedThreshold = $('#editThreshold').val();
                var editedPreRequisite = $('#editDescription').val();
                var editedDepartment = $('#departmentDropdown').val();
                var checkedPrerequisites = [];
                $('input[type="checkbox"]:checked').each(function () {
                    checkedPrerequisites.push($(this).val());
                });

                //alert('Checked Prerequisites: ' + checkedPrerequisites.join(', '));

                // Prepare data to send to the server
                var data = {
                    TrainingId: trainingId,
                    Title: editedTitle,
                    StartDate: editedStartDate,
                    Threshold: editedThreshold ,
                    Description: editedPreRequisite,
                    DepartmentName: editedDepartment,
                    Deadline: editedDeadline,
                    PreRequisite: checkedPrerequisites

                };
                console.log(data);
                // Make an AJAX request to the server
                $.ajax({
                    type: 'POST',
                    url: '/Training/UpdateTraining',
                    data: data,
                    success: function (result) {
                        // Handle the success response from the server
                        toastr.success('Training changes saved successfully!');

                        // Reload the page to see the changes
                        setTimeout(function () {
                            location.reload();
                        }, 1000);
                    },
                    error: function () {
                        // Handle the error case
                        toastr.error('Error occurred while saving changes.');
                    }
                });
                // Close the modal after initiating the AJAX request
                $('#trainingDetailsModal').modal('hide');
            });

            $('#submit').on('click', function () {
                var trainingId = $('#addTrainingForm').data('trainingId');
                var addTitle = $('#addTitle').val();
                var addStartDate = $('#addStartDate').val();
                var addDeadline = $('#addDeadline').val();
                var addThreshold = $('#addThreshold').val();
                var addDescription = $('#addDescription').val();
                var addDepartment = $('#departmentDropdown2').val();
                var checkedPrerequisites = [];
                $('input[type="checkbox"]:checked').each(function () {
                    checkedPrerequisites.push($(this).val());
                });

                alert('Checked Prerequisites: ' + checkedPrerequisites.join(', '));

                // Prepare data to send to the server
                var data = {
                    TrainingId: trainingId,
                    Title: addTitle,
                    StartDate: addStartDate,
                    Threshold: addThreshold,
                    Description: addDescription,
                    DepartmentName: addDepartment,
                    Deadline: addDeadline,
                    PreRequisite: checkedPrerequisites

                };
                console.log(data);
                // Make an AJAX request to the server
                $.ajax({
                    type: 'POST',
                    url: '/Training/AddTraining', // Replace 'ControllerName' with your actual controller name
                    data: data,
                    success: function (result) {
                        // Handle the success response from the server
                        //alert('Changes saved successfully!');

                        toastr.success('New Training added successfully!');

                        // Reload the page to see the changes
                        setTimeout(function () {
                            location.reload();
                        }, 1000);
                    },
                    error: function () {
                        // Handle the error case
                        alert('Error occurred while saving changes.');
                    }
                });

                // Close the modal after initiating the AJAX request
                $('#trainingDetailsModal').modal('hide');
            });


            $('.deleteButton').on('click', function () {
                var trainingId = $(this).data('training-id');
                $.ajax({
                    url: '/Training/DeleteTraining', 
                    type: 'POST',
                    data: { id: trainingId },
                    success: function (result) {
                        if (result.success) {
                            console.log('Training cannot be deleted');
                            toastr.error('Training cannot be deleted!');
                        }
                        else {
                            console.log('Training deleted successfully');
                            toastr.success('Training deleted successfully!');

                            setTimeout(function () {
                                location.reload();
                            }, 3000);
                             }
                    },
                    error: function (error) {
                        console.error('Error deleting training: ' + error);
                        alert('Error deleting training: ' + error);
                    }
                });
            });

            $('.selectedEmployeeButton').on('click', function () {
                var trainingId = $(this).data('training-id');
                var trainingTitle = $('#trainingDetailsModalLabel');

                $.ajax({
                    type: 'GET',
                    url: '/AutomaticProcessing/GetSelectedEmployeeByTrainingId?id=' + trainingId,
                    dataType: 'json',
                    success: function (data) {
                        selectedEmployeesTable.clear();

                        if (data && data.selectedEmployees && data.selectedEmployees.length > 0) {
                            trainingTitle.html('<p>' + data.selectedEmployees[0].TrainingTitle + '</p>');
                            $('#selectedEmployeesTable thead').show();

                            $.each(data.selectedEmployees, function (index, employee) {
                                var employeeName = employee.FirstName + ' ' + employee.LastName;
                                var managerName = employee.ManagerFirstName + ' ' + employee.ManagerLastName;
                                selectedEmployeesTable.row.add([
                                    employeeName,
                                    employee.Email,
                                    managerName
                                
                                ]).draw();
                            });

                            $('#exportBtn').show();
                        } else {
                            trainingTitle.html('<p>No selected employees for this training.</p>');
                            selectedEmployeesTable.clear().draw(); // Clear DataTable rows
                            $('#selectedEmployeesTable thead').hide();
                            $('#exportBtn').hide();
                        }

                        $('#selectedEmployeesModal').modal('show');
                    },
                    error: function (error) {
                        console.error('Error fetching data from the server: ' + error.statusText);
                    }
                });
            });


              // Export button click event
    $('#exportBtn').on('click', function () {
        // Trigger the DataTables buttons export function
        selectedEmployeesTable.buttons(0).trigger();
    });

            // Hide the Excel button
            $('.buttons-excel').hide();

            $('#trainingTable').DataTable({
                "pageLength": 6,
                "lengthChange": false,
                "searching": true

            });
            $('.addTrainingButton').on('click', function (event) {
                console.log('Add Training button clicked');

                // Show the modal
                $('#AddTrainingModal').modal('show');

                // Fetch prerequisites from the server
                $.ajax({
                    url: '/Training/GetAllPreRequisites', // Update the URL if needed
                    type: 'GET',
                    success: function (data) {
                        // Clear existing checkboxes
                        $('#trainingPrerequisites').empty();

                        // Add checkboxes for each prerequisite
                        data.allPreRequisite.forEach(function (preReq) {
                            $('#trainingPrerequisites').append(
                                '<div class="form-check">' +
                                '   <input type="checkbox" class="form-check-input" name="prerequisites" value="' + preReq + '">' +
                                '   <label class="form-check-label">' + preReq + '</label>' +
                                '</div>'
                            );
                        });
                    },
                    error: function (error) {
                        console.error('Error fetching prerequisites: ', error);
                    }
                });

                event.preventDefault();
            });




        },
        error: function (error) {
            console.error('Error fetching training details: ', error);
        }
    });
});


// Function to format date as 'YYYY-MM-DD'
function formatDate(date) {
    var year = date.getFullYear();
    var month = (date.getMonth() + 1).toString().padStart(2, '0');
    var day = date.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
}





