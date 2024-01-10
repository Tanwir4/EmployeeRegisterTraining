﻿// Global variables to store applicant name and training title
var applicantName;
var trainingTitle;

$(document).ready(function () {
    toastr.options = {
        closeButton: true,
        progressBar: true,
        positionClass: 'toast-top-right',
        timeOut: 3000
    };
    $.ajax({
        url: '/Manager/GetApplicationsByManagerId',
        type: 'GET',
        datatype: 'json',
        success: function (data) {
            displayApplication(data.applications);
            $('#applicationTable').DataTable({
                "pageLength": 10,
                "lengthChange": false,
                "searching": true
            });
        
        },
        error: function (error) {
            console.error(error);
        },
    });
});

function displayApplication(data) {
    var applicationTable = $('#applicationTable');
    var tableBody = applicationTable.find('tbody'); 
    console.log('Display trainings function');


    // Add rows for each training entry
    data.forEach(function (application) {
        var trainingHtml = `
            <tr>
                <td>${application["ApplicantName"]}</td>
                <td>${application["TrainingTitle"]}</td>
                <td>${application["ApplicationStatus"]}</td>
                <td>
                    <button class="read viewDocumentBtn" type="button" onclick="viewDocument('${application["ApplicantName"]}', '${application["TrainingTitle"]}', '${application["ApplicationID"]}')">View Document</button>
                    <button class="read declineBtn" type="button" data-toggle="modal" data-target="#declineModal" onclick="prepareDeclineModal('${application["ApplicantName"]}', '${application["TrainingTitle"]}','${application["ApplicationID"]}')">Decline</button>
                    <button class="read approveBtn" type="button" onclick="approveApplication('${application["ApplicantName"]}', '${application["TrainingTitle"]}','${application["ApplicationID"]}')">Approve</button>
                </td>
            </tr>
        `;
        tableBody.append(trainingHtml);
    });
}

// Function to prepare decline modal and store values
function prepareDeclineModal(applicant, title, applicationId) {
    applicantName = applicant;
    trainingTitle = title;
    appID = applicationId;
}

// Function to handle the submit decline reason
function submitDeclineReason() {
    var declineReason = $('#declineReason').val();
    var data = {
        name: applicantName,
        title: trainingTitle,
        declineReason: declineReason,
        applicationID: appID
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
                // Handle the success response from the server
                toastr.success('Training successfully declined!');

                // Reload the page to see the changes
                setTimeout(function () {
                    location.reload();
                }, 3000);
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
function approveApplication(applicantName, trainingTitle, applicationId) {
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
            

            if (response.success) {
                // Handle the success response from the server
                toastr.success('Training successfully approved!');

                // Reload the page to see the changes
                setTimeout(function () {
                    location.reload();
                }, 3000);
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

function DownloadAttachment(attachmentID) {
    $.ajax({
        url: `/Manager/DownloadAttachment`,
        method: 'GET',
        data: { attachmentID: attachmentID },
        xhrFields: {
            responseType: 'blob' // Set the response type to 'blob'
        },
        success: function (data) {
            // Create a Blob from the response data
            var blob = new Blob([data], { type: 'application/octet-stream' });

            // Create a temporary link element
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = `attachment_${attachmentID}.pdf`; // Set the download attribute with a suitable filename

            // Append the link to the document, trigger the click event, and remove the link
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        },
        error: function () {
            // Redirect to the error page in case of an error
            window.location.href = "/Common/InternalServerError";
        }
    });
}

function viewDocument(applicantName, trainingTitle, applicationID) {
    // Display basic information in an alert
    var message = `Applicant's Name: ${applicantName}\nTraining Title: ${trainingTitle}\nApplication ID: ${applicationID}`;
    alert(message);

    // Fetch attachments for the specified ApplicationID
    $.ajax({
        url: `/Manager/GetAttachmentsByApplicationID`,
        method: 'GET',
        data: { applicationID: applicationID },
        dataType: 'json',
        success: function (data) {
            console.log(data);
            data.Attachments.forEach(function (attachmentID) {
                DownloadAttachment(attachmentID);
            });
        },
        error: function (xhr, status, error) {
            // Log the error details in the console
            console.error("Error:", xhr.responseText);
            console.error("Status:", status);
            console.error("Error object:", error);
        }
    });
}