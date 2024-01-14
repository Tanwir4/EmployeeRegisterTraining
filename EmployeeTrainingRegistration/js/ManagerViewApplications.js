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
function prepareDeclineModal(applicant, title, applicationId) {
    applicantName = applicant;
    trainingTitle = title;
    appID = applicationId;
}
function submitDeclineReason() {
    var declineReason = $('#declineReason').val();
    var data = {
        name: applicantName,
        title: trainingTitle,
        declineReason: declineReason,
        applicationID: appID
    };
    $.ajax({
        url: '/Manager/DeclineApplication',
        type: 'POST',
        datatype: 'json',
        data: data,
        success: function (response) {
            if (response.success) {
                toastr.success('Training successfully declined!');

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

    $('#declineModal').modal('hide');
}

function approveApplication(applicantName, trainingTitle, applicationId) {
    var data = {
        name: applicantName,
        title: trainingTitle,
        applicationID: applicationId

    };

    $.ajax({
        url: '/Manager/ApproveApplication',
        type: 'POST',
        dataType: 'json',
        data: data,
        success: function (response) {
            

            if (response.success) {
                toastr.success('Training successfully approved!');
                setTimeout(function () {
                    location.reload();
                }, 3000);
            } else {
                alert('Failed to approve application. Error: ' + response.message);
            }
        },
        error: function (error) {
            console.error(error);
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
            responseType: 'blob' 
        },
        success: function (data) {
            var blob = new Blob([data], { type: 'application/octet-stream' });
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = `attachment_${attachmentID}.pdf`; 
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        },
        error: function () {
            window.location.href = "/Common/InternalServerError";
        }
    });
}

function viewDocument(applicantName, trainingTitle, applicationID) {
    var message = `Applicant's Name: ${applicantName}\nTraining Title: ${trainingTitle}\nApplication ID: ${applicationID}`;
    alert(message);
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
            console.error("Error:", xhr.responseText);
            console.error("Status:", status);
            console.error("Error object:", error);
        }
    });
}