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
    var trainingCards = $('#trainingCards');
    data.forEach(function (training) {
        var trainingHtml = `
           <div class="card1">
              <div class="header">
                <span class="icon">
                </span>
                <p class="alert">${training["Title"]}</p>
              </div>
                <p class="message"> </p>
          <div class="actions button">
            <button class="read" onclick="openTrainingDetails(${training["TrainingID"]})" type="button">View</button>
            <button type="button" onclick="submitApplication(${training["TrainingID"]})" class="read">Apply</button>              
           </div>
        </div>`;
        trainingCards.append(trainingHtml);
    });
}
function openTrainingDetails(trainingId) {
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
            $('#trainingTitle').text(data.trainings.Title);
            $('#trainingPreRequisite').text(data.trainings.PreRequisite);
            $('#trainingDeadline').text(formattedDeadline);
            $('#trainingSeat').text(data.trainings.Threshold);
            $('#trainingStartDate').text(formattedStartDate);
            $('#trainingDetailsModal').modal('show');
        },
        error: function (error) {
            console.error(error);
        },
    });
}
function openApplication() {
    $('#ApplicationModal').modal('show');
}

function submitApplication(trainingId) {
    $.ajax({
        url: '/Application/SubmitApplication',
        type: 'POST',
        data: { trainingId: trainingId },
        datatype: 'json',
        success: function (data) {
        },
        error: function (error) {
            console.error(error);
        },
    });
}
