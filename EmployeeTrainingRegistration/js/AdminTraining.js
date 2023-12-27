$(document).ready(function () {
    // Fetch training details from the server
    $.ajax({
        url: '/Training/GetTraining', // Replace with the actual URL
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
                    '</td>' +
                    '</tr>';
                trainingTableBody.append(row);
            });
            $('.editButton').on('click', function () {
                var trainingId = $(this).data('training-id');

                // Fetch training details by ID
                $.ajax({
                    url: '/Training/GetTrainingById',
                    type: 'GET',
                    data: { trainingId: trainingId },
                    dataType: 'json',
                    success: function (data) {
                        // Populate the modal with the selected training details
                        $('#trainingDetailsModal').modal('show');
                        $('#editTitle').val(data.trainings.Title); // Populate the text field with the title

                        // Convert Unix timestamp to Date object
                        var startDate = new Date(parseInt(data.trainings.StartDate.substr(6)));
                        var deadline = new Date(parseInt(data.trainings.Deadline.substr(6)));
                       
                        // Format the date and set the value directly
                        $('#editDate').val(formatDate(startDate));
                        $('#editThreshold').val(data.trainings.Threshold);
                        $('#editDescription').val(data.trainings.PreRequisite);
                        $('#editDeadline').val(formatDate(deadline));
                        populateDepartmentDropdown();

                        // Set the trainingId as a data attribute in the form
                        $('#editTrainingForm').data('trainingId', trainingId);
                    },
                    error: function (error) {
                        console.error(error);
                    }
                });
            });

 


            $('#saveEdit').on('click', function () {
                var trainingId = $('#editTrainingForm').data('trainingId');
                var editedTitle = $('#editTitle').val();
                var editedDate = $('#editDate').val();
           

                // Prepare data to send to the server
                var data = {
                    TrainingId: trainingId,
                    Title: editedTitle,
                    StartDate: editedDate
                };

                // Make an AJAX request to the server
                $.ajax({
                    type: 'POST',
                    url: '/Training/UpdateTraining', // Replace 'ControllerName' with your actual controller name
                    data: data,
                    success: function (result) {
                        // Handle the success response from the server
                        alert('Changes saved successfully!');

                        // Reload the page to see the changes
                        location.reload();
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
                        alert('Training deleted successfully');
                        location.reload();

                    },
                    error: function (error) {
                        console.error('Error deleting training: ' + error);
                        alert('Error deleting training: ' + error);
                    }
                });
                console.log('Delete button clicked for Training ID: ' + trainingId);
                alert('Delete button clicked for Training ID: ' + trainingId);
            });

            $('.addTrainingButton').on('click', function (event) {
                console.log('Add Training button clicked');
                //alert('Add Training button clicked');
                $('#AddTrainingModal').modal('show');
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
