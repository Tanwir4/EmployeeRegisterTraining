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
        errors.managerName = "Please enter Manager Name.";
    }
    if (department === "Other") {
        errors.department = "Please select a valid department.";
    }
    if (!email.trim()) {
        errors.email = "Please enter Email.";
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
        return true;
    }
}

