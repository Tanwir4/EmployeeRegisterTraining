$(document).ready(function () {
    $.ajax({
        url: '/Training/GetTraining',
        type: 'GET',
        datatype: 'json',
        success: function (data) {
            displayTraining(data.trainings);
        },
        error: function (error) {
            console.error(error);
        },
    });
});
function displayTraining(data) {
    var trainingTable = $('#trainingTable');

    // Clear existing content in the table
    trainingTable.empty();

    // Add table headers
    trainingTable.append(`
        <thead>
            <tr>
                <th>Title</th>
                <th>Description</th>
                <th>Action</th>
            </tr>
        </thead>
        <tbody>
    `);

    // Add rows for each training entry
    data.forEach(function (training) {
        var trainingHtml = `
            <tr>
                <td>${training["Title"]}</td>
                 <td>${training["Description"]}</td>
                <td>
                    <button class="read" onclick="openTrainingDetails(${training["TrainingID"]})" type="button">View</button> 
                    <button onclick="openApplication(${training["TrainingID"]})">Apply</button>
                </td>
            </tr>
        `;
        trainingTable.append(trainingHtml);
    });

    // Close the table body
    trainingTable.append(`</tbody>`);
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

function displayFileName(input, index) {
    var fileName = input.files[0] ? input.files[0].name : 'No file selected';
    $('#fileName_' + index).html(fileName);
}


function displayFileName(input, index) {
    var fileName = input.files[0] ? input.files[0].name : 'No file selected';
    $('#fileName_' + index).html(fileName);
}


function submitApplication() {
    // Retrieve the trainingId from the data attribute
    var trainingId = $('#ApplicationModal').data('trainingId');
    console.log('Submit Application js function');
    // Ensure that trainingId is not null or undefined
    if (!trainingId) {
        console.error('TrainingId is missing');
        return;
    }
    var formData = new FormData($('#applicationForm')[0]);
    formData.append('trainingId', trainingId);
    $.ajax({
        url: '/Application/SubmitApplication',  // Remove trainingId from the URL
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (data) {
            // Handle success
            alert("Successful");
        },
        error: function (error) {
            console.error(error);
        },
    });
}











