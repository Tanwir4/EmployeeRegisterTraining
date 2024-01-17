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
            url: '/Training/StartAutomaticProcessing',
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
    $.ajax({
        url: '/Training/GetAllTrainingForAdmin', 
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            var trainingTableBody = $('#trainingTable tbody');
            $.each(data.trainings, function (index, training) {
                var isExpired = checkIfTrainingExpired(training.TrainingID);
                var statusBadge = isExpired
                    ? '<span class="badge bg-success">Active</span>'
                    : '<span class="badge bg-danger">Expired</span>';
                var row = '<tr>' +
                    '<td>' + statusBadge + '</td>' +
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
                $.ajax({
                    url: '/Training/GetTrainingById',
                    type: 'GET',
                    data: { trainingId: trainingId },
                    dataType: 'json',
                    success: function (data) {
                        $('#trainingDetailsModal').modal('show');
                        $('#editTitle').val(data.trainings.Title);
                        var startDate = new Date(parseInt(data.trainings.StartDate.substr(6)));
                        var deadline = new Date(parseInt(data.trainings.Deadline.substr(6)));
                        $('#editDate').val(formatDate(startDate));
                        $('#editThreshold').val(data.trainings.Threshold);
                        $('#editDescription').val(data.trainings.Description);
                        $('#editDeadline').val(formatDate(deadline));
                        $('#departmentDropdown').val(data.trainings.DepartmentName);

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
                var editedStartDate = $('#editDate').val();
                var editedDeadline = $('#editDeadline').val();
                var editedThreshold = $('#editThreshold').val();
                var editedPreRequisite = $('#editDescription').val();
                var editedDepartment = $('#departmentDropdown').val();
                var checkedPrerequisites = [];
                $('input[type="checkbox"]:checked').each(function () {
                    checkedPrerequisites.push($(this).val());
                });
                var data = {
                    TrainingId: trainingId,
                    Title: editedTitle,
                    StartDate: editedStartDate,
                    Threshold: editedThreshold,
                    Description: editedPreRequisite,
                    DepartmentName: editedDepartment,
                    Deadline: editedDeadline,
                    PreRequisite: checkedPrerequisites

                };
                console.log(data);
                $.ajax({
                    type: 'POST',
                    url: '/Training/UpdateTraining',
                    data: data,
                    success: function (result) {
                        toastr.success('Training changes saved successfully!');
                        setTimeout(function () {
                            location.reload();
                        }, 1000);
                    },
                    error: function () {
                        toastr.error('Error occurred while saving changes.');
                    }
                });
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
                $.ajax({
                    type: 'POST',
                    url: '/Training/AddTraining',
                    data: data,
                    success: function (result) {
                        toastr.success('New Training added successfully!');
                        setTimeout(function () {
                            location.reload();
                        }, 1000);
                    },
                    error: function () {
                        alert('Error occurred while saving changes.');
                    }
                });
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
                            //console.log('Training cannot be deleted');
                            toastr.success('Training successfully deleted!');
                            setTimeout(function () {
                                location.reload();
                            }, 1000);
                        }
                        else {
                            //console.log('Training deleted successfully');
                            toastr.error('Training cannot be deleted!');
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
                            selectedEmployeesTable.clear().draw();
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

            $('#exportBtn').on('click', function () {
                selectedEmployeesTable.buttons(0).trigger();
            });
            $('.buttons-excel').hide();

            $('#trainingTable').DataTable({
                "pageLength": 6,
                "lengthChange": false,
                "searching": true

            });
            $('.addTrainingButton').on('click', function (event) {
                console.log('Add Training button clicked');
                $('#AddTrainingModal').modal('show');
                $.ajax({
                    url: '/Training/GetAllPreRequisites', 
                    type: 'GET',
                    success: function (data) {
                        $('#trainingPrerequisites').empty();
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
function formatDate(date) {
    var year = date.getFullYear();
    var month = (date.getMonth() + 1).toString().padStart(2, '0');
    var day = date.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
}

function checkIfTrainingExpired(trainingId) {
    var isExpired = false;
    $.ajax({
        url: '/Training/IsTrainingExpired',
        type: 'GET',
        data: { trainingId: trainingId },
        async: false,
        success: function (result) {
            isExpired = result;
        }
    });
    return isExpired;
}