 function validateRegisterForm() {
    var firstName = document.getElementsByName("FirstName")[0].value;
    var lastName = document.getElementsByName("LastName")[0].value;
    var mobileNumber = document.getElementsByName("MobileNumber")[0].value;
    var nic = document.getElementsByName("NationalIdentityCard")[0].value;
    var managerName = document.getElementsByName("ManagerName")[0].value;
    var department = document.getElementById("department").value;
    var email = document.getElementsByName("Email")[0].value;
    var password = document.getElementsByName("Password")[0].value;
    var errors = {};
    if (!firstName.trim()) {
        errors.firstName = "Please enter First Name.";
    }
    if (!lastName.trim()) {
        errors.lastName = "Please enter Last Name.";
    }
    if (!mobileNumber.trim()) {
        errors.mobileNumber = "Please enter Mobile Number.";
    }
    if (!nic.trim()) {
        errors.nic = "Please enter NIC.";
    }
    if (!managerName.trim()) {
        errors.managerName = "Please select Manager Name.";
    }
    if (department === "Other") {
        errors.department = "Please select a valid department.";
     }

/*     if (!email.trim()) {
         errors.email = "Please enter Email.";
     } else {
         isEmailUnique(email)
             .then(isUnique => {
                 if (!isUnique) {
                     errors.email = "Email already exists.";
                 }
             })
             .catch(error => {
                 // Handle the error if needed
             });
     }*/
    
     if (!email.trim()) {
         errors.email = "Please enter Email.";
     }

     if (email.trim()) {
         var isUnique = isEmailUnique(email);
         if (isUnique.success === false) { errors.emailUniqueness = isUnique.message; }
     }
     
//    isEmailUnique(email, function (isUnique) {
//        if (!isUnique) {
//            errors.email = "Email already exists.";
//        }

//})
    if (nic.trim() && !/^[A-Za-z]/.test(nic.trim())) {
        errors.nicFormat = "NIC should start with a letter.";
    }
    if (nic.trim() && !/^[A-Za-z][0-9]{14}$/.test(nic.trim())) {
        errors.nicLength = "NIC should have a total length of 15 characters.";
    }
    if (!password.trim()) {
        errors.password = "Please enter Password.";
    }

    var errorDiv = document.getElementById("errorMessages");
    errorDiv.innerHTML = "";
    if (Object.keys(errors).length > 0) {
        for (var key in errors) {
            errorDiv.innerHTML += "<p>" + errors[key] + "</p>";
        }
        return false;
    } else {
        showToaster("Registration successful!");
        setTimeout(function () {
            window.location.href = "/Login/Login";
        }, 3000);
        return true;
    }
}
function showToaster(message) {
    var toaster = document.getElementById("toaster");
    var toasterMessage = document.getElementById("toasterMessage");
    toasterMessage.innerText = message;
    toaster.style.display = "block";
    setTimeout(function () {
        toaster.style.display = "none";
    }, 3000);
}

//function isEmailUnique(email, callback) {
//    $.ajax({
//        url: `/Register/IsEmailUnique?email=${email}`,
//        type: 'GET',
//        dataType: 'json',
//        success: function (data) {
//            callback(data.success);
//        },
//        error: function () {
//            callback(false);
//        }
//    });


//}

/*function isEmailUnique(email) {

    var formData = new FormData()
    formData.append('email', email)
    fetch('/Register/IsEmailUnique', {
        method: 'get',
        body: formData,
        async:false
    })
        .then(response => response.json())
        .then(data => {
            return data.success
        })
        .catch((error) => {
            console.error("Error, ", error)
            return false
        })
}*/

function isEmailUnique(email) {
    var isEmailUnique = false;
    $.ajax({
        url: '/Register/IsEmailUnique',
        type: 'GET',
        data: { email: email },
        async: false,
        success: function (result) {
            isEmailUnique = result;
        }
    });
    return isEmailUnique;
}







