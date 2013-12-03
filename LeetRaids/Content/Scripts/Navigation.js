function showNav(listElm) {
    listElm.children("li.unselected").show();
}

function hideNav(listElm) {
    listElm.children(".unselected").hide();
}

function activate(imgLink) {
    var bgImage = imgLink.style.backgroundImage;
    if (bgImage.indexOf("_active") == -1) {
        var fileFormatStartIndex = bgImage.lastIndexOf(".");
        var fileLocationAndName = bgImage.substring(0, fileFormatStartIndex);
        var fileFormat = bgImage.substr(fileFormatStartIndex, bgImage.length - fileFormatStartIndex);
        var newBGImage = fileLocationAndName + "_active" + fileFormat;
        //alert(newBGImage);
        imgLink.style.backgroundImage = newBGImage;
    }
}

function deactivate(imgLink) {
    var bgImage = imgLink.style.backgroundImage;
    if (bgImage.indexOf("_active") != -1) {
        var newBGImage = bgImage.replace("_active", "");
        //alert(newBGImage);
        imgLink.style.backgroundImage = newBGImage;
    }
}