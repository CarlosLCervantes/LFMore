function isEmpty(val) {
    if (val.length > 0) { return false; }
    else { return true; }
}

function isNotEmpty(val) {
    if (val.length > 0) { return true; }
    else { return false; }
}

function isAlpha(val) {
    var myRegex = new RegExp("^[a-zA-Z]+$");
    var result = myRegex.test(val);

    return result;
}

//Alpha or Spaces
function isParagraph(val) {
    var result = !isEmpty(val);

    if (result) {
        var myRegex = new RegExp("\\w+");
        result = myRegex.test(val);
    }

    return result;
}

function isNumber(val){
    //TODO: Deny 0
    var myRegex = new RegExp("^\\d+$");
    var result = myRegex.test(val);

    return result;
}

function isZip(val) {
    //var myRegex = new RegExp("\d{5}(-\d{4})?");
    var myRegex = new RegExp("\\d{5}");
    var result = myRegex.test(val);
    return result;
}

function isPhone(val) {
    var myRegex = new RegExp("\\(\\d{3}\\)\\d{3}-\\d{4}");
    var result = myRegex.test(val);

    return result;
}

function isEmail(val) {
    var myRegex = new RegExp("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*");
    var result = myRegex.test(val);

    return result;
}

function isDate(val) {
    var myRegex = new RegExp("(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\\d\\d");
    var result = myRegex.test(val);

    return result;
}

/*Dynamic Validate*/
function validate(sender, type, errorMsg) {
    var valid = false;
    if(type == "singleName"){
        valid = !isEmpty(sender.val());
    }
    else if (type == 'number') {
        valid = isNumber(sender.val());
    }

    var errorMsgContainer = $("#" + errorMsg);
    if (errorMsgContainer.length > 0) {
        if (valid) { errorMsgContainer.css('visibility', 'hidden') }
        else { errorMsgContainer.css('visibility', 'visible'); }
    }

    return valid;

}

function val_el(value, valFnc, elErrorMsgID, displayMode) {
    var errorMsgContainer = $("#" + elErrorMsgID);
    var valid = valFnc(value);
    if (displayMode == 'dynamic') {
        if (valid) { errorMsgContainer.css('display', 'none') }
        else { errorMsgContainer.css('display', 'inline'); }
    }
    else {
        if (valid) { errorMsgContainer.css('visibility', 'hidden') }
        else { errorMsgContainer.css('visibility', 'visible'); }
    }
}

function val_msg(value, valFnc, elErrorMsgID, errorMsg) {
    var errorMsgContainer = $("#" + elErrorMsgID);
    var valid = valFnc(value);
    if (valid) {errorMsgContainer.html(errorMsg); }
    else {errorMsgContainer.html(""); }
}
/*********************Below This line coding to specific implimentation***************************/
function val_FName(txtNameBox) {
    var name = txtNameBox.value;
    var isValid = false;
    var val = document.getElementById('valFirstName');

    if (isAlpha(name)) {
        val.style.visibility = "hidden";
        isValid = true;
    }
    else {
        val.style.visibility = "visible";
        isValid = false;
    }

    return isValid;
}

function val_LName(txtNameBox) {
    var name = txtNameBox.value;
    var isValid = false;
    var val = document.getElementById('valLastName');

    if (isAlpha(name)) {
        val.style.visibility = "hidden";
        isValid = true;
    }
    else {
        val.style.visibility = "visible";
        isValid = false;
    }

    return isValid;
}

function val_Zip(txtZip) {
    var zip = txtZip.value;
    var isValid = false;
    var val = document.getElementById('valZip');

    if (isZip(zip)) {
        val.style.visibility = "hidden";
        isValid = true;
    }
    else {
        val.style.visibility = "visible";
        isValid = false;
    }

    return isValid;
}

function val_Address(txtAddress) {
    var address = txtAddress.value;
    var isValid = false;
    var val = document.getElementById('valAddress');

    if (isEmpty(address)) {
        val.style.visibility = "visible";
        isValid = false;
    }
    else {
        val.style.visibility = "hidden";
        isValid = true;
    }

    return isValid;
}

function val_Phone(txtPhone) {
    var phone = txtPhone.value;
    var isValid = false;
    var val = document.getElementById('valPhone');

    if (isPhone(phone)) {
        val.style.visibility = "hidden";
        isValid = true;
    }
    else {
        val.style.visibility = "visible";
        isValid = false;
    }

    return isValid;
}

function val_Email(txtEmail) {
    var email = txtEmail.value;
    var isValid = false;
    var val = document.getElementById('valEmail');

    if (isEmail(email)) {
        val.style.visibility = "hidden";
        isValid = true;
    }
    else {
        val.style.visibility = "visible";
        isValid = false;
    }

    return isValid;
}

function val_userName(txtUserName) {
    var userName = txtUserName.value;
    var isValid = true;
    var val = document.getElementById('valUserName');

    if (userName.length < 5 || userName.length > 15) {
        isValid = false;
    }

    if (isValid) {
        val.style.visibility = "hidden";
    }
    else {
        val.style.visibility = "visible";
    }

    return isValid;
}

function val_PasswordConfirm(txtPasswordID, txtPasswordConfirm) {
    var password = document.getElementById(txtPasswordID).value;
    var confirmPassword = txtPasswordConfirm.value;
    var isValid;

    var val = document.getElementById('valPasswordConfirm');

    if (password == confirmPassword) {
        val.style.visibility = "hidden";
        isValid = true;
    }
    else {
        val.style.visibility = "visible";
        isValid = false;
    }

    return isValid;
}

function val_IsNotEmpty(textBox, validatorMsgID) {
    var validatorMessage = document.getElementById(validatorMsgID);
    if (isEmpty(textBox.value)) {
        validatorMessage.style.visibility = "visible";
    }
    else {
        validatorMessage.style.visibility = "hidden";
    }


}



function selectTxt(txt) {
    txt.style.border = "solid 3px Gold";
}

function unSelectTxt(txt) {
    txt.style.border = "0px";
}