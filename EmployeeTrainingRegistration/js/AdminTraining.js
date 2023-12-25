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

            // Handle edit button click
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

                        var startDate = new Date(parseInt(data.trainings.StartDate.substr(6)));
                        startDate.setDate(startDate.getDate() + 1); // Increase the date by one day
                        var formattedStartDate = startDate.toISOString().split('T')[0];
                        console.log(formattedStartDate);
                       // $('#editDate').val(formattedStartDate);

                        console.log('Retrieved date:', data.trainings.StartDate);
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
                    Date: editedDate
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
                // Add your delete logic here, e.g., show a confirmation dialog and then delete the training
                console.log('Delete button clicked for Training ID: ' + trainingId);
            });
        },
        error: function (error) {
            console.error('Error fetching training details: ', error);
        }
    });
});
