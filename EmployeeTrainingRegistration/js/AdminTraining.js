$(document).ready(function () {
    toastr.options = {
        closeButton: true,
        progressBar: true,
        positionClass: 'toast-top-right',
        timeOut: 2000
    };
    

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
                        // Populate the modal with the selected training details
                        $('#trainingDetailsModal').modal('show');
                        $('#editTitle').val(data.trainings.Title);
                        var startDate = new Date(parseInt(data.trainings.StartDate.substr(6)));
                        var deadline = new Date(parseInt(data.trainings.Deadline.substr(6)));
                        $('#editDate').val(formatDate(startDate));
                        $('#editThreshold').val(data.trainings.Threshold);
                        $('#editDescription').val(data.trainings.Description);
                        $('#editDeadline').val(formatDate(deadline));

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

                        populateDepartmentDropdown();

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
                var addDepartment = $('#addDepartment').val();
                var checkedPrerequisites = [];
                $('input[type="checkbox"]:checked').each(function () {
                    checkedPrerequisites.push($(this).val());
                });

                //alert('Checked Prerequisites: ' + checkedPrerequisites.join(', '));

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





            // Handle delete button click
            $('.deleteButton').on('click', function () {
                var trainingId = $(this).data('training-id');
                $.ajax({
                    url: '/Training/DeleteTraining', 
                    type: 'POST',
                    data: { id: trainingId },
                    success: function (result) {
                        console.log('Training deleted successfully');
                        //alert('Training deleted successfully');
                        toastr.success('Training deleted successfully!');

                        // Reload the page to see the changes
                        setTimeout(function () {
                            location.reload();
                        }, 2000);

                    },
                    error: function (error) {
                        console.error('Error deleting training: ' + error);
                        alert('Error deleting training: ' + error);
                    }
                });
                console.log('Delete button clicked for Training ID: ' + trainingId);
                alert('Delete button clicked for Training ID: ' + trainingId);
            });



            $('.selectedEmployeeButton').on('click', function () {
                var trainingId = $(this).data('training-id');
                var trainingTitle = document.getElementById('trainingDetailsModalLabel');
                console.log('Selected Employee Button clicked for Training ID: ' + trainingId);

                // Fetch data for the selected training ID and populate the modal body
                $.ajax({
                    type: 'GET',
                    url: '/AutomaticProcessing/GetSelectedEmployeeByTrainingId?id=' + trainingId,
                    dataType: 'json',
                    success: function (data) {
                        // Check if the data is available
                        if (data && data.selectedEmployees && data.selectedEmployees.length > 0) {
                            // Set the training title
                            trainingTitle.innerHTML = `<p>${data.selectedEmployees[0].TrainingTitle}</p>`;
                            $('#selectedEmployeesTable thead').show(); 
                            // Loop through the data and append rows to the table body
                            $.each(data.selectedEmployees, function (index, employee) {
                                var row = '<tr>' +
                                    '<td>' + employee.FirstName + '</td>' +
                                    '<td>' + employee.LastName + '</td>' +
                                    '<td>' + employee.Email + '</td>' +
                                    '</tr>';
                                $('#selectedEmployeesTable tbody').append(row);
                            });

                            // Hide the table headers
                        

                            // Show the modal
                            $('#selectedEmployeesModal').modal('show');
                        } else {
                            // Handle case when there are no selected employees
                            trainingTitle.innerHTML = '<p>No selected employees for this training.</p>';
                            $('#selectedEmployeesTable tbody').empty(); // Clear any existing rows
                            $('#selectedEmployeesTable thead').hide(); // Hide the table headers
                            $('#selectedEmployeesModal').modal('show');
                        }
                    },
                    error: function (error) {
                        // Handle AJAX error
                        console.error('Error fetching data from the server: ' + error.statusText);
                    }
                });
            });



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

function populateDepartmentDropdown() {
    $.ajax({
        type: 'GET',
        url: '/Department/GetAllDepartmentName', // Replace with your actual controller and action names
        dataType: 'json',
        success: function (data) {
            if (data && data.departments) {
                var departmentDropdown = document.getElementById("departmentDropdown");
                departmentDropdown.innerHTML = ""; // Clear existing options

                // Iterate through the departments and append options to the dropdown
                data.departments.forEach(function (department) {
                    // Replace 'departmentId' and 'departmentName' with your actual property names
                    var option = document.createElement("option");
                    option.value = department.DepartmentId;
                    option.text = department.DepartmentName;
                    departmentDropdown.add(option);
                });
                console.log("Data from server:", data);
            } else {
                console.error('Error retrieving department data');
            }
        },
        error: function (error) {
            console.error('Error: ' + error.responseText);
        }
    });
}
