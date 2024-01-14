$(document).ready(function () {
    $.ajax({
        url: '/Application/GetApplicationById',
        type: 'GET',
        datatype: 'json',
        success: function (data) {
            displayApplications(data.applications);
            $('#applicationTable').DataTable({
                "pageLength": 5,
                "lengthChange": false,
                "searching": true
            });
        },
        error: function (error) {
            console.error(error);
        },
    });
});
function displayApplications(data) {
    var applicationTable = $('#applicationTable');
    var tableBody = applicationTable.find('tbody');
    console.log('Display application function');

    data.forEach(function (application) {
        // Define badge based on application status
        var statusBadgeClass;
        switch (application["Status"]) {
            case "Approved":
                statusBadgeClass = 'bg-success';
                break;
            case "Declined":
                statusBadgeClass = 'bg-danger';
                break;
            case "Pending":
                statusBadgeClass = 'bg-warning';
                break;
            default:
                statusBadgeClass = 'bg-secondary';
        }
        var statusBadge = `<span class="badge ${statusBadgeClass} badge-font-size">${application["Status"]}</span>`;

        var rowHtml = `
            <tr>
                <td>${application["TrainingTitle"]}</td>
                <td>${statusBadge}</td>
            </tr>`;
        tableBody.append(rowHtml);
    });
}
