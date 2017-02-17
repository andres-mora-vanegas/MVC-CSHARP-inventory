$(".ajaxProcessSingle").change(function () {
    var datax = {};
    datax.id = $(this).val();
    datax.action = $(this).attr("dataAction");
    datax.owner = $(this).attr("dataOwner");
    if (datax.id != "") {
        var uri = "/" + datax.owner + "/" + datax.action;
        if ($(this).attr("dataBefore") != null) {
            var a = $(this).attr("dataBefore");
            $.ajax({
                url: uri,
                type: 'post',
                data: datax
            }).done(function (response) {
                window[a](response)
            });
        }
        else {
            $.ajax({
                url: uri,
                type: 'post',
                data: datax
            }).done(function (response) {
                //console.log(response);
                bootbox.alert(response);
                //$("#quantityInfo").html(response);
            });
        }
    }
    else {
        bootbox.alert(noData);
    }
})

$(document).on("click", ".ajaxProcessSingleC", function () {
    addToJson();
    var datax = {};
    datax.id = json;
    datax.action = $(this).attr("dataAction");
    datax.owner = $(this).attr("dataOwner");
    console.log(datax.id);
    if (datax.id != "") {
        var uri = "/" + datax.owner + "/" + datax.action;
        if ($(this).attr("dataBefore") != null) {
            var a = $(this).attr("dataBefore");
            $.ajax({
                url: uri,
                type: 'post',
                data: datax
            }).done(function (response) {
                window[a](response)
            });
        }
        else {
            $.ajax({
                url: uri,
                type: 'post',
                data: datax
            }).done(function (response) {
                bootbox.alert(response);
            }).fail(function () {
                bootbox.alert(error);
            })
        }
    }
    else {
        bootbox.alert(noData);
    }
})