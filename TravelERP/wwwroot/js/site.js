//*** For SubMenu Dropdown List in Navbar Start ***

    $('.dropdown-submenu > a').on("click", function (e) {
        var submenu = $(this);
    $('.dropdown-submenu .dropdown-menu').removeClass('show');
    submenu.next('.dropdown-menu').addClass('show');
    e.stopPropagation();
});

//*** For SubMenu Dropdown List in Navbar End ***

//*** For Disable Button While Post Start ***

function DisableButtons() {
    var inputs = document.getElementsByTagName("INPUT");
    for (var i in inputs) {
        if (inputs[i].type == "button" || inputs[i].type == "submit") {
            inputs[i].disabled = true;
        }
    }
}
window.onbeforeunload = DisableButtons;
//*** For Disable Button While Post Start ***
