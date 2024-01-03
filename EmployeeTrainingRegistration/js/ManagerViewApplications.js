// Global variables to store applicant name and training title
var applicantName;
var trainingTitle;

$(document).ready(function () {
    $.ajax({
        url: '/Manager/GetApplicationsByManagerId',
        type: 'GET',
        datatype: 'json',
        success: function (data) {
            displayApplication(data.applications);
        },
        error: function (error) {
            console.error(error);
        },
    });
});

function displayApplication(data) {
    var applicationTable = $('#applicationTable');

    // Clear existing content in the table
    applicationTable.empty();

    // Add table headers
    applicationTable.append(`
        <thead>
            <tr>
                <th>Applicant's Name</th>
                <th>Training Title</th>
                <th>Application Status</th>
                <th>Action</th>
            </tr>
        </thead>
        <tbody>
    `);

    // Add rows for each training entry
    data.forEach(function (application) {
        var trainingHtml = `
            <tr>
                <td>${application["ApplicantName"]}</td>
                <td>${application["TrainingTitle"]}</td>
                <td>${application["ApplicationStatus"]}</td>
                <td>
                    <button class="read" type="button" onclick="viewDocument('${application["ApplicantName"]}', '${application["TrainingTitle"]}')">View Document</button> 
                    <button class="read" type="button" data-toggle="modal" data-target="#declineModal" onclick="prepareDeclineModal('${application["ApplicantName"]}', '${application["TrainingTitle"]}')">Decline</button>
                    <button class="read" type="button" onclick="approveApplication('${application["ApplicantName"]}', '${application["TrainingTitle"]}','${application["ApplicationID"]}')">Approve</button>
                </td>
            </tr>
        `;
        applicationTable.append(trainingHtml);
    });

    // Close the table body
    applicationTable.append(`</tbody>`);
}

// Function to prepare decline modal and store values
function prepareDeclineModal(applicant, title) {
    applicantName = applicant;
    trainingTitle = title;
}

// Function to handle the submit decline reason
function submitDeclineReason() {
    var declineReason = $('#declineReason').val();
    var data = {
        name: applicantName,
        title: trainingTitle,
        declineReason: declineReason
    };

    // Send the data to the server for processing
    $.ajax({
        url: '/Manager/DeclineApplication',
        type: 'POST',
        datatype: 'json',
        data: data,
        success: function (response) {
            // Display message in an alert box
            if (response.success) {
                alert('Application declined successfully!');
            } else {
                alert('Failed to decline application. Error: ' + response.message);
            }
        },
        error: function (error) {
            console.error(error);
        }
    });

    // Close the modal
    $('#declineModal').modal('hide');
}

// Function to handle the approve action
function approveApplication(applicantName, trainingTitle,applicationId) {
    var data = {
        name: applicantName,
        title: trainingTitle,
        applicationID: applicationId

    };

    // Send the data to the server for processing
    $.ajax({
        url: '/Manager/ApproveApplication',
        type: 'POST',
        dataType: 'json',
        data: data,
        success: function (response) {
            // Display message in an alert box
            if (response.success) {
                alert('Application approved successfully!');
            } else {
                alert('Failed to approve application. Error: ' + response.message);
            }
        },
        error: function (error) {
            console.error(error);
            // Handle error if needed
            alert('An error occurred while processing your request.');
        }
    });
}
