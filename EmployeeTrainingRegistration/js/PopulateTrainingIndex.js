$(document).ready(function () {
    toastr.options = {
        closeButton: true,
        progressBar: true,
        positionClass: 'toast-top-right',
        timeOut: 3000
    };
    $.ajax({
        url: '/Training/GetTraining',
        type: 'GET',
        datatype: 'json',
        success: function (data) {
            try {
                displayTraining(data.trainings);
                $('#trainingTable').DataTable({
                    "pageLength": 5,
                    "lengthChange": false,
                    "searching": true


                });
            }
            catch (error) { window.location.href = '/Common/InternalServerError' }
            //displayTraining(data.trainings);
        },
        error: function (error) {
            console.error(error);
            window.location.href = '/Common/InternalServerError'
        },
    });

});

function displayTraining(data) {
    var trainingTable = $('#trainingTable');
    var tableBody = trainingTable.find('tbody');
    console.log('Display trainings function');
    data.forEach(function (training) {
        var isTrainingApplied = checkIfTrainingApplied(training["TrainingID"]);

        var applyButton = isTrainingApplied
            ? '<button disabled style="background-color: #4CAF50;">Already Applied</button>'
            : '<button onclick="openApplication(' + training["TrainingID"] + ')">Apply</button>';

        var rowHtml = `
            <tr>
                <td>${training["Title"]}</td>
                <td>${training["Description"]}</td>
                <td>
                    <button class="read" onclick="openTrainingDetails(${training["TrainingID"]})" type="button">View</button> 
                    ${applyButton}
                </td>
            </tr>`;
        tableBody.append(rowHtml);
    });
}

function checkIfTrainingApplied(trainingId) {
    // Make an Ajax call to your controller action to check if training is applied
    var isTrainingApplied = false;

    // Assuming you have an endpoint to check if training is applied
    $.ajax({
        url: '/Training/IsTrainingApplied',
        type: 'GET',
        data: { trainingId: trainingId },
        async: false, // Synchronous call for simplicity, consider using asynchronous call
        success: function (result) {
            isTrainingApplied = result;
        }
    });

    return isTrainingApplied;
}




function openTrainingDetails(trainingId) {
    console.log('Training ID:', trainingId);
    $.ajax({
        url: '/Training/GetTrainingById',
        type: 'GET',
        data: { trainingId: trainingId },
        datatype: 'json',
        success: function (data) {
            var startDate = new Date(parseInt(data.trainings.StartDate.substr(6)));
            var formattedStartDate = startDate.toLocaleDateString('en-US', { year: 'numeric', month: 'long', day: 'numeric' });
            var deadline = new Date(parseInt(data.trainings.Deadline.substr(6)));
            var formattedDeadline = deadline.toLocaleDateString('en-US', { year: 'numeric', month: 'long', day: 'numeric' });

            // Display training details
            $('#trainingTitle').html(data.trainings.Title);
            $('#trainingPreRequisite').html('<span>Description: </span>' + data.trainings.Description);
            $('#trainingDeadline').html('<span>Deadline: </span>' + formattedDeadline);
            $('#trainingSeat').html('<span>Seat Threshold: </span>' + data.trainings.Threshold);
            $('#trainingStartDate').html('<span>Start Date: </span>' + formattedStartDate);

            // Display prerequisites
            var prerequisitesHtml = '<span>Prerequisites: </span>';
            if (data.preRequisites.length > 0) {
                prerequisitesHtml += '<ul>';
                for (var i = 0; i < data.preRequisites.length; i++) {
                    prerequisitesHtml += '<li>' + data.preRequisites[i] + '</li>';
                }
                prerequisitesHtml += '</ul>';
            } else {
                prerequisitesHtml += 'None';
            }
            $('#trainingPrerequisites').html(prerequisitesHtml);

            // Show the modal
            $('#trainingDetailsModal').modal('show');
        },
        error: function (error) {
            console.error(error);
        },
    });
}

function openApplication(trainingId) {
    $('#ApplicationModal').modal('show');
    $.ajax({
        url: '/Training/GetTrainingById',
        type: 'GET',
        data: { trainingId: trainingId },
        datatype: 'json',
        success: function (data) {
            $('#test').html('You are applying for: ' + data.trainings.Title);

            // Clear previous inputs
            $('#prerequisitesInputs').empty();

            // Create file input for each prerequisite
            for (var i = 0; i < data.preRequisites.length; i++) {
                var inputHtml = '<div class="col-md-12">' +
                    '<p>' + data.preRequisites[i] + '</p>' +

                    '<label for="fileInput_' + i + '" class="btn btn-primary">' +
                    'Browse' +
                    '<input type="file" id="fileInput_' + i + '" name="fileInput_' + i + '" style="display:none;" onchange="displayFileName(this, ' + i + ')" />' +
                    '</label>' +
                    '<span class="prerequisite-name mt-2" id="fileName_' + i + '">No file selected</span>' +

                    '</div>';
                $('#prerequisitesInputs').append(inputHtml);
            }

            // Set the trainingId as a data attribute in the modal
            $('#ApplicationModal').data('trainingId', trainingId);
        },
        error: function (error) {
            console.error(error);
        },
    });
}

/*function openApplication(trainingId) {
    $('#ApplicationModal').modal('show');
    $.ajax({
        url: '/Training/GetTrainingById',
        type: 'GET',
        data: { trainingId: trainingId },
        datatype: 'json',
        success: function (data) {
            $('#test').html('You are applying for: ' + data.trainings.Title);

            // Clear previous inputs
            $('#prerequisitesFileUploads').empty();

            // Create file input for each prerequisite
            // Iterate over prerequisites and create file input for each
            if (Array.isArray(data.preRequisites)) {
                data.preRequisites.forEach(function (prerequisite, index) {
                    var fileInputHtml = `
                        <label for="fileInput${index}">${prerequisite} File:</label>
                        <input type="file" id="fileInput${index}" class="form-control mb-3" name="file${index}">
                            `;
                    $('#prerequisitesFileUploads').append(fileInputHtml);
                });
            }

            // Set the trainingId as a data attribute in the modal
            $('#ApplicationModal').data('trainingId', trainingId);
        },
        error: function (error) {
            console.error(error);
        },
    });
}
*/
function displayFileName(input, index) {
    var fileName = input.files[0] ? input.files[0].name : 'No file selected';
    $('#fileName_' + index).html(fileName);
}



function submitApplication() {
    var trainingId = $('#ApplicationModal').data('trainingId');
    console.log('Submit Application js function');
    alert(applicationId);

    if (!trainingId) {
        console.error('TrainingId is missing');
        return;
    }

    var formData = new FormData($('#applicationForm')[0]);

    // Find all file inputs with the name 'fileInput_' and append them to formData
    $('input[name^="fileInput_"]').each(function () {
        var index = $(this).attr('id').split('_')[1];
        formData.append('fileInputs', $(this)[0].files[0]);
    });

    formData.append('trainingId', trainingId);

    $.ajax({
        url: '/Application/SubmitApplication',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (data) {
            toastr.success('Training applied successfully!');

            // Reload the page to see the changes
            setTimeout(function () {
                location.reload();
            }, 3000);
        },
        error: function (error) {
            console.error(error);
        },
    });
}

