var openup = true;
function Add() {
    var Add = document.getElementById("Add");
    Add.style.transition = '1s';
    if (Add.className == "fixed-top top-100 container") {
        Add.className = Add.className.replace("fixed-top top-100 container", "fixed-top top-50 translate-middle-y container");
        //openup = false;
        return 0;
    }
    else if (Add.className == "fixed-top top-50 translate-middle-y container") {
        Add.className = Add.className.replace("fixed-top top-50 translate-middle-y container", "fixed-top top-100 container");
        //openup = true;
        return 0;
    }
}

function Edit() {
    var Edit = document.getElementById("Edit");
    Edit.style.transition = '1s';
    if (Edit.className == "fixed-top top-100 container") {
        Edit.className = Edit.className.replace("fixed-top top-100 container", "fixed-top top-50 translate-middle-y container");
        //openup = false;
        return 0;
    }
    else if (Edit.className == "fixed-top top-50 translate-middle-y container") {
        Edit.className = Edit.className.replace("fixed-top top-50 translate-middle-y container", "fixed-top top-100 container");
        //openup = true;
        return 0;
    }
}

function Delete(value) {
    var inputDlt = document.getElementById("inputDlt").value = value;
    var Dlt = document.getElementById("Delete");
    Dlt.style.transition = "1s";
    if (Dlt.className == "fixed-top top-100 container") {
        Dlt.className = Dlt.className.replace("fixed-top top-100 container", "fixed-top top-50 translate-middle-y container");
        //openup = false;
        return 0;
    }
    else if (Dlt.className == "fixed-top top-50 translate-middle-y container") {
        Dlt.className = Dlt.className.replace("fixed-top top-50 translate-middle-y container", "fixed-top top-100 container");
        //openup = true;
        return 0;
    }
}

function Details(value) {
    var inputDetail = document.getElementById("inputDetail").value = value;
    var Dlt = document.getElementById("Details");
    Dlt.style.transition = "1s";
    if (Dlt.className == "fixed-top top-100 container") {
        Dlt.className = Dlt.className.replace("fixed-top top-100 container", "fixed-top top-50 translate-middle-y container");
        //openup = false;
        return 0;
    }
    else if (Dlt.className == "fixed-top top-50 translate-middle-y container") {
        Dlt.className = Dlt.className.replace("fixed-top top-50 translate-middle-y container", "fixed-top top-100 container");
        //openup = true;
        return 0;
    }
}

function updateDT(x) {
    const date = new Date(x);
    const day = date.getDate();
    const month = date.getMonth() + 1;
    const year = date.getFullYear();

    let h = date.getHours();
    const m = date.getMinutes();
    const s = date.getSeconds();
    const ampm = h >= 12 ? 'PM' : 'AM';
    h = h % 12;
    h = h ? h : 12;
    const minutes = m < 10 ? '0' + m : m;
    const seccond = s < 10 ? '0' + s : s;
    const result = `${month}/${day}/${year} ${h}:${minutes}:${seccond} ${ampm}`;
    return result;
}
