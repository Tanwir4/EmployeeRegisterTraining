function validateForm() {
    var email = document.getElementsByName("Email")[0].value;
    var password = document.getElementsByName("Password")[0].value;
    var errors = {};

    if (!email.trim()) {
        errors.email = "Please enter an email address.";
    } else if (!/^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/.test(email)) {
        errors.email = "Invalid Email Address!";
    }

    if (!password.trim()) {
        errors.password = "Please enter a password.";
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
