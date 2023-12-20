﻿$(document).ready(function () {
    $.ajax({
        url: '/Application/GetApplicationById',
        type: 'GET',
        datatype: 'json',
        success: function (data) {
            displayApplications(data.applications);
        },
        error: function (error) {
            console.error(error);
        },
    });
});

function displayApplications(data) {
    var applicationCards = $('#applicationCards');
    data.forEach(function (application) {
        var applicationHtml = `
           <div class="card2">
              <div class="header">
                <span class="icon">
                </span>
                <p class="alert">${application["TrainingTitle"]}</p>
              </div>
                <p class="message">Status: ${application["Status"]}</p>
        </div>`;
        applicationCards.append(applicationHtml);
    });
}