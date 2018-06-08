﻿var _timeoutcount = 500;//ms
// textbox search
var Nvs001txt_search_app_code = document.getElementById('Nvs001txt_search_app_code');
// div hiển thị commbox search sau khi tìm kiếm
var NVS002_DivShowSearchAppCode = $("#NVS002_DivShowSearchAppCode");

// Init a timeout variable to be used below
var timeout = null;

// Listen for keystroke events
Nvs001txt_search_app_code.onkeyup = function (e) {

    // Clear the timeout if it has already been set.
    // This will prevent the previous task from executing
    // if it has been less than <MILLISECONDS>
    clearTimeout(timeout);

    // Make a new timeout set to go off in 800ms
    timeout = setTimeout(function () {
       // alert('Input Value:' + Nvs001txt_search_app_code.value);
        // tìm kiếm ở đoạn này
        NVSFunc001ShowComboxSearch()
    }, _timeoutcount);
};

function NVSFunc001ShowComboxSearch()
{
    try {
        if (Nvs001txt_search_app_code.value == "")
            return;
        $.ajax({
            type: "POST",
            url: "/quan-ly-thong-tin/hang-hoa-dich-vu/combobox-search",
            headers: { "cache-control": "no-cache" },
            data: { p_search: Nvs001txt_search_app_code.value },
            async: false,
            success: function (data) {
                if (data != null) {
                    if (validateResponse(data)) {
                        NVS002_DivShowSearchAppCode.html(data);
                    }
                }
                return false;
            }
        });
    } catch (e) {
        alert(e.toString())
    }
}

