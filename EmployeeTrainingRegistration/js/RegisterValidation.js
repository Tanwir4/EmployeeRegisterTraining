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
        errors.mobNum = "Please enter Mobile Number.";
     }
     if (mobileNumber.trim()) {
         var isUnique = isMobileNumberUnique(mobileNumber);
         if (isUnique.success === false) { errors.mobNumUniqueness = isUnique.message; }
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
     if (!email.trim()) {
         errors.email = "Please enter Email.";
     }

     if (email.trim()) {
         var isUnique = isEmailUnique(email);
         if (isUnique.success === false) { errors.emailUniqueness = isUnique.message; }
     }
    if (nic.trim() && !/^[A-Za-z]/.test(nic.trim())) {
        errors.nicFormat = "NIC should start with a letter.";
    }
    if (nic.trim() && !/^[A-Za-z][0-9]{14}$/.test(nic.trim())) {
        errors.nicLength = "NIC should have a total length of 15 characters.";
     }
     if (nic.trim()) {
         var isUnique = isNicUnique(nic);
         if (isUnique.success === false) { errors.nicUniqueness = isUnique.message; }
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
function isNicUnique(nic) {
    var isNicUnique = false;
    $.ajax({
        url: '/Register/IsNicUnique',
        type: 'GET',
        data: { nic: nic },
        async: false,
        success: function (result) {
            isNicUnique = result;
        }
    });
    return isNicUnique;
}
function isMobileNumberUnique(mobileNumber) {
    var isMobileNumberUnique = false;
    $.ajax({
        url: '/Register/IsMobileUnique',
        type: 'GET',
        data: { mobileNumber: mobileNumber },
        async: false,
        success: function (result) {
            isMobileNumberUnique = result;
        }
    });
    return isMobileNumberUnique;
}







